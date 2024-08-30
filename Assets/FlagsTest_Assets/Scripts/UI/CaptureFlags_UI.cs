using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlagsTest
{
    public class CaptureFlags_UI :MonoBehaviour
    {
        [SerializeField] Slider _CaptureSliderRef;
        [SerializeField] Vector3 _SliderOffsetPosition = new Vector3 (0, 3, 0);

        Dictionary<Flag, Slider> FlagsSliders = new Dictionary<Flag, Slider>();
        Dictionary<Flag, RectTransform> FlagsSlidersTransforms = new Dictionary<Flag, RectTransform>();
        Camera Camera;

        private void Start ()
        {
            Camera = Camera.main;
            _CaptureSliderRef.gameObject.SetActive (false);
        }

        void Update ()
        {
            foreach (var flag in GameEntity.Instance.GetAllFlags)
            {
                if (!FlagsSliders.TryGetValue (flag, out var slider))
                {
                    slider = Instantiate (_CaptureSliderRef, _CaptureSliderRef.transform.parent);
                    FlagsSliders[flag] = slider;
                    FlagsSlidersTransforms[flag] = slider.GetComponent<RectTransform>();
                }

                slider.gameObject.SetActive (flag.CaptureFlagPercent > 0);

                if (flag.CaptureFlagPercent > 0)
                {
                    slider.value = flag.CaptureFlagPercent;
                    slider.image.color = flag.EnemyTeamColor;

                    var pos = (Vector2)Camera.WorldToViewportPoint (flag.Position + _SliderOffsetPosition);

                    FlagsSlidersTransforms[flag].anchorMin = pos;
                    FlagsSlidersTransforms[flag].anchorMax = pos;
                }
            }
        }
    }
}
