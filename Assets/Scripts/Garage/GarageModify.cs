using System.Collections.Generic;
using System.Linq;
using Car;
using Garage.UIPrefabs;
using Infrastructure.SaveLoad;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    public class GarageModify : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [Header("Colors")]
        [SerializeField] private ColorSelectPrefab _colorSelectPrefab;
        [SerializeField] private RectTransform _spawnColorsPlace;
        private List<ColorSelectPrefab> _colorsPool = new List<ColorSelectPrefab>();     
        [Header("SmokeOfWheel")]
        [SerializeField] private RectTransform _spawnSmokeOfWheelColorsPlace;
        private List<ColorSelectPrefab> _smokeOfWheelColorsPool = new List<ColorSelectPrefab>();
        
        [Header("Details")]
        [SerializeField] private DetailSelectPrefab _detailSelectPrefab;
        [SerializeField] private RectTransform _spawnDetailsPlace;
        private List<DetailSelectPrefab> _detailsPool = new List<DetailSelectPrefab>();

        private CarMainController _currentCarController;
        private CarSaveLoadController _carSaveLoadController;
        private PlayerCarData _playerCarData;
        private PlayerDataController _playerDataController;
        private MessagesService _messagesService;

        public void Init(CarStyleSO carStyle, CarMainController currentCarController, CarSaveLoadController carStyleData, PlayerDataController playerDataController, MessagesService messagesService)
        {
            _currentCarController = currentCarController;
            _carSaveLoadController = carStyleData;
            _messagesService = messagesService;
            _playerDataController = playerDataController;
            _playerCarData = carStyleData.GetPlayerCarById(currentCarController.Id);
            InitColors(carStyle.Colors);
            InitSmokeOfWheelColors(carStyle.SmokeColors);
            InitDetails(carStyle.StyleObjects);
            _panel.SetActive(true);
        }

        private void InitColors(List<CarColor> colors)
        {
            DefaultColors();
            foreach (var color in colors)
            {
                var prefab = Instantiate(_colorSelectPrefab, _spawnColorsPlace);
                prefab.Init(color, SetColor);
                _colorsPool.Add(prefab);
            }
        }       
        
        private void InitSmokeOfWheelColors(List<CarColor> colors)
        {
            DefaultSmokeOfWheelColors();
            foreach (var color in colors)
            {
                var prefab = Instantiate(_colorSelectPrefab, _spawnSmokeOfWheelColorsPlace);
                prefab.Init(color, SetSmokeOfWheelColor);
                _smokeOfWheelColorsPool.Add(prefab);
            }
        }

        private void DefaultColors()
        {
            foreach (var color in _colorsPool)
            {
                Destroy(color.gameObject);
            }
            _colorsPool.Clear();
        }  
        
        private void DefaultSmokeOfWheelColors()
        {
            foreach (var color in _smokeOfWheelColorsPool)
            {
                Destroy(color.gameObject);
            }
            _smokeOfWheelColorsPool.Clear();
        }

        private void InitDetails(List<CarStyleObject> details)
        {
            DefaultDetails();
            foreach (var detail in details)
            {
                var prefab = Instantiate(_detailSelectPrefab, _spawnDetailsPlace);
              
                prefab.Init(detail,_playerCarData.Data.PurchasedDetails.Contains(detail.Type), BuyDetail);
                _detailsPool.Add(prefab);
            }
        }
        
        private void DefaultDetails()
        {
            foreach (var detail in _detailsPool)
            {
                Destroy(detail.gameObject);
            }
            _detailsPool.Clear();
        }


        private void SetColor(int id)
        {
            _playerCarData.Data.ColorId = id;
            _currentCarController.SetCarStyleParams( _playerCarData.Data);
            _carSaveLoadController.Save( _playerCarData);
        }   
        
        private void SetSmokeOfWheelColor(int id)
        {
            _playerCarData.Data.SmokeOfWheelColoId = id;
            _currentCarController.SetCarStyleParams(_playerCarData.Data);
            _carSaveLoadController.Save( _playerCarData);
        }

        private void SetDetail(CarDetailsEnum type)
        {
            if (_playerCarData.Data.StyleObjects.Contains(type))
            {
                _playerCarData.Data.StyleObjects.Remove(type);
            }
            else
            {
                _playerCarData.Data.StyleObjects.Add(type);
            }
            _currentCarController.SetCarStyleParams( _playerCarData.Data);
            _carSaveLoadController.Save(_playerCarData);
        }

        private void BuyDetail(CarStyleObject styleObject)
        {
            if (_playerCarData.Data.PurchasedDetails.Contains(styleObject.Type))
            {
                SetDetail(styleObject.Type);
                return;
            }
            if (_playerDataController.Cash < styleObject.Cost)
            {
                _messagesService.InitMessage(string.Empty, "No money to buy");
            }
            else
            {
                _carSaveLoadController.AddPurchasedDetails(_currentCarController.Id, styleObject.Type);
                SetDetail(styleObject.Type);
                _playerDataController.UpdateCash(_playerDataController.Cash - styleObject.Cost);
                _detailsPool.First(_ => _.Type == styleObject.Type).UpdateState(_playerCarData.Data.PurchasedDetails.Contains(styleObject.Type));
            }
        }
    }
}