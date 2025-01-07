using System;
using System.Linq;
using Cinemachine;
using Infrastructure.SaveLoad;
using Services;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelInitiator : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Transform _spawnPoint;
        private MainData _mainData;
        private PlayerDataController _playerDataController;
        private CarSaveLoadController _carSaveLoadController;


        [Inject]
        public void Construct(MainData mainData, PlayerDataController playerDataController, CarSaveLoadController carSaveLoadController)
        {
            _mainData = mainData;
            _playerDataController = playerDataController;
            _carSaveLoadController = carSaveLoadController;
           
        }

        private void Start()
        {
            _mainData.CarsDataList.GetCarById(_playerDataController.CurrentCar);
            var carData = _mainData.CarsDataList.GetCarById(_playerDataController.CurrentCar);
            var car = Instantiate(_mainData.CarsDataList.GetCarById(_playerDataController.CurrentCar).CarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            car.Init(carData.CarStyle,carData.CarParams,car.Id);
            car.SetCarStyleParams(_carSaveLoadController.PlayerCarsData.First(_=> _.CarId == carData.Id).Data);
            car.InitToGame();
            _camera.Follow = car.CameraFollowTransform;
            _camera.LookAt = car.CameraFollowTransform;
        }
    }
}