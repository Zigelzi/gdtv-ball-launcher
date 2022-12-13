using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DD.Emotions;

namespace DD.UI
{
    public class ButtonEmotionSelection : MonoBehaviour
    {
        EmotionSpawner _emotionHandler;
        void Awake()
        {
            _emotionHandler = FindObjectOfType<EmotionSpawner>();
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
