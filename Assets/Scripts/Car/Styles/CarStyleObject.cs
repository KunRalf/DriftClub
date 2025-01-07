using System;
using UnityEngine;

namespace Car
{
    [Serializable]
    public class CarStyleObject
    {
        [field:SerializeField] public CarDetailsEnum Type { get; private set; }
        [field:SerializeField] public CarStyleObjectPrefab Prefab { get; private set; }
        [field:SerializeField] public float Cost { get; private set; }
    }
}