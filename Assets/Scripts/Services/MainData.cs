using CarStore;
using UnityEngine;

namespace Services
{
    public class MainData : MonoBehaviour
    {
        [field: SerializeField] public CarsDataListSO CarsDataList { get; private set; }
    }
}