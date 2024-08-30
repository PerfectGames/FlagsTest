using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlagsTest
{
    public class Flag :TeamObjectWithRenderers
    {
        [SerializeField] Transform _RadiusTransform;

        public event Action<Flag> OnCaptureFlagAction;
        public Player EnemyPlayer { get; private set; }
        public bool InProtected { get; private set; }
        public float Radius { get; private set; }
        public bool InCapture { get; private set; }
        public Color EnemyTeamColor { get; set; }
        public float CaptureFlagPercent { get; set; }
        HashSet<Player> PlayersInRadius = new HashSet<Player>();
        float CaptureFlagTimer;

        public override Vector3 Position 
        { 
            get => base.Position; 
            set
            {
                base.Position = value;
                transform.position = value;
            }
        }

        protected override void Awake ()
        {
            base.Awake ();

            Radius = WL.SelectedLevel.FlagRadius;
            _RadiusTransform.localScale = Vector3.one * Radius * 2;
            OnCaptureFlagAction += GameEntity.Instance.OnCaptureFlag;
        }

        public override void LogicUpdate ()
        {
            base.LogicUpdate ();

            FixedUpdateCaptureLogic ();
        }

        void FixedUpdateCaptureLogic ()
        {
            if (EnemyPlayer && !InProtected)
            {
                if (!EnemyPlayer.InPenalty)
                {
                    CaptureFlagTimer -= Time.fixedDeltaTime;
                }

                if (CaptureFlagTimer <= 0)
                {
                    SetTeam (EnemyPlayer.Team);
                    EnemyPlayer = null;
                    CaptureFlagTimer = WL.SelectedLevel.CaptureFlagDuration;
                    CaptureFlagPercent = 0;
                    OnCaptureFlagAction?.Invoke (this);
                }
                else
                {
                    CaptureFlagPercent = 1 - CaptureFlagTimer / WL.SelectedLevel.CaptureFlagDuration;
                }

                InCapture = true;
            }
            else
            {
                CaptureFlagTimer = WL.SelectedLevel.CaptureFlagDuration;
                CaptureFlagPercent = 0;
                InCapture = false;
            }
        }

        public void AddPlayerInRadius (Player player)
        {
            PlayersInRadius.Add (player);
            CheckPlayers ();
        }

        public void RemovePlayerFromRadius (Player player)
        {
            if (EnemyPlayer == player)
            {
                MiniGamesManager.Instance.TryStopMiniGameForPlayer (EnemyPlayer);
                EnemyPlayer = null;
            }
            PlayersInRadius.Remove (player);
            CheckPlayers ();
        }

        void CheckPlayers ()
        {
            InProtected = false;
            foreach (var p in PlayersInRadius)
            {
                if (p.Team == Team)
                {
                    InProtected = true;
                }
                else if (EnemyPlayer == null)
                {
                    EnemyPlayer = p;
                    EnemyTeamColor = EnemyPlayer.TeamColor;
                    MiniGamesManager.Instance.ServerStartMiniGameWithProbability (EnemyPlayer);
                }
            }
        }

        public override void SetTeam (Team team)
        {
            base.SetTeam (team);

            CheckPlayers ();
        }
    }
}
