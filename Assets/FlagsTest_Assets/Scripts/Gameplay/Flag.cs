using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlagsTest
{
    public class Flag :TeamObjectWithRenderers
    {
        [SerializeField] Transform _RadiusTransform;

        public event Action<Flag> OnCaptureFlagAction;
        public bool InProtected { get; private set; }
        public float Radius { get; private set; }
        public Player EnemyPlayer { get; private set; }
        public float CaptureFlagPercent { get; private set; }

        HashSet<Player> PlayersInRadius = new HashSet<Player>();
        float CaptureFlagTimer;

        protected override void FixedUpdate ()
        {
            base.FixedUpdate ();

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
            }
            else
            {
                CaptureFlagTimer = WL.SelectedLevel.CaptureFlagDuration;
                CaptureFlagPercent = 0;
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
                EnemyPlayer = null;
            }
            PlayersInRadius.Remove (player);
            CheckPlayers ();
        }

        public void SetRadius (float radius)
        {
            Radius = radius;
            _RadiusTransform.localScale = Vector3.one * radius * 2;
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
                    GameController.Instance.TryStartMiniGameForPlayer (EnemyPlayer);
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
