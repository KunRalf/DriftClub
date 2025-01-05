using UnityEngine;

namespace Car
{
    [CreateAssetMenu(fileName = "CarParams", menuName = "CarSettings/CarParams", order = 0)]
    public class CarParamsSO : ScriptableObject
    {
        [field:SerializeField] public float MotorPower { get; private set; }
        [field:SerializeField] public AnimationCurve SteeringCurve { get; private set; }
        [field:SerializeField] public float BrakePower { get; private set; }
        [field:SerializeField] public float HandBrakePower { get; private set; }
       
    }
}