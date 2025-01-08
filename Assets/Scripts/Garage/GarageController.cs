using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using CarStore;
using Helpers;
using Infrastructure.SaveLoad;
using Network;
using PlayerHub;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private CashUI _cashUI;

        [Header("Buttons")]
        [SerializeField] private Button _goToGameButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _modifyButton;
        [SerializeField] private TextMeshProUGUI _modifyButtonText;

        private CarSaveLoadController _carSaveLoadController;
        private PlayerDataController _playerDataController;
        private MessagesService _messagesService;
        private NetworkService _networkService;

        [Inject]
        public void Construct(MainData mainData, CarSaveLoadController carSaveLoadController,
            PlayerDataController playerDataController, MessagesService messagesService, NetworkService networkService)
        {
            _carsDataListSO = mainData.CarsDataList;
            _carSaveLoadController = carSaveLoadController;
            _playerDataController = playerDataController;
            _messagesService = messagesService;
            _networkService = networkService;
            _garageCarSwitcher.OnCarSwitch += CarSwitch;
            _cashUI.UpdateCash(_playerDataController.Cash);
            _playerDataController.OnCashChanged += _cashUI.UpdateCash;
            _garageCarSwitcher.Init(_carsDataListSO.GetAllCars(),_carPoint, carSaveLoadController);
        }
        
        private void OpenModify()
        {
            _garageModify.Init(_garageCarSwitcher.CurrentCar.CarStyle, _garageCarSwitcher.CurrentCarController,_carSaveLoadController, _playerDataController, _messagesService);
            _garageCarSwitcher.Close();
        }

        private void CarSwitch(int carId)
        {
            bool isHas = _playerDataController.OpenedCars.Contains(carId);
            if(isHas)
                _playerDataController.SetCurrentCar(carId);
            CheckModify(isHas, carId);
            CheckReadyToGame(isHas);
        }

        private void CheckReadyToGame(bool isHas)
        {
            if (isHas)
            {
                _goToGameButton.gameObject.SetActive(true);
                _goToGameButton.AddListener(GoToGame); 
                
                _joinButton.gameObject.SetActive(true);
                _joinButton.AddListener(Join);
            }
            else
            {
                _goToGameButton.gameObject.SetActive(false);
                _goToGameButton.RemoveAllListeners(); 
                
                _joinButton.gameObject.SetActive(false);
                _joinButton.RemoveAllListeners();
            }
        }

        private void CheckModify(bool isHas, int carId)
        {
            ResetModifyButton();
            if (isHas)
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

        private void GoToGame()
        {
            // SceneManager.LoadSceneAsync("GameScene");
            _networkService.CreateRoom();
        }
        
        private void Join()
        {
            // SceneManager.LoadSceneAsync("GameScene");
            _networkService.JoinRandomRoom();
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
                _messagesService.InitMessage(string.Empty, "U buy a car");
                CheckOnEmptyStyles();
            }
        }
        
        private void CheckOnEmptyStyles()
        {
            PlayerCarData curData =
                _carSaveLoadController.PlayerCarsData.FirstOrDefault(_ => _.CarId == _garageCarSwitcher.CurrentCar.Id);
            if (curData == default)
            {
                curData = new PlayerCarData()
                {
                    CarId = _garageCarSwitcher.CurrentCar.Id,
                    Data = new CarStyleData()
                };
                curData.Data.ColorId = _garageCarSwitcher.CurrentCar.CarStyle.GetCarColor(0).Item1;
                _carSaveLoadController.Save(curData);
            }
        }
        
        private void ResetModifyButton()
        {
            _modifyButton.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            _garageCarSwitcher.OnCarSwitch -= CarSwitch;
            _playerDataController.OnCashChanged -= _cashUI.UpdateCash;
        }
    }
}