using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace DD.Core
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float bounceStrength = 4f;
        [SerializeField] float movementSpeed = 2f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float flyingTreshold = .1f;
        [SerializeField] float rotationSpeedTreshhold = 1.5f;

        Rigidbody2D _rb;
        Gyroscope gyro;
        GravitySensor gravitySensor;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            gyro = GetDevice<Gyroscope>(UnityEditor.EditorApplication.isRemoteConnected);
            gravitySensor = GetDevice<GravitySensor>(UnityEditor.EditorApplication.isRemoteConnected);
            EnableDeviceIfNeeded(gyro);
            EnableDeviceIfNeeded(gravitySensor);

            if (!IsFlying() && IsRotatingFast())
            {
                Bounce();
            }
            
        }

        void FixedUpdate()
        {
            Roll();    
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
            float rotationSpeed = Mathf.Abs(gyro.angularVelocity.ReadValue().z);
            if (rotationSpeed >= rotationSpeedTreshhold)
            {
                return true;
            }

            return false;
        }

        void Bounce()
        {
            if (gyro == null) return;

            float upwardsForce = Mathf.Abs(gyro.angularVelocity.ReadValue().z) * bounceStrength;
            Vector2 acceleration = new Vector2(0, upwardsForce);

            _rb.AddForce(acceleration, ForceMode2D.Impulse);
        }

        void Roll()
        {
            if (gravitySensor == null) return;

            Vector2 horizontalForce = new Vector2(gravitySensor.gravity.ReadValue().y, 0);
            _rb.AddForce(-horizontalForce * movementSpeed, ForceMode2D.Impulse);
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
