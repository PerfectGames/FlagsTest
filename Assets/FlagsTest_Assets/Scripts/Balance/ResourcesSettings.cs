using UnityEngine;

namespace FlagsTest
{
    /// <summary>
    /// Links to resources.
    /// </summary>

    [CreateAssetMenu (fileName = "ResourcesSettings", menuName = "GameBalance/Settings/ResourcesSettings")]
    public class ResourcesSettings :ScriptableObject
    {
        [field: SerializeField] public Flag FlagRef { get; private set; }
        [field: SerializeField] public Player PlayerRef { get; private set; }
        [field: SerializeField] public PlayerController PlayerControllerRef { get; private set; }
    }
}
