using System;
using Car;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Garage.UIPrefabs
{
    public class ColorSelectPrefab : MonoBehaviour
    {
        [SerializeField] private Image _colorImage;
        [SerializeField] private Button _colorButton;
        private int _id;
        
        
        public void Init(CarColor carColor, Action<int> color)
        {
            _id = carColor.Id;
            _colorImage.color = carColor.Color;
            _colorButton.AddListener(() => color(_id));
            // событие на запоминание
        }
    }
}