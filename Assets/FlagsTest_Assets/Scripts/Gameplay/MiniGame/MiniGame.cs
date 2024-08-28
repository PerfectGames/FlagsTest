using System;
using UnityEngine;

namespace FlagsTest
{
    public class MiniGame
    {
        public Player Player { get; private set; }
        public MiniGameDescription MiniGameDescription { get; private set; }
        public bool IsCompleted { get; private set; }


        Action<MiniGame, bool> ResultCallBack;
        protected float Timer;

        public virtual void FixedUpdate ()
        {
            Timer += Time.deltaTime;
            if (Timer >= MiniGameDescription.MiniGameDuration)
            {
                OnCompleteMiniGame (false);
            }
        }

        public virtual void Initialize (Player player, MiniGameDescription description, Action<MiniGame, bool> callBack)
        {
            Player = player;
            MiniGameDescription = description;
            ResultCallBack = callBack;
        }

        protected virtual void OnCompleteMiniGame (bool success)
        {
            ResultCallBack?.Invoke(this, success);
            IsCompleted = true;
        }
    }
}
