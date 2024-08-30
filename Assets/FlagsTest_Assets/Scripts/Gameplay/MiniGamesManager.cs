using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlagsTest
{
    public class MiniGamesManager
    {
        public static MiniGamesManager Instance { get; private set; }

        List<MiniGame> MiniGames = new List<MiniGame>();

        public event Action<MiniGame, int> OnStartMiniGame;
        public event Action<MiniGame, bool> OnEndMiniGame;

        LevelDescription Level = WL.SelectedLevel;

        public MiniGamesManager ()
        {
            Instance = this;
        }

        public void ClientStartMiniGame (Player player, int seed)
        {
            Random.InitState(seed);

            MiniGame miniGame = null;

            foreach (var playerMinigames in MiniGames.Where (m => m.Player == player))
            {
                playerMinigames.CompleteWithoutCallBack ();
            }

            if (Level.MiniGame is SliderMiniGameDescription)
            {
                miniGame = new Slider_MiniGame ();
            }

            if (miniGame != null)
            {
                miniGame.Initialize (player, Level.MiniGame, OnClientCompleteMiniGame);
                MiniGames.Add (miniGame);
                OnStartMiniGame?.Invoke (miniGame, seed);
            }
        }

        public void ServerStartMiniGameWithProbability (Player player)
        {
            float probability = Random.Range (0f, 1f);
            if (probability < Level.MiniGame.MiniGameProbability)
            {
                int seed = Mathf.RoundToInt (Time.time);
                Random.InitState (seed);
                MiniGame miniGame = null;

                if (Level.MiniGame is SliderMiniGameDescription)
                {
                    miniGame = new Slider_MiniGame ();
                }

                if (miniGame != null)
                {
                    miniGame.Initialize (player, Level.MiniGame, OnServerCompleteMiniGame);
                    MiniGames.Add (miniGame);
                    OnStartMiniGame?.Invoke (miniGame, seed);
                }
            }
        }

        public void TryStopMiniGameForPlayer (Player player, float time = -1)
        {
            MiniGame miniGameForPlayer = null;
            foreach (var miniGame in MiniGames)
            {
                if (miniGame.Player == player)
                {
                    miniGameForPlayer = miniGame;
                    break;
                }
            }

            if (miniGameForPlayer != null)
            {
                if (time > 0)
                {
                    miniGameForPlayer.CheckResult (time);
                }
                else
                {
                    miniGameForPlayer.FailMinigame ();
                }
            }
        }

        void OnClientCompleteMiniGame (MiniGame miniGame, bool success)
        {
            OnEndMiniGame?.Invoke (miniGame, success);
        }

        void OnServerCompleteMiniGame (MiniGame miniGame, bool success)
        {
            if (!success && miniGame.Player)
            {
                miniGame.Player.ActivatePenalty (miniGame.MiniGameDescription.PenaltyDuration);
            }

            OnEndMiniGame?.Invoke (miniGame, success);
        }

        public void LogicUpdate ()
        {
            int index = MiniGames.Count - 1;

            while (index >= 0)
            {
                if (MiniGames[index].IsCompleted)
                {
                    MiniGames.Remove (MiniGames[index]);
                }
                else
                {
                    MiniGames[index].LogicUpdate ();
                }
                index--;
            }
        }
    }
}
