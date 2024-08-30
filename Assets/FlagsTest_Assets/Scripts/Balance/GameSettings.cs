using System.Collections.Generic;
using UnityEngine;

namespace FlagsTest
{
    [CreateAssetMenu (fileName = "GameSettings", menuName = "GameBalance/Settings/GameSettings")]
    public class GameSettings :ScriptableObject
    {
        [field: SerializeField] public LevelDescription MainLevel { get; private set; }

        [SerializeField] TeamColorKV[] _Teams;

        public Color GetColorForTeam (Team team)
        {
            foreach (var teamColorKV in _Teams)
            {
                if (team == teamColorKV.Team)
                {
                    return teamColorKV.Color;
                }
            }

            Debug.LogError ($"Color for [{team}] not found");
            return Color.white;
        }

        public int TeamsCount => _Teams.Length;

        public Team GetTeam (int index)
        {
            return _Teams[index].Team;
        }

        public IEnumerable<Team> Teams
        {
            get
            {
                foreach (var team in _Teams)
                {
                    yield return team.Team;
                }
            }
        }

        [System.Serializable]
        struct TeamColorKV
        {
            public Team Team;
            public Color Color;
        }
    }

    public enum Team
    {
        Red,
        Yellow,
        Blue
    }
}
