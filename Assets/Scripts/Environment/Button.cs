using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class Button : MonoBehaviour
    {
        [SerializeField] Door _targetDoor;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Emotion"))
            {
                OpenDoor();
            }
        }

        void OpenDoor()
        {
            if (_targetDoor == null) return;

            _targetDoor.Open();
        }
    }
}
