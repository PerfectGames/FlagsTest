using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

namespace FlagsTest
{
    public class GameController_Network :NetworkManager
    {
        [SerializeField] GameObject MenuConnectionObject;
        [SerializeField] Button _ConnectToServerButton;
        [SerializeField] Button _CreateServerButton;
        [SerializeField] Button _CreateHostButton;
        [SerializeField] TMP_InputField _ServerAddress;

        [SerializeField] Transform _PlaneTransform;

        int CreatedTeamIndex = 0;
        Dictionary<NetworkConnectionToClient, Player> ClientPlayers = new Dictionary<NetworkConnectionToClient, Player> ();

        GameEntity GameEntity;
        MiniGamesManager MiniGamesManager;

        public override void Awake ()
        {
            base.Awake ();
            Application.targetFrameRate = 60;
            _ConnectToServerButton.onClick.AddListener (() => { networkAddress = _ServerAddress.text; StartClient (); });
            _CreateHostButton.onClick.AddListener (StartHost);
            _CreateServerButton.onClick.AddListener (StartServer);
        }

        public override void OnStartServer ()
        {
            NetworkServer.RegisterHandler<CreatePlayerMessage> (OnCreatePlayer);
            NetworkServer.RegisterHandler<MinigameResultMessage> (OnReciveMiniGameResult);

            _PlaneTransform.localScale = new Vector3 (WL.SelectedLevel.LevelSize.x, 1, WL.SelectedLevel.LevelSize.y);
            GameEntity = new GameEntity ();
            MiniGamesManager = new MiniGamesManager ();
            MiniGamesManager.OnStartMiniGame += OnStartMiniGame;
            MenuConnectionObject.SetActive (false);
        }

        public override void OnStartClient ()
        {
            NetworkClient.RegisterHandler<StartMinigameMessage> (OnReciveStartMiniGame);

            base.OnStartClient ();
            
            _PlaneTransform.localScale = new Vector3 (WL.SelectedLevel.LevelSize.x, 1, WL.SelectedLevel.LevelSize.y);
            if (GameEntity.Instance == null)
            {
                GameEntity = new GameEntity ();
                MiniGamesManager = new MiniGamesManager ();
                MiniGamesManager.OnEndMiniGame += OnEndMiniGame;
            }
            MenuConnectionObject.SetActive (false);
        }

        public override void OnClientConnect ()
        {
            base.OnClientConnect ();

            NetworkClient.Send (new CreatePlayerMessage ());
        }

        void OnCreatePlayer (NetworkConnectionToClient conn, CreatePlayerMessage message) 
        {
            Team playerTeam = B.GameSettings.GetTeam(CreatedTeamIndex.Repeat (0, B.GameSettings.TeamsCount -1));
            
            if (CreatedTeamIndex < B.GameSettings.TeamsCount)
            {
                var flags = GameEntity.CreateFlagsForTeam (playerTeam);
                foreach (var flag in flags)
                {
                    NetworkServer.Spawn (flag.gameObject);
                }
            }
            
            var player = GameEntity.CreatePlayer (playerTeam);
            NetworkServer.AddPlayerForConnection (conn, player.gameObject);
            ClientPlayers.Add (conn, player);

            CreatedTeamIndex++;
        }

        void OnStartMiniGame (MiniGame miniGame, int seed)
        {
            foreach (var clientPlayerKV in ClientPlayers)
            {
                if (clientPlayerKV.Value == miniGame.Player)
                {
                    if (!clientPlayerKV.Key.identity.isLocalPlayer)
                    {
                        clientPlayerKV.Key.Send (new StartMinigameMessage (seed));
                    }
                    break;
                }
            }
        }

        void OnEndMiniGame (MiniGame miniGame, bool success)
        {
            NetworkClient.Send (new MinigameResultMessage (miniGame.Timer));
        }

        //Client
        void OnReciveStartMiniGame (StartMinigameMessage message)
        {
            var player = NetworkClient.localPlayer.GetComponent<Player>();
            if (player)
            {
                MiniGamesManager.ClientStartMiniGame (player, message.Seed);
            }
        }

        //Server
        void OnReciveMiniGameResult (NetworkConnectionToClient conn, MinigameResultMessage message)
        {
            if (ClientPlayers.TryGetValue (conn, out var player) && player != null)
            {
                MiniGamesManager.TryStopMiniGameForPlayer (player, message.Time);
            }
        }

        public override void Update ()
        {
            base.Update ();

            if (NetworkClient.active)
            {
                GameEntity.VisualUpdate ();
            }
        }

        void FixedUpdate ()
        {
            if (NetworkServer.active)
            {
                GameEntity.LogicUpdate ();
            }

            if (MiniGamesManager != null)
            {
                MiniGamesManager.LogicUpdate ();
            }
        }
    }

    public struct CreatePlayerMessage :NetworkMessage
    {
    }

    public struct StartMinigameMessage :NetworkMessage
    {
        public int Seed;

        public StartMinigameMessage (int seed)
        {
            Seed = seed;
        }
    }

    public struct MinigameResultMessage :NetworkMessage
    {
        public float Time;

        public MinigameResultMessage (float time)
        {
            Time = time;
        }
    }
}
