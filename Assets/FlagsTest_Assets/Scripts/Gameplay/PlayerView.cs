using UnityEngine;

namespace FlagsTest
{
    public partial class Player :TeamObjectWithRenderers
    {
        [SerializeField] Transform _ViewTransform;
        [SerializeField] float _MoveSpeed = 10f;
        [SerializeField] float _PlayerRadius = 0.5f;

        public Transform ViewTransform => _ViewTransform;

        Vector2 _InterpolationOffset;
        Vector2 InterpolationOffset
        {
            get { return _InterpolationOffset; }
            set
            {
                _InterpolationOffset = value;
                ViewTransform.localPosition = ViewTransform.InverseTransformDirection (new Vector3 (value.x, 0, value.y));
            }
        }

        void UpdateInterpolationLogic ()
        {
            if (Control != null && Control.Move.sqrMagnitude > 0)
            {
                InterpolationOffset += Control.Move * _MoveSpeed * Time.deltaTime;
            }
        }

        void FixedUpdateMoveLogic ()
        {
            if (Control != null && Control.Move.sqrMagnitude > 0)
            {
                Vector3 deltaPos = new Vector3 (Control.Move.x, 0, Control.Move.y) * _MoveSpeed * Time.fixedDeltaTime;
                transform.rotation = Quaternion.AngleAxis (-Vector3.SignedAngle (deltaPos, Vector3.forward, Vector3.up), Vector3.up);
                transform.position = GameController.Instance.CheckZoneCollision (transform.position + deltaPos, _PlayerRadius);
                InterpolationOffset = Vector2.zero;
            }
        }
    }
}
