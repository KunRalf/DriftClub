using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using CarStore;
using Helpers;
using Infrastructure.SaveLoad;
using Services;
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

        private CarSaveLoadController _carSaveLoadController;

        [Inject]
        public void Construct(MainData mainData, CarSaveLoadController carSaveLoadController)
        {
            _carsDataListSO = mainData.CarsDataList;
            _carSaveLoadController = carSaveLoadController;
            _garageCarSwitcher.Init(_carsDataListSO.GetAllCars(),_carPoint, carSaveLoadController);
            _modifyButton.AddListener(OpenModify);
        }
        
        private void OpenModify()
        {
            _garageModify.Init(_garageCarSwitcher.CurrentCar.CarStyle, _garageCarSwitcher.CurrentCarController,_carSaveLoadController);
            _garageCarSwitcher.Close();
        }
    }
}