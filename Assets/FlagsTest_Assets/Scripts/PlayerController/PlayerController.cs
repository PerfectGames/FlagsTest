using UnityEngine;

namespace FlagsTest
{
    public class PlayerController :PlayerInitilize
    {
        [SerializeField] PlayerInitilize[] _PlayerInitilizeObjects;

        public override void Initialize (Player player)
        {
            base.Initialize (player);

            foreach (var p in _PlayerInitilizeObjects)
            {
                p.Initialize (player);
            }
        }
    }
}
