using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarStore
{
    [CreateAssetMenu(fileName = "CarsList", menuName = "Data/CarsList", order = 0)]
    public class CarsDataListSO : ScriptableObject, ICarDataProvider
    {
        [SerializeField] private List<CarData> _cars;

        public CarData GetCarById(int id)
        {
            var car = _cars.FirstOrDefault(_ => _.Id == id);
            if(car == default)
                throw new ArgumentException("Car not found");
            return car;
        }
        

        public List<CarData> GetAllCars() => _cars.ToList();
    }
}