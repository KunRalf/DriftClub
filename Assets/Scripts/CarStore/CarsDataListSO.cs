using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarStore
{
    [CreateAssetMenu(fileName = "CarsList", menuName = "Data/CarsList", order = 0)]
    public class CarsDataListSO : ScriptableObject
    {
        [SerializeField] private List<CarData> _cars;

        public CarData GetCarById(int id)
        {
            var car = _cars.FirstOrDefault(_ => _.Id == id);
            if(car == default)
                throw new ArgumentException("Car not found");
            return car;
        }

        public List<int> GetAvailableIds()
        {
            return _cars.Select(_ => _.Id).ToList();
        }
        
    }
}