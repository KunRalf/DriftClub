using System;
using System.Collections.Generic;
using Car.CarComponents;
using UnityEngine;

namespace Car
{
    public class CarMovement : MonoBehaviour
    {
        public event Action OnDriftStarted;
        public event Action<float> OnDriftEnded;
        public event Action<float> OnDriftProgress;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private List<Wheel> _forwardWheels;
        [SerializeField] private List<Wheel> _backWheels;
        [SerializeField] private CarParamsSO _carParamsSo;

        private float _brakeInput;
        private float _verticalInput;
        private float _horizontalInput;
        private float _speed;
        private float _slipAngle;

        private bool _isDrifting;
        private float _driftStartTime;
        private float _driftPoints;
        private float _minDriftAngle = 30f; 
        private float _maxDriftAngle = 90f; 
        private float _minSpeedForDrift = 5f;
        
        
        public void Init(CarParamsSO carParamsSo)
        {
            _carParamsSo = carParamsSo;
        }
        
        private void Update()
        {
            _speed = _rigidbody.velocity.magnitude;
            CheckInput();
            CheckDrift();
            CalculateSlipAngle();
        }
        
        private void FixedUpdate()
        {
            Motor();
            ApplySteering();
            Brake();
            LimitSpeed();
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
                wheel.WheelCollider.brakeTorque = _brakeInput * _carParamsSo.BrakePower * 0.7f;
            }

            foreach (var wheel in _backWheels)
            {
                wheel.WheelCollider.brakeTorque = _brakeInput * _carParamsSo.BrakePower * 0.3f;
            }
        }

        private void Motor()
        {
            foreach (var wheel in _backWheels)
            {
                wheel.WheelCollider.motorTorque = _carParamsSo.MotorPower * _verticalInput;
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
            float steeringAngle = _horizontalInput * _carParamsSo.SteeringCurve.Evaluate(_speed);
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
        
        private void CalculateSlipAngle()
        {
            _slipAngle = Vector3.Angle(transform.forward, _rigidbody.velocity - transform.forward);
        }
        
        private void CheckDrift()
        {
            if (_speed > _minSpeedForDrift && _slipAngle > _minDriftAngle && _slipAngle < _maxDriftAngle)
            {
                if (!_isDrifting)
                {
                    _isDrifting = true;
                    _driftStartTime = Time.time;
                    OnDriftStarted?.Invoke();
                }
                
                float driftDuration = Time.time - _driftStartTime;
                _driftPoints += driftDuration * _slipAngle * 0.01f;
                OnDriftProgress?.Invoke(_driftPoints);
            }
            else
            {
                if (_isDrifting)
                {
                    _isDrifting = false;
                    OnDriftEnded?.Invoke(_driftPoints);
                    _driftPoints = 0;
                }
            }
        }
        
        private void LimitSpeed()
        {
            if (_rigidbody.velocity.magnitude > _carParamsSo.MaxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _carParamsSo.MaxSpeed;
            }
        }
    }
}