using System;
using System.Collections;
using UnityEngine;

namespace DD.Emotions
{
    public class Emotion : MonoBehaviour
    {
        // TODO: Get rid of static event
        public static event Action<GameObject> onEmotionDemolish;

        public void Demolish(GameObject source)
        {
            Destroy(gameObject);
            onEmotionDemolish?.Invoke(source);
        }

        
    }
}
