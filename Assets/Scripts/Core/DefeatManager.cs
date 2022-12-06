using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Core
{
    public class DefeatManager : MonoBehaviour
    {
        [SerializeField] float _gracePeriodDuration = 5f;

        public UnityEvent onGracePeriodEnd;

        void Awake()
        {
            EmotionHandler.onEmotionsExhausted += HandleEmotionsExhausted;
        }

        void OnDisable()
        {
            EmotionHandler.onEmotionsExhausted -= HandleEmotionsExhausted;
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
