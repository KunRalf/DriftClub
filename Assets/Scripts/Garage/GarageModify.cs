using System.Collections.Generic;
using System.Linq;
using Car;
using Garage.UIPrefabs;
using Infrastructure.SaveLoad;
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
        
        [Header("Details")]
        [SerializeField] private DetailSelectPrefab _detailSelectPrefab;
        [SerializeField] private RectTransform _spawnDetailsPlace;
        private List<DetailSelectPrefab> _detailsPool = new List<DetailSelectPrefab>();

        private CarMainController _currentCarController;
        private CarSaveLoadController _carSaveLoadController;
        private PlayerCarData _playerCarData;

        public void Init(CarStyleSO carStyle, CarMainController currentCarController, CarSaveLoadController carStyleData)
        {
            InitColors(carStyle.Colors);
            InitDetails(carStyle.StyleObjects);
            _currentCarController = currentCarController;
            _carSaveLoadController = carStyleData;
            _playerCarData = carStyleData.PlayerCarsData.First(_ => _.CarId == currentCarController.Id);
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

        private void DefaultColors()
        {
            foreach (var color in _colorsPool)
            {
                Destroy(color.gameObject);
            }
            _colorsPool.Clear();
        }

        private void InitDetails(List<CarStyleObject> details)
        {
            DefaultDetails();
            foreach (var detail in details)
            {
                var prefab = Instantiate(_detailSelectPrefab, _spawnDetailsPlace);
                prefab.Init(detail, SetDetail);
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
    }
}