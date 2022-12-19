using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DD.Emotions;
using DD.Environment;

namespace DD.Mood
{
    public class MoodAdjuster : MonoBehaviour
    {
        [SerializeField] int _hitEmotions = 0;
        [SerializeField] float _happyMoodGravity = 1.5f;
        [SerializeField] float _maxVelocity = 3f;
        [SerializeField][Range(1, 5)] int _happyMoodTreshhold = 2;
        [SerializeField][Range(-1, -5)] int _repressedEmotionsThreshold = -2;
        
        Rigidbody2D _rb;

        public UnityEvent onEmotionHit;
        public UnityEvent onEmotionMiss;
        public UnityEvent onHappyMood;
        public UnityEvent onEmotionsRepressed;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            Emotion.onEmotionDemolish += HandleEmotionDemolished;    
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

        public bool HasEmotionsAvailable()
        {
            if (_hitEmotions >= _repressedEmotionsThreshold && 
                _hitEmotions <= _happyMoodTreshhold)
            {
                return true;
            }

            return false;
        }

        IEnumerator StartHappyMood()
        {
            yield return new WaitForSeconds(2f);
            onHappyMood?.Invoke();
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
                CheckHappyMood();
            }
            if (source.TryGetComponent<Boundary>(out Boundary boundary))
            {
                _hitEmotions--;
                onEmotionMiss?.Invoke();
                CheckRepressedEmotions();
            }
            
        }

        void CheckHappyMood()
        {
            if (_hitEmotions >= _happyMoodTreshhold)
            {
                _rb.gravityScale = -_happyMoodGravity;
                _rb.velocity = new Vector2(0, Mathf.Clamp(_rb.velocity.y, -_maxVelocity, _maxVelocity));
                StartCoroutine(StartHappyMood());
            }

        }

        void CheckRepressedEmotions()
        {
            if (_hitEmotions <= _repressedEmotionsThreshold) {
                onEmotionsRepressed?.Invoke();
            }
        }
    }
}
