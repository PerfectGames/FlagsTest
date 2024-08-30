using UnityEngine;

namespace FlagsTest
{
    public class GameController_Single :MonoBehaviour
    {
        [SerializeField] Transform _PlaneTransform;
        [SerializeField] int _AiCount = 2;

        int CreatedTeamIndex = 0;

        GameEntity GameEntity;
        MiniGamesManager MiniGamesManager;

        void Start ()
        {
            _PlaneTransform.localScale = new Vector3 (WL.SelectedLevel.LevelSize.x, 1, WL.SelectedLevel.LevelSize.y);

            GameEntity = new GameEntity();
            MiniGamesManager = new MiniGamesManager();

            var userPlayer = CreatePlayer ();
            var playerController = Instantiate (B.ResourcesSettings.PlayerControllerRef);
            playerController.Initialize (userPlayer);

            for (int i = 0; i < _AiCount; i++)
            {
                var aiPlayer = CreatePlayer ();
                var ai = aiPlayer.gameObject.AddComponent<AIControl> ();
                ai.Initialize (aiPlayer);
            }
        }

        public Player CreatePlayer ()
        {
            Team playerTeam = B.GameSettings.GetTeam(CreatedTeamIndex.Repeat (0, B.GameSettings.TeamsCount -1));

            if (CreatedTeamIndex < B.GameSettings.TeamsCount)
            {
                GameEntity.CreateFlagsForTeam (playerTeam);
            }

            var player = GameEntity.CreatePlayer (playerTeam);

            CreatedTeamIndex++;
            return player;
        }

        private void Update ()
        {
            GameEntity.VisualUpdate ();
        }

        void FixedUpdate ()
        {
            GameEntity.LogicUpdate ();
            MiniGamesManager.LogicUpdate ();
        }
    }
}
