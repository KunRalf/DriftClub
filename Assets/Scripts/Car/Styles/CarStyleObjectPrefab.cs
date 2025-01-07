using UnityEngine;

namespace Car
{
    public class CarStyleObjectPrefab : MonoBehaviour
    {
        [field:SerializeField] public CarDetailsEnum Type { get; private set; }
    }
}