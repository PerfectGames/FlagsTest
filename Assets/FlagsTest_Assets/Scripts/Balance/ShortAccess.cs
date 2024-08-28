using UnityEngine;

namespace FlagsTest
{
    /// <summary> 
    /// Fast access to settings.
    /// B - Balance.
    /// </summary>
    public static class B
    {
        static Settings _Settings;

        static Settings Settings
        {
            get
            {
                if (_Settings == null)
                {
                    _Settings = Resources.Load<Settings> ("Settings");
                }
                return _Settings;
            }
        }

        public static GameSettings GameSettings { get { return Settings.GameSettings; } }
        public static ResourcesSettings ResourcesSettings { get { return Settings.ResourcesSettings; } }
    }

    /// <summary> 
    /// World loading data
    /// WL - World loading
    /// </summary>
    public static class WL
    {
        public static LevelDescription SelectedLevel => B.GameSettings.MainLevel;
    }

    /// <summary> 
    /// Constants used in game
    /// C - Constants
    /// </summary>
    public static class C
    {

    }
}
