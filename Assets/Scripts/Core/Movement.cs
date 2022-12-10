using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace DD.Core
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 2f;

        Rigidbody2D _rb;
        LinearAccelerationSensor accelerationSensor;
        Gyroscope gyro;
        GravitySensor gravitySensor;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            gyro = GetDevice<Gyroscope>(UnityEditor.EditorApplication.isRemoteConnected);

            EnableDeviceIfNeeded(gyro);
            MoveEmotion();
        }

        void MoveEmotion()
        {
            if (gyro == null) return;

            float upwardsForce = Mathf.Abs(gyro.angularVelocity.ReadValue().z) * movementSpeed;
            Vector2 acceleration = new Vector2(0, upwardsForce);

            _rb.AddForce(acceleration, ForceMode2D.Impulse);
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
