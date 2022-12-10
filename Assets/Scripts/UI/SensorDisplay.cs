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


        // Update is called once per frame
        void Update()
        {
            UpdateGyro();
        }

        void UpdateGyro()
        {

            Gyroscope gyro = GetDevice<Gyroscope>(UnityEditor.EditorApplication.isRemoteConnected);

            EnableDeviceIfNeeded(gyro);

            if (gyro != null)
            {
                _textGyro.text = gyro.angularVelocity.ReadValue().ToString();
            }
        }

        TDevice GetDevice<TDevice>(bool isRemote) where TDevice : InputDevice
        {
            foreach(InputDevice device in InputSystem.devices)
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
