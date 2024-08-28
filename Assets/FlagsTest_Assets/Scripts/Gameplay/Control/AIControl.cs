using UnityEngine;

namespace FlagsTest
{
    public class AIControl :PlayerInitilize, IControl
    {
        [SerializeField] float _MinToFlagForStop = 2;
        public Vector2 Move { get; private set; }

        Flag TargetFlag;
        float SqrMinToFlagForStop;

        private void FixedUpdate ()
        {
            float minSqrDistance = float.MaxValue;
            Flag nearestFlag = null;

            if (TargetFlag && !TargetFlag.InProtected)
            {
                if ((TargetFlag.Position - Player.Position).sqrMagnitude > SqrMinToFlagForStop)
                {
                    var direction = (TargetFlag.Position - Player.Position).normalized;
                    Move = new Vector2 (direction.x, direction.z);
                }
                else
                {
                    Move = Vector2.zero;
                }
                return;
            }

            foreach (var f in GameController.Instance.GetAllFlags)
            {
                if (f.InProtected || Player.Team == f.Team && !f.EnemyPlayer)
                {
                    continue;
                }

                float sqrDistance = (f.Position - Player.Position).sqrMagnitude;
                if (minSqrDistance > sqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    nearestFlag = f;
                }
            }

            TargetFlag = nearestFlag;
        }

        public override void Initialize (Player player)
        {
            base.Initialize (player);

            player.Control = this;
            SqrMinToFlagForStop = Mathf.Pow (_MinToFlagForStop, 2);
        }
    }
}
