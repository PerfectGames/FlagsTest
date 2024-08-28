using UnityEngine;

namespace FlagsTest
{
    public class MiniGameDescription :ScriptableObject
    {
        [field: SerializeField, Range(0f, 1f)] public float MiniGameProbability { get; private set; } = 0.5f;
        [field: SerializeField, Range (0f, 100f)] public float MiniGameDuration { get; private set; } = 1.5f;
        [field: SerializeField, Range (0f, 100f)] public float PenaltyDuration { get; private set; } = 3f;
    }
}
