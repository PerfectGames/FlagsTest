using System;
using Random = UnityEngine.Random;

namespace FlagsTest
{
    public class Slider_MiniGame :MiniGame
    {
        public SliderMiniGameDescription SliderMiniGameDescription { get; private set; }

        public float SuccessZoneStart {  get; private set; }

        public override void Initialize (Player player, MiniGameDescription description, Action<MiniGame, bool> callBack)
        {
            base.Initialize (player, description, callBack);
            SliderMiniGameDescription = description as SliderMiniGameDescription;
            SuccessZoneStart = Random.Range (0f, 1f - SliderMiniGameDescription.SuccesZone);
        }

        public override void CheckResult (float endTime)
        {
            float sliderValue =  GetSliderValue (endTime);
            OnCompleteMiniGame (sliderValue > SuccessZoneStart && sliderValue < SuccessZoneStart + SliderMiniGameDescription.SuccesZone);
        }

        public float GetSliderValue ()
        {
            return GetSliderValue (Timer);
        }

        public float GetSliderValue (float time)
        {
            int direction = (int)(time * SliderMiniGameDescription.SliderLoopsPerSecond) + 1;
            direction = direction % 2 == 0 ? -1 : 1;

            float sliderPosition = (time * SliderMiniGameDescription.SliderLoopsPerSecond) % 1;
            if (direction == -1)
            {
                sliderPosition = 1 - sliderPosition;
            }

            return sliderPosition;
        }
    }
}
