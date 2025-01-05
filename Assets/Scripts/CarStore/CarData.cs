using System;
using Car;
using UnityEngine;

namespace CarStore
{
    [Serializable]
    public class CarData
    {
        [field:SerializeField] public int Id { get; private set; }
        [field:SerializeField] public CarMainController CarPrefab { get; private set; }
    }
}