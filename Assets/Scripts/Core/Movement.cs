using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace DD.Core
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 20f;

        Rigidbody2D _rb;
        LinearAccelerationSensor accelerationSensor;
        UnityEngine.InputSystem.Gyroscope gyro;

        void Awake()
        {
            accelerationSensor = LinearAccelerationSensor.current;
            gyro = Gyroscope.current;

            _rb = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            InputSystem.EnableDevice(accelerationSensor);
            InputSystem.EnableDevice(gyro);
        }

        void Update()
        {
            MoveEmotion();
        }

        void OnDisable()
        {
            InputSystem.DisableDevice(accelerationSensor);
            InputSystem.EnableDevice(gyro);
        }

        void MoveEmotion()
        {
            if (accelerationSensor == null) return;

            Vector2 acceleration = accelerationSensor.acceleration.ReadValue() * movementSpeed;

            _rb.AddForce(acceleration, ForceMode2D.Impulse);
        }
    }
}
