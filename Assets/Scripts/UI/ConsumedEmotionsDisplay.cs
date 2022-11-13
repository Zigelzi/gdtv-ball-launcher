using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumedEmotionsDisplay : MonoBehaviour
{
    [SerializeField] GameObject imagePrefab;

    void OnEnable()
    {
        EmotionStock.onEmotionConsumed += HandleEmotionConsumed;
    }

    void OnDisable()
    {
        EmotionStock.onEmotionConsumed -= HandleEmotionConsumed;
    }

    void HandleEmotionConsumed()
    {
        AddConsumedEmotion();
    }

    void AddConsumedEmotion()
    {
        if (imagePrefab == null) return;

        Vector2 spawnPosition = GetSpawnPosition();
        RectTransform instantiatedImage = Instantiate(imagePrefab, transform).GetComponent<RectTransform>();

        instantiatedImage.anchoredPosition = spawnPosition;

    }

    Vector2 GetSpawnPosition()
    {
        float lastXPosition = 0f;
        float xOffset = 20f;
        Vector2 spawnPosition = new Vector2(0, 0);

        if (transform.childCount > 0)
        {
            foreach (RectTransform emotion in transform)
            {
                lastXPosition += emotion.rect.width + xOffset;
            }
            spawnPosition = new Vector2(lastXPosition, 0);
        }

        return spawnPosition;

    }
}
