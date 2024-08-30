using Mirror;
using UnityEngine;

namespace FlagsTest
{
    public class Player_Network :TeamObject_Network, IControl
    {
        Player Player;

        float PenaltyTimer;

        public Vector2 Move { get; private set; }

        protected override void Awake ()
        {
            base.Awake ();
            Player = TeamObject as Player;
        }

        public override void OnStartServer ()
        {
            base.OnStartServer ();

            if (!isOwned)
            {
                Player.Control = this;
            }
        }

        public override void OnStartClient ()
        {
            base.OnStartClient ();

            if (!isOwned)
            {
                Player.Control = this;
            }
        }

        public override void OnStartLocalPlayer ()
        {
            base.OnStartLocalPlayer ();

            PlayerController.GetOrCreate ().Initialize (Player);
        }

        void FixedUpdate ()
        {
            if (isServer)
            {
                Position = TeamObject.Position;
                if (Player.PenaltyTimer > PenaltyTimer)
                {
                    PenaltyTimer = Player.PenaltyTimer;
                    SendPenaltyRpc (PenaltyTimer);
                }
            }
            else if (PenaltyTimer > 0)
            {
                PenaltyTimer -= Time.fixedDeltaTime;
                Player.PenaltyTimer = PenaltyTimer;
            }

            if (isOwned && Player.Control != null && Move != Player.Control.Move)
            {
                Move = Player.Control.Move;

                if (isServer)
                {
                    SendMoveRpc (Move);
                }
                else
                {
                    SendMoveCommand (Move);
                }
            }
        }

        [Command (channel = Channels.Reliable)]
        void SendMoveCommand (Vector2 move)
        {
            Move = Vector2.ClampMagnitude(move, 1);
            SendMoveRpc (Move);
        }

        [ClientRpc (channel = Channels.Unreliable, includeOwner = false)]
        void SendMoveRpc (Vector2 move)
        {
            Move = move;
        }

        [ClientRpc (channel = Channels.Reliable)]
        void SendPenaltyRpc (float penalty)
        {
            PenaltyTimer = penalty - (float) NetworkTime.rtt;
        }

        protected override void OnChangePosition (Vector3 oldPosition, Vector3 newPosition)
        {
            if (!isServer)
            {
                TeamObject.Position = newPosition + Move.ToVector3() * Player.GetMoveSpeed * (float)NetworkTime.rtt;
            }
        }
    }
}
