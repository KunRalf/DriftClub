using System;
using UnityEngine;

namespace Car
{
    [Serializable]
    public class CarStyleObject
    {
        [field:SerializeField] public CarDetailsEnum Type { get; private set; }
        [field:SerializeField] public GameObject Prefab { get; private set; }
    }
}