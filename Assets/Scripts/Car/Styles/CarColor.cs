using System;
using UnityEngine;

namespace Car
{
    [Serializable]
    public class CarColor
    {
        [field:SerializeField] public int Id { get; private set; } 
        [field:SerializeField] public Color Color { get; private set; } 
    }
}