using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void OnPress ()
        {
            float sliderValue =  GetSliderValue ();
            OnCompleteMiniGame (sliderValue > SuccessZoneStart && sliderValue < SuccessZoneStart + SliderMiniGameDescription.SuccesZone);
        }

        public float GetSliderValue ()
        {
            int direction = (int)(Timer * SliderMiniGameDescription.SliderLoopsPerSecond) + 1;
            direction = direction % 2 == 0 ? -1 : 1;

            float sliderPosition = (Timer * SliderMiniGameDescription.SliderLoopsPerSecond) % 1;
            if (direction == -1)
            {
                sliderPosition = 1 - sliderPosition;
            }

            return sliderPosition;
        }
    }
}
