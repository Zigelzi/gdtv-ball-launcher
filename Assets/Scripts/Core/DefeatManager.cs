using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DD.Emotions;

namespace DD.Core
{
    public class DefeatManager : MonoBehaviour
    {
        [SerializeField] float _gracePeriodDuration = 5f;

        public UnityEvent onGracePeriodEnd;

        void Awake()
        {
            EmotionSpawner.onEmotionsExhausted += HandleEmotionsExhausted;
        }

        void OnDisable()
        {
            EmotionSpawner.onEmotionsExhausted -= HandleEmotionsExhausted;
        }

        void HandleEmotionsExhausted()
        {
            StartCoroutine(GracePeriod());
        }

        IEnumerator GracePeriod()
        {
            yield return new WaitForSeconds(_gracePeriodDuration);
            onGracePeriodEnd?.Invoke();
        }
    }
}
