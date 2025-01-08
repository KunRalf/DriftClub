using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Car
{
    public class CarStyle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _bodyCarMeshRenderer;
        [SerializeField] private List<CarDetailPlaces> _detailPlaces;
        [SerializeField] private List<ParticleSystem> _smokeOfWheels;
        private Material _bodyCarMaterial;
        private CarStyleSO _carStyleSo;

        private List<CarStyleObjectPrefab> _detailsOnCar = new List<CarStyleObjectPrefab>();
        
        private const string BODY_MATERIAL_KEY = "Car_Body (Instance)";
    
        public void Init(CarStyleSO carStyleSo)
        {
            _carStyleSo = carStyleSo;
            var mat = _bodyCarMeshRenderer.materials.FirstOrDefault(_ => _.name == BODY_MATERIAL_KEY);
            _bodyCarMaterial = mat;
            // загрузка сохраненного
        }
        

        public void SetStyle(CarStyleData styleData)
        {
            SetColor(styleData.ColorId);
            SetSmokeOfWheelsColor(styleData.SmokeOfWheelColoId);
            SetDetails(styleData.StyleObjects);
        }

        private void SetDetails(List<CarDetailsEnum> styleDataObj)
        {
            if (styleDataObj == null) return;
            
            foreach (var detailOnCar in
                     _detailsOnCar.ToList())
            {
                if (!styleDataObj.Contains(detailOnCar.Type))
                {
                    Destroy(detailOnCar.gameObject);
                    
                    var place = _detailPlaces.FirstOrDefault(_ => _.Type == detailOnCar.Type);
                    if (place != default)
                    {
                        place.IsEmpty = true;
                    }

                    _detailsOnCar.Remove(detailOnCar);
                }
            }
            
            List<CarStyleObjectPrefab> updatedParts = new List<CarStyleObjectPrefab>();
            foreach (var part in styleDataObj)
            {
                var partDetail = part;
                
                var detailOnCar = _detailsOnCar.FirstOrDefault(_ => _.Type == partDetail);
                if (detailOnCar == default)
                {
                    var place = _detailPlaces.FirstOrDefault(_ => _.Type == part);
                    if (place == default) continue;
                    if (!place.IsEmpty) continue;

                    var detail = _carStyleSo.GetDetail(part);
                    if (detail.Item2 == null) continue;

                    var detailPrefab = Instantiate(detail.Item2, place.Place.position, place.Place.rotation, transform);
                    _detailsOnCar.Add(detailPrefab);
                    place.IsEmpty = false;
                    updatedParts.Add(detailPrefab);
                }
                else
                {
                    updatedParts.Add(detailOnCar);
                }
            }
            
            _detailsOnCar = updatedParts;
        }

        private void SetColor(int colorId)
        {
            _bodyCarMaterial.color = _carStyleSo.GetCarColor(colorId).Item2;
        } 
        private void SetSmokeOfWheelsColor(int colorId)
        {
            foreach (var smoke in _smokeOfWheels)
            {
                var mainModule = smoke.main;
                mainModule.startColor = _carStyleSo.GetSmokeColor(colorId).Item2;
            }
        }
    }
}