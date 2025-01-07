using System;
using Car;
using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage.UIPrefabs
{
    public class DetailSelectPrefab : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _detailName;
        [SerializeField] private TextMeshProUGUI _detailCost;
        [SerializeField] private Button _button;
        public CarDetailsEnum Type => _styleObj.Type;
    
        private CarStyleObject _styleObj;

        public void Init(CarStyleObject carStyleObject, bool isBuy, Action<CarStyleObject> callback)
        {
            _styleObj = carStyleObject;
            _detailCost.text =isBuy ? $"was buy" : $"Cost: {carStyleObject.Cost}";
            _detailName.text = Type.ToString();
            _button.AddListener(() => callback(_styleObj));
        }

        public void UpdateState(bool isBuy)
        {
            _detailCost.text =isBuy ? $"was buy" : $"Cost: {_styleObj.Cost}";
        }
    }
}