using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Car
{
    [CreateAssetMenu(fileName = "CarStyle", menuName = "CarSettings/CarStyle", order = 0)]
    public class CarStyleSO : ScriptableObject
    {
        [field: SerializeField] public List<CarColor> Colors { get; private set; }
        [field: SerializeField] public List<CarStyleObject> StyleObjects { get; private set; }

        public (CarDetailsEnum, CarStyleObjectPrefab) GetDetail(CarDetailsEnum carDetail)
        {
            if (StyleObjects.Count == 0)
            {
                throw new ArgumentException("No Details have been assigned.");
            }
            var detail = StyleObjects.FirstOrDefault(_ => _.Type == carDetail);
            return (detail?.Type ?? CarDetailsEnum.None, detail?.Prefab);
        }

        public (int, Color) GetCarColor(int id)
        {
            if (Colors.Count == 0)
            {
                throw new ArgumentException("No Colors have been assigned.");
            }
            var color = Colors.FirstOrDefault(_ => _.Id == id);
            if (color == default) return (0,Colors[0].Color);
            return (color.Id,color.Color);
        }
    }
}