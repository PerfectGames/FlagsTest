using System;
using UnityEngine;

namespace FlagsTest
{
    public class MiniGame: ILogicUpdatable
    {
        public Player Player { get; private set; }
        public Flag Flag { get; private set; }
        public MiniGameDescription MiniGameDescription { get; private set; }
        public bool IsCompleted { get; private set; }

        Action<MiniGame, bool> ResultCallBack;
        public float Timer {  get; private set; }

        public virtual void LogicUpdate ()
        {
            if (IsCompleted)
            {
                return;
            }

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

        public virtual void CheckResult (float endTime) { }

        protected virtual void OnCompleteMiniGame (bool success)
        {
            if (!IsCompleted)
            {
                ResultCallBack?.Invoke (this, success);
                IsCompleted = true;
            }
        }

        public void FailMinigame ()
        {
            if (!IsCompleted)
                OnCompleteMiniGame (false);
        }

        public void CompleteWithoutCallBack ()
        {
            ResultCallBack = null;
            IsCompleted = true;
        }
    }
}
