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
            SetDetails(styleData.StyleObjects);
        }

        private void SetDetails(List<CarDetailsEnum> styleDataObj)
        {
            if (styleDataObj == null) return;

            // Удаляем запчасти, которых нет в актуальном списке
            foreach (var detailOnCar in
                     _detailsOnCar.ToList()) // Используем ToList(), чтобы избежать ошибок при изменении коллекции
            {
                if (!styleDataObj.Contains(detailOnCar.Type))
                {
                    // Удаляем запчасть
                    Destroy(detailOnCar.gameObject);

                    // Освобождаем место, если оно связано с этой запчастью
                    var place = _detailPlaces.FirstOrDefault(_ => _.Type == detailOnCar.Type);
                    if (place != default)
                    {
                        place.IsEmpty = true;
                    }

                    // Удаляем запчасть из списка
                    _detailsOnCar.Remove(detailOnCar);
                }
            }

            // Добавляем или обновляем запчасти из актуального списка
            List<CarStyleObjectPrefab> updatedParts = new List<CarStyleObjectPrefab>();
            foreach (var part in styleDataObj)
            {
                var partDetail = part;

                // Проверяем, есть ли такая запчасть на автомобиле
                var detailOnCar = _detailsOnCar.FirstOrDefault(_ => _.Type == partDetail);
                if (detailOnCar == default)
                {
                    // Если запчасти нет, создаем её
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
                    // Если запчасть уже есть, просто добавляем её в обновленный список
                    updatedParts.Add(detailOnCar);
                }
            }

            // Обновляем список запчастей на автомобиле
            _detailsOnCar = updatedParts;
        }

        private void SetColor(int colorId)
        {
            _bodyCarMaterial.color = _carStyleSo.GetCarColor(colorId).Item2;
        }
    }
}