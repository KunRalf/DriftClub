using Infrastructure.SaveLoad;
using UnityEngine;
using Zenject;

namespace Car
{
    public class CarMainController : MonoBehaviour
    {
        [SerializeField] private CarMovement _carMovement;
        [SerializeField] private CarStyle _carStyle;


        [Inject]
        public void Construct(CarSaveLoadController carSaveLoadController)
        {
        }
        
        #region Initialization

        public void InitToGarage()
        {
            
        }

        public void InitToGame()
        {
            
        }

        #endregion

        #region Save Load Car Syle

        private void GetCarStyleParams()
        {
            
        }

        private void SetCarStyleParams()
        {
            
        }

        #endregion
        
     
    }
}