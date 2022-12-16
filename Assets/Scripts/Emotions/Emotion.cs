using System;
using System.Collections;
using UnityEngine;

namespace DD.Emotions
{
    public class Emotion : MonoBehaviour
    {
        [SerializeField] float _maxSpeed = 30f;
        [SerializeField] float _lifetime = 2f;

        Rigidbody2D _emotionRb;

        // TODO: Get rid of static event
        public static event Action<GameObject> onEmotionDemolish;
        void Awake()
        {
            _emotionRb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            LimitVelocity();
        }

        public void Demolish(GameObject source)
        {
            Destroy(gameObject);
            onEmotionDemolish?.Invoke(source);
        }

        void LimitVelocity()
        {
            float maxSpeedX = Mathf.Clamp(_emotionRb.velocity.x, -_maxSpeed, _maxSpeed);
            float maxSpeedY = Mathf.Clamp(_emotionRb.velocity.y, -_maxSpeed, _maxSpeed);
            _emotionRb.velocity = new Vector2(maxSpeedX, maxSpeedY);
        }
    }
}
