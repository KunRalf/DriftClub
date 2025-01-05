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
        [SerializeField] private Button _button;
        private CarDetailsEnum _type;
        
        public void Init(CarStyleObject carStyleObject, Action<CarDetailsEnum> callback)
        {
            _type = carStyleObject.Type;
            _detailName.text = _type.ToString();
            _button.AddListener(() => callback(_type));
        }
    }
}