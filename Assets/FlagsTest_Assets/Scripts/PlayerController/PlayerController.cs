using UnityEngine;

namespace FlagsTest
{
    public class PlayerController :PlayerInitilize
    {
        [SerializeField] PlayerInitilize[] _PlayerInitilizeObjects;
        
        public static PlayerController Instance;

        public static PlayerController GetOrCreate ()
        {
            if (Instance == null)
                Instance = Instantiate (B.ResourcesSettings.PlayerControllerRef);

            return Instance;
        }

        private void Awake ()
        {
            
        }

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
