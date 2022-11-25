using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class CollisionEffect : MonoBehaviour
    {
        [SerializeField] GameObject _vfxPrefab;

        bool _hasCollided = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_vfxPrefab == null || _hasCollided) return;

            if (collision.collider.CompareTag("Emotion"))
            {
                Instantiate(_vfxPrefab, collision.contacts[0].point, Quaternion.identity);
            }
        }
    }
}
