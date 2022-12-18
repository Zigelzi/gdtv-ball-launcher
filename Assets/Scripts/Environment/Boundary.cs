using DD.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DD.Emotions;

namespace DD.Environment
{
    public class Boundary : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Emotion>(out Emotion emotion))
            {
                emotion.Demolish(gameObject);
            }
        }
    }
}
