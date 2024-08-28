using Mirror;
using UnityEngine;

namespace FlagsTest
{
    public class TeamObject :MonoBehaviour
    {
        Vector3 _Position;
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Team Team { get; private set; }
        public Color TeamColor { get; private set; }

        void SetTeam (Team oldTeam, Team newTeam)
        {
            SetTeam (newTeam);
        }

        public virtual void SetTeam (Team team)
        {
            Team = team;
            TeamColor = B.GameSettings.GetColorForTeam (team);
        }

        protected virtual void Update () { }
        protected virtual void FixedUpdate () { }
    }
}
