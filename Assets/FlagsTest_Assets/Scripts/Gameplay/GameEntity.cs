using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlagsTest
{
    public class GameEntity
    {
        List<Player> AllPlayers = new List<Player>();
        List<Flag> AllFlags = new List<Flag>();
        List<ILogicUpdatable> AllLogicUpdatable = new List<ILogicUpdatable>();
        List<IVisualUpdatable> AllVisualUpdatable = new List<IVisualUpdatable>();

        LevelDescription Level => WL.SelectedLevel;
        Vector2 HalfLivelSize;
        List<Vector2Int> CellsForFlags = new List<Vector2Int>();

        public static GameEntity Instance { get; private set; }
        public float SqrFlagRadius { get; private set; }
        public IEnumerable<Flag> GetAllFlags => AllFlags;

        public GameEntity ()
        {
            Instance = this;

            HalfLivelSize = Level.LevelSize / 2f;
            SqrFlagRadius = Mathf.Pow (Level.FlagRadius, 2);
        }

        public Player CreatePlayer (Team playerTeam)
        {
            var player = GameObject.Instantiate (B.ResourcesSettings.PlayerRef);

            List<TeamObject> teamObjects = new List<TeamObject>();
            teamObjects.AddRange (AllFlags.Where (f => f.Team == playerTeam));

            if (teamObjects.Count == 0)
            {
                teamObjects.AddRange (AllFlags.Where (p => p.Team == playerTeam));
            }

            if (teamObjects.Count > 0)
            {
                player.Position = teamObjects[Random.Range (0, teamObjects.Count)].Position;
            }
            else
            {
                player.Position = Vector3.zero;
            }

            player.SetTeam (playerTeam);

            player.name = $"Player_{playerTeam}_{AllPlayers.Count}";
            return player;
        }

        public List<Flag> CreateFlagsForTeam (Team team)
        {
            var result = new List<Flag> ();

            Vector2 halfLivelSize = WL.SelectedLevel.LevelSize / 2;

            float cellSize = WL.SelectedLevel.FlagRadius * 2;
            Vector2Int cells = new Vector2Int();
            cells.x = (int)(WL.SelectedLevel.LevelSize.x / cellSize) - 1;
            cells.y = (int)(WL.SelectedLevel.LevelSize.y / cellSize) - 1;

            for (int i = 0; i < WL.SelectedLevel.FlagsForTeamsCount; i++)
            {
                bool intersectionFlags = false;

                Vector2Int randomCell = new Vector2Int(Random.Range (0, cells.x), Random.Range (0, cells.y));
                Vector2Int newFlagCell = new Vector2Int ();
                for (int x = 0; x < cells.x; x++)
                {
                    for (int y = 0; y < cells.y; y++)
                    {
                        newFlagCell = new Vector2Int ((randomCell.x + x).Repeat (0, cells.x), (randomCell.y + y).Repeat (0, cells.y));
                        intersectionFlags = CellsForFlags.Contains (newFlagCell);
                        if (!intersectionFlags)
                        {
                            break;
                        }
                    }
                    if (!intersectionFlags)
                    {
                        break;
                    }
                }

                CellsForFlags.Add (newFlagCell);

                var flag = GameObject.Instantiate (B.ResourcesSettings.FlagRef);
                flag.Position = new Vector3 
                (
                    newFlagCell.x * cellSize - halfLivelSize.x + WL.SelectedLevel.FlagRadius,
                    0,
                    newFlagCell.y * cellSize - halfLivelSize.y + WL.SelectedLevel.FlagRadius
                );

                flag.SetTeam (team);

                flag.name = $"Flag_{team}_{AllFlags.Count}";
                result.Add (flag);
            }

            return result;
        }

        public void OnCreateTeamObject (TeamObject teamObject)
        {
            if (teamObject is Player)
            {
                AllPlayers.Add (teamObject as Player);
            }
            else if (teamObject is Flag)
            {
                AllFlags.Add (teamObject as Flag);
            }

            AllLogicUpdatable.Add (teamObject);
            AllVisualUpdatable.Add (teamObject);
        }

        public void VisualUpdate ()
        {
            foreach (var updatable in AllVisualUpdatable)
            {
                updatable.VisualUpdate ();
            }
        }

        public void LogicUpdate ()
        {
            UpdateCheckPlayersInRadius ();

            foreach (var updatable in AllLogicUpdatable)
            {
                updatable.LogicUpdate ();
            }
        }

        public void OnCaptureFlag (Flag flag)
        {
            //TODO Add end game logic
        }

        void UpdateCheckPlayersInRadius ()
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