using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace DD.UI
{
    public class SensorDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text _textAcceleration;
        [SerializeField] TMP_Text _textGyro;

        void OnEnable()
        {
            if (IsSensorsSupported())
            {
                InputSystem.EnableDevice(Gyroscope.current);
            }
        }

        void OnDisable()
        {
            if(IsSensorsSupported())
            {
                InputSystem.DisableDevice(Gyroscope.current);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            string acceleration;
            string gyroAcceleration;

            if (IsSensorsSupported())
            {
                gyroAcceleration = Gyroscope.current.angularVelocity.ReadValue().ToString();
                _textGyro.text = gyroAcceleration;
            }
            else
            {
                _textAcceleration.text = "Sensors not supported";
                _textGyro.text = "Sensors not supported";
            }
        }

        bool IsSensorsSupported()
        {
            if (SystemInfo.supportsGyroscope) return true;

            return false;
        }
    }
}
