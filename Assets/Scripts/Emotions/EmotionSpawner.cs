using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using DD.Environment;
using DD.Mood;

namespace DD.Emotions
{
    public class EmotionSpawner : MonoBehaviour
    {
        [SerializeField] Emotion _currentEmotion;
        [SerializeField] List<Emotion> _availableEmotions = new List<Emotion>();
        [SerializeField] Emotion _selectedEmotion;

        EmotionStock _emotionStock;
        MoodAdjuster _moodAdjuster;

        public static Action onEmotionsExhausted;

        void Awake()
        {
            _emotionStock = GetComponent<EmotionStock>();
            _moodAdjuster = FindObjectOfType<MoodAdjuster>();
            _selectedEmotion = _availableEmotions[0];
        }

        void OnEnable()
        {
            Emotion.onEmotionDemolish += HandleEmotionDestroyed;
            _moodAdjuster.onHappyMood.AddListener(HandleHappyMood);
        }

        void Start()
        {
            RespawnEmotion(_selectedEmotion);
        }

        void Update()
        {

        }

        void OnDisable()
        {
            Emotion.onEmotionDemolish -= HandleEmotionDestroyed;

            _moodAdjuster.onHappyMood.RemoveListener(HandleHappyMood);
        }

        public void NextEmotion()
        {
            int currentEmotionIndex = _availableEmotions.IndexOf(_selectedEmotion);
            int nextEmotionIndex = currentEmotionIndex + 1;
            if (nextEmotionIndex >= _availableEmotions.Count)
            {
                _selectedEmotion = _availableEmotions[0];
            }
            else
            {
                _selectedEmotion = _availableEmotions[nextEmotionIndex];
            }

            SwapEmotion(_selectedEmotion);
        }

        public void PreviousEmotion()
        {
            int currentEmotionIndex = _availableEmotions.IndexOf(_selectedEmotion);
            int previousEmotionIndex = currentEmotionIndex - 1;
            if (previousEmotionIndex < 0)
            {
                _selectedEmotion = _availableEmotions[_availableEmotions.Count - 1];
            }
            else
            {
                _selectedEmotion = _availableEmotions[previousEmotionIndex];
            }

            SwapEmotion(_selectedEmotion);
        }

        void SwapEmotion(Emotion newEmotion)
        {
            if (_availableEmotions == null || 
                _availableEmotions.Count == 0 ||
                _currentEmotion == null) return;

            Destroy(_currentEmotion.gameObject);
            RespawnEmotion(newEmotion);
        }

        bool IsTouchingUI()
        {
            if (Touchscreen.current.primaryTouch.press.isPressed && 
                EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            return false;
        }

        void RespawnEmotion(Emotion emotion)
        {
            int spawnedEmotionIndex;

            if (_availableEmotions == null || _availableEmotions.Count == 0) return;

            spawnedEmotionIndex = _availableEmotions.IndexOf(emotion);

            _currentEmotion = Instantiate(_availableEmotions[spawnedEmotionIndex],
                transform.position,
                Quaternion.identity);

        }

        void HandleEmotionDestroyed(GameObject emotion)
        {
            if (_emotionStock.HasEmotionsRemaining())
            {
                RespawnEmotion(_selectedEmotion);
            }
            else
            {
                onEmotionsExhausted?.Invoke();
            }
        }

        void HandleHappyMood()
        {
            enabled = false;
        }
    }
}
