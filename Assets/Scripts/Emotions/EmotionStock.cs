using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Emotions
{
    public class EmotionStock : MonoBehaviour
    {
        [SerializeField] int _maxEmotions = 5;
        [SerializeField] int _emotionsRemaining = -1;

        public static Action onEmotionConsumed;

        void Awake()
        {
            _emotionsRemaining = _maxEmotions;
        }

        public void Consume()
        {
            if (_emotionsRemaining > 0)
            {
                _emotionsRemaining--;
                onEmotionConsumed?.Invoke();
            }
        }

        public bool HasEmotionsRemaining()
        {
            if (_emotionsRemaining > 0)
            {
                return true;
            }

            return false;
        }
    }
}
