using System;
using UnityEngine;

namespace Car.CarComponents
{
    [Serializable]
    public class Wheel
    {
        [field:SerializeField] public Transform WheelTransform { get; private set; }
        [field:SerializeField] public WheelCollider WheelCollider { get; private set; }
        [field:SerializeField] public ParticleSystem WheelSmoke { get; private set; }
        
        [SerializeField]private float _slipAllowance;

        public void UpdateWheelTransform()
        {
            WheelCollider.GetWorldPose(out Vector3 wheelPose, out Quaternion wheelRot);
            WheelTransform.position = wheelPose;
            WheelTransform.rotation = wheelRot;
        }

        public void SmokeParticle()
        {
            WheelCollider.GetGroundHit(out WheelHit wheelHit);
            if (Mathf.Abs(wheelHit.sidewaysSlip) + Mathf.Abs(wheelHit.forwardSlip) > _slipAllowance)
            {
                WheelSmoke.Play();
            }
            else
            {
                WheelSmoke.Stop();
            }
        }

    }
}