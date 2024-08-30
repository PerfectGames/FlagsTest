using UnityEngine;
using Mirror;

namespace FlagsTest
{
    [RequireComponent(typeof (TeamObject))]
    public class TeamObject_Network :NetworkBehaviour
    {
        protected TeamObject TeamObject;

        [SyncVar (hook = nameof (OnChangePosition))]
        protected Vector3 Position;

        [SyncVar (hook = nameof (OnChageTeam))]
        Team Team;

        protected virtual void Awake ()
        {
            TeamObject = GetComponent<TeamObject> ();

            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                enabled = false;
            }
        }

        public override void OnStartServer ()
        {
            base.OnStartServer ();

            TeamObject.OnChangeTeamAction += OnChageTeam;
            Team = TeamObject.Team;
            Position = TeamObject.Position;
        }

        public override void OnStartClient ()
        {
            base.OnStartClient ();

            TeamObject.Position = Position;
            TeamObject.SetTeam (Team);
        }

        protected virtual void OnChangePosition (Vector3 oldPosition, Vector3 newPosition)
        {
            if (!isServer)
                TeamObject.Position = newPosition;
        }

        void OnChageTeam (Team oldTeam, Team newTeam)
        {
            if (isServer)
            {
                Team = newTeam;
            }
            else
            {
                TeamObject.SetTeam (newTeam);
            }
        }
    }
}
