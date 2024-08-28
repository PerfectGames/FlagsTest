using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlagsTest
{
    public class GameController :Singleton<GameController>
    {
        [SerializeField] Transform _PlaneTransform;
        [SerializeField] bool _AiEnabledForDebug;
        [SerializeField] int _AiCount = 2;

        Dictionary<Team, List<Flag>> TeamFlags = new Dictionary<Team, List<Flag>>();
        Dictionary<Team, List<Player>> TeamPlayers = new Dictionary<Team, List<Player>>();

        List<MiniGame> MiniGames = new List<MiniGame>();
        List<Flag> AllFlags = new List<Flag>();
        List<Player> AllPlayers = new List<Player>();
        Queue<Team> FreeTeams = new Queue<Team>();

        LevelDescription Level;
        Vector2 HalfLivelSize;

        public event Action<MiniGame> OnStartMiniGame;
        public event Action<MiniGame, bool> OnEndMiniGame;

        public float SqrFlagRadius { get; private set; }

        public IEnumerable<Flag> GetAllFlags
        {
            get
            {
                foreach (var flag in AllFlags)
                {
                    yield return flag;
                }
            }
        }

        void Start ()
        {
            Level = WL.SelectedLevel;

            HalfLivelSize = Level.LevelSize / 2f;
            SqrFlagRadius = Mathf.Pow (Level.FlagRadius, 2);

            _PlaneTransform.localScale = new Vector3 (Level.LevelSize.x, 1, Level.LevelSize.y);

            var localPlayer = CreatePlayer ();

            var playerController = Instantiate (B.ResourcesSettings.PlayerControllerRef);
            playerController.Initialize (localPlayer);

            if (_AiEnabledForDebug)
            {
                for (int i = 0; i < _AiCount; i++)
                {
                    var aiPlayer = CreatePlayer ();
                    var ai = aiPlayer.gameObject.AddComponent<AIControl> ();
                    ai.Initialize (aiPlayer);
                }
            }
        }

        void FixedUpdate ()
        {
            FixedUpdateCheckPlayersInRadius ();
            bool hasMiniGameForRemove = false;

            foreach (var miniGmae in MiniGames)
            {
                miniGmae.FixedUpdate ();
                hasMiniGameForRemove |= miniGmae.IsCompleted;
            }

            if (hasMiniGameForRemove)
            {
                MiniGames.RemoveAll (g => g.IsCompleted);
            }
        }

        public void TryStartMiniGameForPlayer (Player player)
        {
            float probability = Random.Range (0f, 1f);
            if (probability < Level.MiniGame.MiniGameProbability)
            {
                if (Level.MiniGame is SliderMiniGameDescription)
                {
                    var miniGame = new Slider_MiniGame ();
                    miniGame.Initialize (player, Level.MiniGame, OnCompleteMiniGame);
                    MiniGames.Add (miniGame);
                    OnStartMiniGame?.Invoke (miniGame);
                }
            }
        }

        void OnCompleteMiniGame (MiniGame miniGame, bool success)
        {
            if (!success)
            {
                miniGame.Player.ActivatePenalty (miniGame.MiniGameDescription.PenaltyDuration);
            }

            OnEndMiniGame?.Invoke (miniGame, success);
        }

        void OnCaptureFlag (Flag flag)
        {

        }

        Player CreatePlayer ()
        {
            if (FreeTeams.Count == 0)
            {
                foreach (var team in B.GameSettings.Teams)
                    FreeTeams.Enqueue (team);
            }

            var playerTeam = FreeTeams.Dequeue();
            var player = Instantiate (B.ResourcesSettings.PlayerRef);
            CreateFlagsForTeamIfNeeded (playerTeam);

            if (!TeamPlayers.TryGetValue (playerTeam, out var teamPlayers))
            {
                teamPlayers = new List<Player> ();
                TeamPlayers[playerTeam] = teamPlayers;
            }

            if (TeamFlags.TryGetValue (playerTeam, out var flags) && flags.Count > 0)
            {
                player.Position = flags[Random.Range(0, flags.Count - 1)].Position;
            }
            else if (teamPlayers.Count > 0)
            {
                player.Position = teamPlayers[Random.Range (0, teamPlayers.Count - 1)].Position;
            }
            else
            {
                player.Position = Vector3.zero;
            }

            player.SetTeam (playerTeam);

            teamPlayers.Add (player);
            AllPlayers.Add (player);

            return player;
        }

        void CreateFlagsForTeamIfNeeded (Team team)
        {
            if (TeamFlags.ContainsKey (team))
            {
                return;
            }

            float doubleSqrRadius = Mathf.Pow (Level.FlagRadius * 2, 2);
            List<Flag> newFlags = new List<Flag> ();

            Vector3 randomPos;
            for (int i = 0; i < Level.FlagsForTeamsCount; i++)
            {
                bool intersectionFlags = false;
                int iterationCounter = 10;

                do
                {
                    float randomX = Random.Range (-HalfLivelSize.x + Level.FlagRadius, HalfLivelSize.x - Level.FlagRadius);
                    float randomZ = Random.Range (-HalfLivelSize.y + Level.FlagRadius, HalfLivelSize.y - Level.FlagRadius);
                    randomPos = new Vector3 (randomX, 0, randomZ);
                    foreach (var existFlag in AllFlags)
                    {
                        intersectionFlags = (existFlag.Position - randomPos).sqrMagnitude < doubleSqrRadius;
                        if (intersectionFlags)
                        {
                            break;
                        }
                    }
                    iterationCounter--;
                } 
                while (intersectionFlags && iterationCounter > 0);

                var flag = Instantiate (B.ResourcesSettings.FlagRef);
                flag.Position = randomPos;
                flag.SetTeam (team);
                flag.SetRadius (Level.FlagRadius);

                newFlags.Add (flag);
                AllFlags.Add (flag);
            }

            TeamFlags[team] = newFlags;
        }

        void FixedUpdateCheckPlayersInRadius ()
        {
            foreach (var player in AllPlayers)
            {
                if (player.InRadiusFlag)
                {
                    if ((player.InRadiusFlag.Position - player.Position).sqrMagnitude > SqrFlagRadius)
                    {
                        player.InRadiusFlag.RemovePlayerFromRadius (player);
                        player.InRadiusFlag = null;
                    }
                }
                else
                {
                    float sqrDistance;
                    float minSqrDistance = float.MaxValue;
                    Flag nearestFlag = null;
                    foreach (var flag in AllFlags)
                    {
                        sqrDistance = (flag.Position - player.Position).sqrMagnitude;
                        if (minSqrDistance > sqrDistance)
                        {
                            minSqrDistance = sqrDistance;
                            nearestFlag = flag;
                        }
                    }

                    if (minSqrDistance < SqrFlagRadius && nearestFlag != null)
                    {
                        nearestFlag.AddPlayerInRadius (player);
                        player.InRadiusFlag = nearestFlag;
                    }
                }
            }
        }

        public Vector3 CheckZoneCollision (Vector3 position, float radius)
        {
            position.x = Mathf.Clamp (position.x, -HalfLivelSize.x + radius, HalfLivelSize.x - radius);
            position.y = 0;
            position.z = Mathf.Clamp (position.z, -HalfLivelSize.y + radius, HalfLivelSize.y - radius);

            return position;
        }
    }
}