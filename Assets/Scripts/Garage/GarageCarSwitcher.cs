using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using CarStore;
using Helpers;
using Infrastructure.SaveLoad;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Garage
{
    public class GarageCarSwitcher : MonoBehaviour
    {
        public event Action<int> OnCarSwitch; 
        
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _prevCar;
        [SerializeField] private Button _nextCar;

        private Transform _spawnPoint;
        private List<CarData> _cars = new List<CarData>();
        private int _currentCarIndex;
        private CarSaveLoadController _carSaveLoadController;
        
        public CarData CurrentCar { get; private set; }
        public CarMainController CurrentCarController { get; private set; }
        
        public void Init(List<CarData> cars, Transform spawnPoint, CarSaveLoadController carSaveLoadController)
        {
            _panel.SetActive(true);
            _cars = cars;
            _spawnPoint = spawnPoint;
            _prevCar.AddListener(PrevCar);
            _nextCar.AddListener(NextCar);
            // save data
            _carSaveLoadController = carSaveLoadController;
            _currentCarIndex = 0;
            ShowCar(_currentCarIndex);
        }
        
        public void Close() => _panel.SetActive(false);
        
        private void NextCar()
        {
            _currentCarIndex++;
            if (_currentCarIndex >= _cars.Count)
            {
                _currentCarIndex = 0; 
            }
            ShowCar(_currentCarIndex);
        }
        
        private void PrevCar()
        {
            _currentCarIndex--;
            if (_currentCarIndex < 0)
            {
                _currentCarIndex = _cars.Count - 1; 
            }
            ShowCar(_currentCarIndex);
        }

        private void CheckOnEmptyStyles()
        {
            PlayerCarData curData =
                _carSaveLoadController.PlayerCarsData.FirstOrDefault(_ => _.CarId == CurrentCar.Id);
            if (curData == default)
            {
                curData = new PlayerCarData()
                {
                    CarId = CurrentCar.Id,
                    Data = new CarStyleData()
                };
                curData.Data.ColorId = CurrentCar.CarStyle.GetCarColor(0).Item1;
                _carSaveLoadController.Save(curData);
            }
        }
        
        
        private void ShowCar(int index)
        {
            if (CurrentCarController != null)
            {
                Destroy(CurrentCarController.gameObject);
            }
            var car = _cars[index];
            OnCarSwitch?.Invoke(car.Id);
            CurrentCarController = Instantiate(car.CarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            CurrentCarController.Init(car.CarStyle, car.CarParams, car.Id);
            CurrentCarController.InitToGarage();
            var carData = _carSaveLoadController.PlayerCarsData.FirstOrDefault(_ => _.CarId == car.Id);
            if (carData != default)
            {
                CurrentCarController.SetCarStyleParams(carData.Data);
            }
            CurrentCar = car;
            CheckOnEmptyStyles();
        }

        private void Default()
        {
            _prevCar.RemoveListener(PrevCar);
            _nextCar.RemoveListener(NextCar);
        }
        
      
        private void OnDestroy()
        {
            Default();
        }
    }
}