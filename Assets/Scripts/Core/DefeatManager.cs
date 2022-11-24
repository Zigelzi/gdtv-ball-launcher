using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    public class DefeatManager : MonoBehaviour
    {
        [SerializeField] float gracePeriodDuration = 5f;

        public static Action onGracePeriodEnd;

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
            yield return new WaitForSeconds(gracePeriodDuration);
            onGracePeriodEnd?.Invoke();
        }
    }
}
