using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

using DD.Mood;

namespace DD.Emotions
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float bounceStrength = 4f;
        [SerializeField] float movementSpeed = 2f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float flyingTreshold = .1f;
        [SerializeField] float rotationSpeedTreshhold = 1.5f;
        [SerializeField] float _maxHorizonalSpeed = 8f;
        [SerializeField] float _maxUpwardSpeed = 15f;
        [SerializeField] float _maxDownwardSpeed = 30f;

        Rigidbody2D _rb;
        Gyroscope _gyro;
        GravitySensor _gravitySensor;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }


        void Update()
        {
            if (Application.isEditor)
            {
                _gyro = GetDevice<Gyroscope>(true);
                _gravitySensor = GetDevice<GravitySensor>(true);
            }
            else
            {
                _gyro = GetDevice<Gyroscope>(false);
                _gravitySensor = GetDevice<GravitySensor>(false);
            }
            
            EnableDeviceIfNeeded(_gyro);
            EnableDeviceIfNeeded(_gravitySensor);

            LimitVelocity();

            if (!IsFlying() && IsRotatingFast())
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

        bool IsRotatingFast()
        {
            if (_gyro == null) return false;

            float rotationSpeed = Mathf.Abs(_gyro.angularVelocity.ReadValue().z);
            if (rotationSpeed >= rotationSpeedTreshhold)
            {
                return true;
            }

            return false;
        }

        void Bounce()
        {
            if (_gyro == null) return;

            float upwardsForce = Mathf.Abs(_gyro.angularVelocity.ReadValue().z) * bounceStrength;
            Vector2 acceleration = new Vector2(0, upwardsForce);

            _rb.AddForce(acceleration, ForceMode2D.Impulse);
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
