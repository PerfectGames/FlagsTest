using UnityEngine;

namespace FlagsTest
{
    public partial class Player :TeamObjectWithRenderers
    {
        public Flag InRadiusFlag { get; set; }
        public bool InPenalty => PenaltyTimer > 0;
        public float PenaltyTimer { get; set; }

        public override void LogicUpdate ()
        {
            base.LogicUpdate ();

            FixedUpdateMiniGameLogic ();
            FixedUpdateMoveLogic ();
        }

        public void ActivatePenalty (float duration)
        {
            PenaltyTimer += duration;
        }

        void FixedUpdateMiniGameLogic ()
        {
            if (InPenalty)
            {
                PenaltyTimer -= Time.fixedDeltaTime;
            }
        }
    }
}
