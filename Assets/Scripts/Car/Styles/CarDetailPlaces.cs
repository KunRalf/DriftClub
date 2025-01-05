using System;
using UnityEngine;

namespace Car
{
    [Serializable]
    public class CarDetailPlaces
    {
        [field: SerializeField] public CarDetailsEnum Type { get; private set; }
        [field: SerializeField] public Transform Place { get; private set; }
        public bool IsEmpty { get; set; } = true;
    }
}