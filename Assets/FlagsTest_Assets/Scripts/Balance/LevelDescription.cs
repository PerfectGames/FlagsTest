using UnityEngine;

namespace FlagsTest
{
    [CreateAssetMenu (fileName = "Level", menuName = "GameBalance/LevelDescription")]
    public class LevelDescription :ScriptableObject
    {
        [field: SerializeField, Range (0, 100)] public int FlagsForTeamsCount { get; private set; } = 2;
        [field: SerializeField, Range (0f, 100f)] public float FlagRadius { get; private set; } = 10f;
        [field: SerializeField, Range (0f, 100f)] public float CaptureFlagDuration { get; private set; } = 10f;
        [field: SerializeField] public Vector2 LevelSize { get; private set; } = new Vector2 (50, 50);

        [field: SerializeField] public MiniGameDescription MiniGame { get; private set; }
    }
}
