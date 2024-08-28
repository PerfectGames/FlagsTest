using UnityEngine;

namespace FlagsTest
{
    /// <summary> 
    /// To access the heir by a static field "Instance".
    /// </summary>
    public abstract class Singleton<T> :MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] private bool dontDestroyOnLoad;

        private static T _Instance;

        public static T Instance
        {
            get
            {
                return _Instance;
            }
        }

        void Awake ()
        {
            if (_Instance == null)
            {
                _Instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad (gameObject);
                }
                AwakeSingleton ();
            }
            else
            {
                Destroy (gameObject.GetComponent<T> ());
            }
        }
        protected virtual void AwakeSingleton () { }
    }
}
