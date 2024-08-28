using System.Collections.Generic;
using UnityEngine;

namespace FlagsTest
{
    public partial class Player :TeamObjectWithRenderers
    {
        public Flag InRadiusFlag { get; set; }
        public bool InPenalty => PenaltyTimer > 0;
        public IControl Control { get; set; }

        float PenaltyTimer;

        void LateUpdate ()
        {
            UpdateInterpolationLogic ();
        }

        protected override void FixedUpdate ()
        {
            base.FixedUpdate ();

            FixedUpdateMiniGameLogic ();
            FixedUpdateMoveLogic ();
        }

        public void ActivatePenalty (float duration)
        {
            PenaltyTimer = duration;
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
