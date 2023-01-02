using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DD.Emotions
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float bounceStrength = 4f;
        [SerializeField] float movementSpeed = 2f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float flyingTreshold = .1f;
        [SerializeField] float _verticalAccelerationThreshold = .15f;
        [SerializeField] float _maxHorizonalSpeed = 8f;
        [SerializeField] float _maxUpwardSpeed = 15f;
        [SerializeField] float _maxDownwardSpeed = 30f;

        Rigidbody2D _rb;
        GravitySensor _gravitySensor;
        LinearAccelerationSensor _linearAccelerationSensor;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }


        void Update()
        {
            if (Application.isEditor)
            {
                _gravitySensor = GetDevice<GravitySensor>(true);
                _linearAccelerationSensor = GetDevice<LinearAccelerationSensor>(true);
            }
            else
            {
                _gravitySensor = GetDevice<GravitySensor>(false);
                _linearAccelerationSensor = GetDevice<LinearAccelerationSensor>(false);
            }

            EnableDeviceIfNeeded(_gravitySensor);
            EnableDeviceIfNeeded(_linearAccelerationSensor);

            LimitVelocity();

            if (!IsFlying() && IsMovingFastVertically())
            {
                Bounce();
            }

        }

        void FixedUpdate()
        {
            Roll();    
        }

        void LimitVelocity()
        {
            float clampedXVelocity = Mathf.Clamp(_rb.velocity.x, -_maxHorizonalSpeed, _maxHorizonalSpeed);
            float clampedYVelocity = Mathf.Clamp(_rb.velocity.y, -_maxDownwardSpeed, _maxUpwardSpeed);
            _rb.velocity = new Vector2(clampedXVelocity, clampedYVelocity);
        }

        bool IsFlying()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
            float colliderRadius = GetComponent<CircleCollider2D>().radius;
            float distanceFromGround = hit.distance - colliderRadius;

            if (distanceFromGround >= flyingTreshold)
            {
                return true;
            }

            return false;
        }

        bool IsMovingFastVertically()
        {
            if (_linearAccelerationSensor == null) return false;

            float verticalAcceleration = Mathf.Abs(_linearAccelerationSensor.acceleration.ReadValue().x);
            
            if (verticalAcceleration >= _verticalAccelerationThreshold)
            {
                return true;
            }

            return false;
        }

        void Bounce()
        {
            if (_linearAccelerationSensor == null) return;

            float verticalAcceleration = -_linearAccelerationSensor.acceleration.ReadValue().x;

            if (verticalAcceleration > 0)
            {
                float acceleration = verticalAcceleration * bounceStrength;
                Vector2 upwardsForce = new Vector2(0, acceleration);

                _rb.AddForce(upwardsForce, ForceMode2D.Impulse);
            }

            
        }

        void Roll()
        {
            Vector2 horizontalForce;
            if (_gravitySensor == null) return;

            // Unity Remote reads the sensor axises differently from the device
            // Handle the difference in axises for testing purposes

            if (Application.isEditor)
            {
                horizontalForce = new Vector2(-_gravitySensor.gravity.ReadValue().y, 0);
            }
            else
            {
                horizontalForce = new Vector2(_gravitySensor.gravity.ReadValue().x, 0);
            }
            
            _rb.AddForce(horizontalForce * movementSpeed, ForceMode2D.Impulse);
        }

        TDevice GetDevice<TDevice>(bool isRemote) where TDevice : InputDevice
        {
            foreach (InputDevice device in InputSystem.devices)
            {
                if (isRemote)
                {
                    if (device.remote && device is TDevice deviceOfType)
                    {
                        return deviceOfType;
                    }
                }
                else
                {
                    if (device is TDevice deviceOfType)
                    {
                        return deviceOfType;
                    }
                }
            }
            return default;
        }

        void EnableDeviceIfNeeded(InputDevice device)
        {
            if (device != null && !device.enabled)
            {
                InputSystem.EnableDevice(device);
            }
        }
    }
}
