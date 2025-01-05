using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Car
{
    public class CarStyle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _bodyCarMeshRenderer;
        [SerializeField] private CarStyleSO _carStyleSo;
        [SerializeField] private List<CarDetailPlaces> _detailPlaces;
        private Material _bodyCarMaterial;

        private void Awake()
        {
            _bodyCarMaterial = _bodyCarMeshRenderer.materials[0];
        }

        private void Start()
        {
            var a = new CarStyleData()
            {
                ColorId = 3,
                StyleObjects = new List<CarDetailsEnum>() { CarDetailsEnum.Spoiler, CarDetailsEnum.AirIntake,CarDetailsEnum.Spoiler }
            };
            new JsonManager<CarStyleData>().SaveJson("CarStyle",a);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var a = new JsonManager<CarStyleData>().LoadJson("CarStyle");
                SetStyle(a);
            }
        }

        public void SetStyle(CarStyleData styleData)
        {
            SetColor(styleData.ColorId);
            SetDetails(styleData.StyleObjects);
        }

        private void SetDetails(List<CarDetailsEnum> styleDataObj)
        {
            foreach (var data in styleDataObj)
            {
                var place = _detailPlaces.FirstOrDefault(_ => _.Type == data);
                if(place == default)  continue;
                if(!place.IsEmpty) continue;
                var detail = _carStyleSo.GetDetail(data);
                if (detail == null) continue;
                Instantiate(detail, place.Place.position, place.Place.rotation, transform);
                place.IsEmpty = false;
            }
        }

        private void SetColor(int colorId)
        {
            var color = _carStyleSo.Colors.FirstOrDefault(_ => _.Id == colorId);
            if (color == default)
            {
                _bodyCarMaterial.color = _carStyleSo.GetDefaultColor().Item2;
            }
            else
            {
                _bodyCarMaterial.color = color.Color;
            }
        }
    }
}