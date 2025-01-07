using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using CarStore;
using Helpers;
using Infrastructure.SaveLoad;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Garage
{
    public class GarageController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _carPoint;
        private CarsDataListSO _carsDataListSO;
       
        [Header("Controllers")]
        [SerializeField] private GarageCarSwitcher _garageCarSwitcher;
        [SerializeField] private GarageModify _garageModify;
        
        [Header("Buttons")]
        [SerializeField] private Button _modifyButton;
        [SerializeField] private TextMeshProUGUI _modifyButtonText;

        private CarSaveLoadController _carSaveLoadController;
        private PlayerDataController _playerDataController;
        private MessagesService _messagesService;

        [Inject]
        public void Construct(MainData mainData, CarSaveLoadController carSaveLoadController, PlayerDataController playerDataController, MessagesService messagesService)
        {
            _carsDataListSO = mainData.CarsDataList;
            _carSaveLoadController = carSaveLoadController;
            _playerDataController = playerDataController;
            _messagesService = messagesService;
            _garageCarSwitcher.OnCarSwitch += CarSwitch;
            _garageCarSwitcher.Init(_carsDataListSO.GetAllCars(),_carPoint, carSaveLoadController);
           
        }
        
        private void OpenModify()
        {
            _garageModify.Init(_garageCarSwitcher.CurrentCar.CarStyle, _garageCarSwitcher.CurrentCarController,_carSaveLoadController, _playerDataController, _messagesService);
            _garageCarSwitcher.Close();
        }

        private void CarSwitch(int carId)
        {
            ResetModifyButton();
            if (_playerDataController.OpenedCars.Contains(carId))
            {
                _modifyButtonText.text = $"Modify";
                _modifyButton.AddListener(OpenModify);
            }
            else
            {
                _modifyButtonText.text = $"Cost: {_carsDataListSO.GetCarById(carId).Cost}";
                _modifyButton.AddListener(() => OpenCarBuyWindow(carId));
            }
        }

        private void OpenCarBuyWindow(int carId)
        {
            _messagesService.InitMessage("Buy car", $"Cost: {_carsDataListSO.GetCarById(carId).Cost}",() => BuyCar(carId));
        }

        private void BuyCar(int carId)
        {
            var carCost = _carsDataListSO.GetCarById(carId).Cost;
            if (_playerDataController.Cash < carCost)
            {
                _messagesService.InitMessage(string.Empty, "No money to buy");
            }
            else
            {
                _playerDataController.UpdateCash(_playerDataController.Cash - carCost);
                _playerDataController.AddCar(carId);
                CarSwitch(carId);
                _messagesService.InitMessage(string.Empty, "U but a car");
            }
        }
        
        
        private void ResetModifyButton()
        {
            _modifyButton.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            _garageCarSwitcher.OnCarSwitch -= CarSwitch;
        }
    }
}