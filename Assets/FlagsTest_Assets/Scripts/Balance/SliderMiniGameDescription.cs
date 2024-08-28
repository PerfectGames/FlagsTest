using UnityEngine;

namespace FlagsTest
{
    [CreateAssetMenu (fileName = "Slider_MiniGame", menuName = "GameBalance/MiniGames/SliderMiniGameDescription")]
    public class SliderMiniGameDescription :MiniGameDescription
    {
        [field: SerializeField, Range (0f, 100f)] public float SliderLoopsPerSecond { get; private set; } = 0.5f;
        [field: SerializeField, Range (0f, 1f)] public float SuccesZone { get; private set; } = 0.25f;
    }
}
