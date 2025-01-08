using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    public class CarStyleData
    {
        public List<CarDetailsEnum> StyleObjects { get; set; } = new List<CarDetailsEnum>();
        public List<CarDetailsEnum>  PurchasedDetails { get; set; } = new List<CarDetailsEnum>();
        public int ColorId { get; set; }
        public int SmokeOfWheelColoId { get; set; }
    }
}