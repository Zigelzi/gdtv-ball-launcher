using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    public class Emotion : MonoBehaviour, IDemolishable
    {
        [SerializeField] float _maxSpeed = 30f;
        [SerializeField] float _lifetime = 2f;

        Rigidbody2D _emotionRb;

        public static event Action onEmotionDemolish;
        void Awake()
        {
            _emotionRb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            LimitVelocity();
        }

        public void Demolish()
        {
            StopCoroutine(DestroyAfterLifeTimeExpires());
            Destroy(gameObject);
            onEmotionDemolish?.Invoke();
        }

        public void StartLifetimeExpiry()
        {
            StartCoroutine(DestroyAfterLifeTimeExpires());
        }

        IEnumerator DestroyAfterLifeTimeExpires()
        {
            yield return new WaitForSeconds(_lifetime);
            Demolish();
        }

        void LimitVelocity()
        {
            float maxSpeedX = Mathf.Clamp(_emotionRb.velocity.x, -_maxSpeed, _maxSpeed);
            float maxSpeedY = Mathf.Clamp(_emotionRb.velocity.y, -_maxSpeed, _maxSpeed);
            _emotionRb.velocity = new Vector2(maxSpeedX, maxSpeedY);
        }
    }
}
