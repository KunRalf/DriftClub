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

        public (int,Color) GetDefaultColor()
        {
            if (Colors.Count == 0)
            {
                throw new ArgumentException("No Colors have been assigned.");
            }

            return (Colors[0].Id, Colors[0].Color);
        }

        public GameObject GetDetail(CarDetailsEnum carDetail)
        {
            var detail = StyleObjects.FirstOrDefault(_ => _.Type == carDetail);
            return detail?.Prefab;
        }
    }
}