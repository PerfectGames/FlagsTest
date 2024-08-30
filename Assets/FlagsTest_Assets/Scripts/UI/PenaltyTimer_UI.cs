using TMPro;
using UnityEngine;

namespace FlagsTest
{
    public class PenaltyTimer_UI :PlayerInitilize
    {
        [SerializeField] TextMeshProUGUI _PenaltyText;

        private void Update ()
        {
            if (IsInited)
            {
                _PenaltyText.gameObject.SetActive (Player.InPenalty);
                if (Player.InPenalty)
                {
                    _PenaltyText.text = Player.PenaltyTimer.ToString ("#0.##");
                }
            }
        }
    }
}
