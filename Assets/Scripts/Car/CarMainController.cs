using System;
using System.Linq;
using Car.UI;
using CarStore;
using Fusion;
using Helpers.Injector;
using Infrastructure;
using Infrastructure.SaveLoad;
using Level;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Car
{
    public class CarMainController : NetworkBehaviour
    {
        public event Action<int> OnUpdatePoints; 
        
        [SerializeField] private CarMovement _carMovement;
        [SerializeField] private CarStyle _carStyle;
        [SerializeField] private CarHUD _carHud;
        [SerializeField]private Transform _cameraFollowTransform;
        
        private CarHUD _curCarHud;
        private ICarDataProvider _carDataProvider;


        public int Id { get; private set; }

        [Inject]
        public void Construct(ICarDataProvider carDataProvider)
        {
            _carDataProvider = carDataProvider;
        }
        
        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                GameLevel.CurLevel?.SetPlayerCamera(_cameraFollowTransform);
            }
            
            Init(_carDataProvider.GetCarById(Id));
            InitToGame();
        }

        #region Initialization
        

        public void Init(CarData data)
        {
            Id = data.Id;
            _carStyle.Init(data.CarStyle);
            _carMovement.Init(data.CarParams);
        }
        
        public void InitToGarage()
        {
            _carMovement.enabled = false;
        }

        public void InitToGame()
        {
            if (Object.HasInputAuthority)
            {
                InitHud();
            }
        }

        private void InitHud()
        {
            _carHud = Instantiate(_carHud);
            _carHud.Init();
            _carMovement.OnDriftStarted += _carHud.ShowDriftPoints;
            _carMovement.OnDriftEnded += _carHud.HideDriftPoints;
            _carMovement.OnDriftEnded += (float cash) => OnUpdatePoints?.Invoke((int)cash);
            _carMovement.OnDriftProgress += _carHud.ProgressPoints;
        }

        #endregion

    
        public void SetCarStyleParams(CarStyleData data)
        {
            _carStyle.SetStyle(data);
        }

        public void SetCarId(int carId)
        {
            Id = carId;
        }
    }
}