using TMPro;
using UnityEngine;

namespace PlayerHub
{
    public class CashUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _myCashText;

        public void UpdateCash(float cash)
        {
            _myCashText.text = "My cash: " + cash.ToString("0");
        }
    }
}