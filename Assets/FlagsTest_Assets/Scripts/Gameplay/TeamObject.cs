using System;
using UnityEngine;

namespace FlagsTest
{
    public class TeamObject :MonoBehaviour, ILogicUpdatable, IVisualUpdatable
    {
        Vector3 _Position;
        public bool StopUpdate => false;
        public virtual Vector3 Position { get => _Position; set => _Position = value;  }
        public Team Team { get; private set; }
        public Color TeamColor { get; private set; }
        public event Action<Team, Team> OnChangeTeamAction;

        protected virtual void Awake ()
        {
            GameEntity.Instance.OnCreateTeamObject (this);
        }

        public virtual void SetTeam (Team team)
        {
            var oldTeam = Team;
            Team = team;
            TeamColor = B.GameSettings.GetColorForTeam (team);
            OnChangeTeamAction?.Invoke (oldTeam, team);
        }

        public virtual void LogicUpdate () { }
        public virtual void VisualUpdate () { }
    }
}
