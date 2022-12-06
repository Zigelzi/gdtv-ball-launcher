using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DD.Core;
using DD.Environment;

namespace DD.Mood
{
    public class MoodAdjuster : MonoBehaviour
    {
        [SerializeField] int _hitEmotions = 0;
        [SerializeField] float _happyMoodGravity = 1.5f;
        [SerializeField] float _maxVelocity = 3f;
        [SerializeField] int _happyMoodTreshhold = 3;
        
        Rigidbody2D _rb;

        public UnityEvent onEmotionHit;
        public UnityEvent onEmotionMiss;

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
            TriggerHappyMood();
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

        void TriggerHappyMood()
        {
            if (_hitEmotions >= _happyMoodTreshhold)
            {
                _rb.gravityScale = -_happyMoodGravity;
                _rb.velocity = new Vector2(0, Mathf.Clamp(_rb.velocity.y, -_maxVelocity, _maxVelocity));
            }
            
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
                onEmotionHit?.Invoke();
            }
            if (source.TryGetComponent<Boundary>(out Boundary boundary))
            {
                _hitEmotions--;
                onEmotionMiss?.Invoke();
            }
            
        }
    }
}
