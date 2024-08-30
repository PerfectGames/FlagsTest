using UnityEngine;

namespace FlagsTest
{
    public partial class Player :TeamObjectWithRenderers
    {
        [SerializeField] Transform _ViewTransform;
        [SerializeField] float _MoveSpeed = 10f;
        [SerializeField] float _InterpolationViewSpeed = 10f;
        [SerializeField] float _PlayerRadius = 0.5f;
        [SerializeField] float _SqrMagnitudeForImmediatelyMove = 25;

        public IControl Control { get; set; }
        public float GetMoveSpeed => _MoveSpeed;

        bool HasMoveControl => Control != null && Control.Move.sqrMagnitude > 0;

        public override void VisualUpdate ()
        {
            base.VisualUpdate ();

            float sqrDistance = (Position - transform.position).sqrMagnitude;
            if (sqrDistance > _SqrMagnitudeForImmediatelyMove)
            {

                transform.position = Position;
            }
            else if (sqrDistance > 0)
            {
                transform.position = Vector3.Lerp (transform.position, Position, _InterpolationViewSpeed * Time.deltaTime);
            }

            if (HasMoveControl)
            {
                transform.rotation = Quaternion.AngleAxis (-Vector3.SignedAngle (Control.Move.ToVector3(), Vector3.forward, Vector3.up), Vector3.up);
            }
        }

        void FixedUpdateMoveLogic ()
        {
            if (HasMoveControl)
            {
                Vector3 deltaPos = Control.Move.ToVector3() * _MoveSpeed * Time.deltaTime;
                Position = GameEntity.Instance.CheckZoneCollision (Position + deltaPos, _PlayerRadius);
            }
        }
    }
}
