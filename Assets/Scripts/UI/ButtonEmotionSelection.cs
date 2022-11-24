using DD.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.UI
{
    public class ButtonEmotionSelection : MonoBehaviour
    {
        EmotionHandler _emotionHandler;
        void Awake()
        {
            _emotionHandler = FindObjectOfType<EmotionHandler>();
        }

        public void NextEmotion()
        {
            _emotionHandler.NextEmotion();
        }

        public void PreviousEmotion()
        {
            _emotionHandler.PreviousEmotion();
        }
    }
}
