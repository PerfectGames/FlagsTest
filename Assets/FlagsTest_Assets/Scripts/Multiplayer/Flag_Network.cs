using Mirror;
using UnityEngine;

namespace FlagsTest
{
    public class Flag_Network :TeamObject_Network
    {
        Flag Flag;

        [SyncVar]
        float CaptureFlagPercent;

        [SyncVar]
        Color CaptureTeamColor;

        public Vector2 Move { get; private set; }

        protected override void Awake ()
        {
            base.Awake ();
            Flag = TeamObject as Flag;
        }

        void FixedUpdate ()
        {
            if (isServer)
            {
                CaptureFlagPercent = Flag.CaptureFlagPercent;
                CaptureTeamColor = Flag.EnemyTeamColor;
            }
            else
            {
                Flag.EnemyTeamColor = CaptureTeamColor;
                Flag.CaptureFlagPercent = CaptureFlagPercent;
            }
        }
    }
}
