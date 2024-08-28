using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

namespace FlagsTest
{
    public class FlagsTest_NetworkManager :NetworkManager
    {
        [SerializeField] Button _ConnectToServerButton;
        [SerializeField] Button _CreateHostButton;
        [SerializeField] TMP_InputField _ServerAddress;

        public override void Awake ()
        {
            base.Awake ();

            _ConnectToServerButton.onClick.AddListener (() => { networkAddress = _ServerAddress.text; StartClient (); });
            _CreateHostButton.onClick.AddListener (StartHost);
        }

        public override void OnStartServer ()
        {
            //NetworkServer.RegisterHandler<CreatePlayerMessage> (OnCreatePlayer);
        }
    }

    public struct CreatePlayerMessage :NetworkMessage
    {
        public string PlayerName;
        public bool IsAI;
    }
}
