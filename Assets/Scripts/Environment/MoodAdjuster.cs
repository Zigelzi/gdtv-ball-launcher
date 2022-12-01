using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DD.Core;
using UnityEngine.Events;

namespace DD.Environment
{
    public class MoodAdjuster : MonoBehaviour
    {
        [SerializeField] int _hitEmotions = 0;
        [SerializeField] float _gravityMultiplier = 0.05f;
        [SerializeField] float _maxSpeed = 5f;
        [SerializeField] int _happyMoodTreshhold = 3;
        
        Rigidbody2D _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
        }

        void OnEnable()
        {
            Emotion.onEmotionDemolish += HandleEmotionDemolished;    
        }

        void Update()
        {
            HandleMood();
            _rb.velocity = new Vector2(0, Mathf.Clamp(_rb.velocity.y, -_maxSpeed, _maxSpeed));
        }

        void OnDisable()
        {
            Emotion.onEmotionDemolish -= HandleEmotionDemolished;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Emotion>(out Emotion hitEmotion))
            {
                AbsorbEmotion(hitEmotion);
            }    
        }

        void HandleMood()
        {
            _rb.gravityScale = _hitEmotions * -_gravityMultiplier;
        }

        void AbsorbEmotion(Emotion emotion)
        {
            emotion.Demolish(gameObject);
        }

        void HandleEmotionDemolished(GameObject source)
        {
            if (source == gameObject)
            {
                _hitEmotions++;
            }
            if (source.TryGetComponent<Boundary>(out Boundary boundary))
            {
                _hitEmotions--;
            }
        }
    }
}
