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
            if(styleDataObj == null) return;
            foreach (var data in styleDataObj)
            {
                var place = _detailPlaces.FirstOrDefault(_ => _.Type == data);
                if(place == default)  continue;
                if(!place.IsEmpty) continue;
                var detail = _carStyleSo.GetDetail(data);
                if (detail.Item2 == null) continue;
                Instantiate(detail.Item2, place.Place.position, place.Place.rotation, transform);
                place.IsEmpty = false;
            }
        }

        private void SetColor(int colorId)
        {
            _bodyCarMaterial.color = _carStyleSo.GetCarColor(colorId).Item2;
        }
    }
}