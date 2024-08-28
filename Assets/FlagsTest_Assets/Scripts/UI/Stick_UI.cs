using UnityEngine;
using UnityEngine.EventSystems;

namespace FlagsTest
{
    [RequireComponent(typeof(RectTransform))]
    public class Stick_UI :PlayerInitilize, IControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] float _StickRadius = 100;
        [SerializeField] RectTransform _StickTransform;
        
        RectTransform RectTransform;
        Canvas Canvas;

        public Vector2 Move { get; private set; }

        private void Awake ()
        {
            RectTransform = GetComponent<RectTransform>();
            Canvas = GetComponentInParent<Canvas> ();
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            OnMoveStick (eventData.position - (Vector2)RectTransform.position);
        }

        public void OnDrag (PointerEventData eventData)
        {
            OnMoveStick (eventData.position - (Vector2)RectTransform.position);
        }

        public void OnPointerUp (PointerEventData eventData)
        {
            OnMoveStick (Vector2.zero);
        }

        public override void Initialize (Player player)
        {
            base.Initialize (player);

            player.Control = this;
        }

        void OnMoveStick (Vector2 movePixels)
        {
            float maxRadius = _StickRadius * Canvas.scaleFactor;
            movePixels = Vector2.ClampMagnitude(movePixels, maxRadius);
            _StickTransform.position = (Vector2)RectTransform.position + movePixels;
            Move = movePixels / maxRadius;
        }
    }
}
