using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmotionStock : MonoBehaviour
{
    [SerializeField] int maxEmotions = 5;
    [SerializeField] int emotionsRemaining = -1;

    void Awake()
    {
        emotionsRemaining = maxEmotions;
    }

    public void Consume()
    {
        if (emotionsRemaining > 0)
        {
            emotionsRemaining--;
        }
    }

    public bool HasEmotionsRemaining()
    {
        if (emotionsRemaining > 0)
        {
            return true;
        }

        return false;
    }
}
