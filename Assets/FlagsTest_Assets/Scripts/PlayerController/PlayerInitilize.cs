using UnityEngine;

namespace FlagsTest
{
    public class PlayerInitilize :MonoBehaviour
    {
        public Player Player { get; private set; }
        public bool IsInited => Player != null;

        public virtual void Initialize (Player player)
        {
            Player = player;
        }
    }
}
