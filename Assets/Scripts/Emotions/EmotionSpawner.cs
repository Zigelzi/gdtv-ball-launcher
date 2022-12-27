using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using DD.Mood;

namespace DD.Emotions
{
    public class EmotionSpawner : MonoBehaviour
    {
        [SerializeField] Emotion _currentEmotion;
        [SerializeField] List<Emotion> _availableEmotions = new List<Emotion>();
        [SerializeField] Emotion _selectedEmotion;

        MoodAdjuster _moodAdjuster;

        void Awake()
        {
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

        void RespawnEmotion(Emotion emotion)
        {
            int spawnedEmotionIndex;

            if (_availableEmotions == null || 
                _availableEmotions.Count == 0) return;

            if (_moodAdjuster.HasEmotionsAvailable())
            {
                spawnedEmotionIndex = _availableEmotions.IndexOf(emotion);

                _currentEmotion = Instantiate(_availableEmotions[spawnedEmotionIndex],
                    transform.position,
                    Quaternion.identity);
            }
            

        }

        void HandleEmotionDestroyed(GameObject emotion)
        {
            RespawnEmotion(_selectedEmotion);
        }

        void HandleHappyMood()
        {
            enabled = false;
        }
    }
}
