using Infrastructure.SaveLoad;
using UnityEngine;
using Zenject;

namespace Car
{
    public class CarMainController : MonoBehaviour
    {
        [SerializeField] private CarMovement _carMovement;
        [SerializeField] private CarStyle _carStyle;
        
        public int Id { get; private set; }
        #region Initialization

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
            
        }

        #endregion

        #region Save Load Car Syle

        private void GetCarStyleParams()
        {
            
        }

        public void SetCarStyleParams(CarStyleData data)
        {
            _carStyle.SetStyle(data);
        }  
        
        #endregion
        
     
    }
}