using System;
using TMPro;
using UnityEngine;

namespace Car.UI
{
    public class CarHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _driftPoints;
        [SerializeField] private TextMeshProUGUI _totalPoints;
   
        private float _roundPoints;

        public void Init()
        {
            _roundPoints = 0;
            UpdateRoundPoints();
        }
        
        public void ShowDriftPoints()
        {
            _driftPoints.gameObject.SetActive(true);
        }

        public void HideDriftPoints(float points)
        {
            _roundPoints += points;
            _driftPoints.gameObject.SetActive(false);
            UpdateRoundPoints();
        }
        
        public void ProgressPoints(float progress)
        {
            _driftPoints.text = progress.ToString("0");
        }

        private void UpdateRoundPoints()
        {
            _totalPoints.text = "Round points: " + _roundPoints.ToString("0");
        }
    }
}