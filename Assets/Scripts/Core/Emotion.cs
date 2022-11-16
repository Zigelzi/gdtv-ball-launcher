using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    public class Emotion : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 30f;
        [SerializeField] float lifetime = 2f;

        Rigidbody2D emotionRb;

        public static event Action onEmotionDestroy;
        void Awake()
        {
            emotionRb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            LimitVelocity();
        }

        public void DestroyEmotion()
        {
            StartCoroutine(DestroyAfterLifeTimeExpires());
        }

        IEnumerator DestroyAfterLifeTimeExpires()
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(gameObject);
            onEmotionDestroy?.Invoke();
        }

        void LimitVelocity()
        {
            float maxSpeedX = Mathf.Clamp(emotionRb.velocity.x, -maxSpeed, maxSpeed);
            float maxSpeedY = Mathf.Clamp(emotionRb.velocity.y, -maxSpeed, maxSpeed);
            emotionRb.velocity = new Vector2(maxSpeedX, maxSpeedY);
        }
    }
}
