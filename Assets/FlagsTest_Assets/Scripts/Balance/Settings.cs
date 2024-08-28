using UnityEngine;

namespace FlagsTest
{
    [CreateAssetMenu (fileName = "Settings", menuName = "GameBalance/Settings/Settings")]

    public class Settings :ScriptableObject
    {
        public GameSettings GameSettings;
        public ResourcesSettings ResourcesSettings;
    }
}
