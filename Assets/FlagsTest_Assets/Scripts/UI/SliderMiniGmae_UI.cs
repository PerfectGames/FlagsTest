using UnityEngine;
using UnityEngine.EventSystems;

namespace FlagsTest
{
    public class SliderMiniGmae_UI :PlayerInitilize, IPointerClickHandler
    {
        [SerializeField] RectTransform SuccesZoneTR;
        [SerializeField] RectTransform SliderTR;

        Slider_MiniGame Slider_MiniGame;

        private void Update ()
        {
            if (Slider_MiniGame != null)
            {
                float ancorX = Slider_MiniGame.GetSliderValue ();

                Vector2 ancorMin = SliderTR.anchorMin;
                Vector2 ancorMax = SliderTR.anchorMax;

                ancorMin.x = ancorX;
                ancorMax.x = ancorX;

                SliderTR.anchorMin = ancorMin;
                SliderTR.anchorMax = ancorMax;
            }
        }

        public override void Initialize (Player player)
        {
            base.Initialize (player);

            GameController.Instance.OnStartMiniGame += OnStartMiniGmae;
            GameController.Instance.OnEndMiniGame += OnEndMiniGmae;

            gameObject.SetActive (false);
        }

        void OnStartMiniGmae (MiniGame miniGame)
        {
            if (Player == miniGame.Player && miniGame is Slider_MiniGame)
            {
                Slider_MiniGame = miniGame as Slider_MiniGame;
                gameObject.SetActive (true);

                float anchorMinX = Slider_MiniGame.SuccessZoneStart;
                float anchorMaxX = anchorMinX + Slider_MiniGame.SliderMiniGameDescription.SuccesZone;

                Vector2 ancorMin = SuccesZoneTR.anchorMin;
                Vector2 ancorMax = SuccesZoneTR.anchorMax;

                ancorMin.x = anchorMinX;
                ancorMax.x = anchorMaxX;

                SuccesZoneTR.anchorMin = ancorMin;
                SuccesZoneTR.anchorMax = ancorMax;
            }
        }

        void OnEndMiniGmae (MiniGame miniGame, bool succes)
        {
            if (Player == miniGame.Player)
            {
                gameObject.SetActive (false);
            }
        }

        public void OnPointerClick (PointerEventData eventData)
        {
            Slider_MiniGame.OnPress ();
        }
    }
}
