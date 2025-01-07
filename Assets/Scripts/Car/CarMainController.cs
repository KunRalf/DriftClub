using System;
using System.Linq;
using Car.UI;
using Infrastructure.SaveLoad;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Car
{
    public class CarMainController : MonoBehaviour
    {
        [SerializeField] private CarMovement _carMovement;
        [SerializeField] private CarStyle _carStyle;
        [SerializeField] private CarHUD _carHud;
        [field: SerializeField] public Transform CameraFollowTransform;
        
        private CarHUD _curCarHud;
        
        public int Id { get; private set; }
        
        #region Initialization

        private void Start()
        {
             InitToGame();
        }

        public void Init(CarStyleSO carStyleSo, CarParamsSO carParamsSo, int id)
        {
            Id = id;
            _carStyle.Init(carStyleSo);
            _carMovement.Init(carParamsSo);
        }
        
        public void InitToGarage()
        {
            _carMovement.enabled = false;
        }

        public void InitToGame()
        {
            InitHud();   
        }

        private void InitHud()
        {
            _carHud = Instantiate(_carHud);
            _carHud.Init();
            _carMovement.OnDriftStarted += _carHud.ShowDriftPoints;
            _carMovement.OnDriftEnded += _carHud.HideDriftPoints;
            _carMovement.OnDriftProgress += _carHud.ProgressPoints;
        }

        #endregion

    
        public void SetCarStyleParams(CarStyleData data)
        {
            _carStyle.SetStyle(data);
        }  
        
    }
}