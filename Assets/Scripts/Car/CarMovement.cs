using System;
using System.Collections.Generic;
using Car.CarComponents;
using UnityEngine;

namespace Car
{
    public class CarMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private List<Wheel> _forwardWheels;
        [SerializeField] private List<Wheel> _backWheels;
        [SerializeField] private CarParams _carParams;

        private float _brakeInput;
        private float _verticalInput;
        private float _horizontalInput;
        private float _speed;
        private float _slipAngle;

        private void Update()
        {
            _speed = _rigidbody.velocity.magnitude;
            CheckInput();
        }

        private void FixedUpdate()
        {
            Motor();
            ApplySteering();
            Brake();
        }

        private void CheckInput()
        {
            _verticalInput = Input.GetAxis("Vertical");
            _horizontalInput = Input.GetAxis("Horizontal");

         
            float movingDirection = Vector3.Dot(transform.forward, _rigidbody.velocity);
            if (movingDirection < -0.5f && _verticalInput > 0)
            {
                _brakeInput = Mathf.Abs(_verticalInput);
            }
            else if (movingDirection > 0.5f && _verticalInput < 0)
            {
                _brakeInput = Mathf.Abs(_verticalInput);
            }
            else
            {
                _brakeInput = 0;
            }
        }

        private void Brake()
        {
            foreach (var wheel in _forwardWheels)
            {
                wheel.WheelCollider.brakeTorque = _brakeInput * _carParams.BrakePower * 0.7f;
            }

            foreach (var wheel in _backWheels)
            {
                wheel.WheelCollider.brakeTorque = _brakeInput * _carParams.BrakePower * 0.3f;
            }
        }

        private void Motor()
        {
            foreach (var wheel in _backWheels)
            {
                wheel.WheelCollider.motorTorque = _carParams.MotorPower * _verticalInput;
                wheel.UpdateWheelTransform();
                wheel.SmokeParticle();
            }

            foreach (var wheel in _forwardWheels)
            {
                wheel.UpdateWheelTransform();
                wheel.SmokeParticle();
            }
        }
        
        private void ApplySteering()
        {
            float steeringAngle = _horizontalInput * _carParams.SteeringCurve.Evaluate(_speed);
            _slipAngle = Vector3.Angle(transform.forward, _rigidbody.velocity - transform.forward);
            if (_slipAngle < 120f)
            {
                steeringAngle += Vector3.SignedAngle(transform.forward, _rigidbody.velocity + transform.forward, Vector3.up);
            }
            steeringAngle = Mathf.Clamp(steeringAngle, -60, 60);
            foreach (var wheel in _forwardWheels)
            {
                wheel.WheelCollider.steerAngle = steeringAngle;
            }
        }
    }
}