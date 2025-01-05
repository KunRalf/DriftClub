using Car;
using CarStore;
using UnityEngine;

namespace Garage
{
    public class GarageController : MonoBehaviour
    {
        [SerializeField] private CarsDataListSO _carsDataListSO;
        
        private CarMainController _currentCar;
    }
}