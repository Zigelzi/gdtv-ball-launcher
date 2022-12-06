using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Mood
{
    public class MoodResizer : MonoBehaviour
    {
        [SerializeField] float _sizeChangeStep = .5f;
        [SerializeField] float _sizeChangeSpeed = 1f;

        MoodAdjuster _moodAdjuster;

        void Awake()
        {
            _moodAdjuster = GetComponent<MoodAdjuster>();
        }
        void OnEnable()
        {
            _moodAdjuster.onEmotionHit.AddListener(HandleEmotionHit);
            _moodAdjuster.onEmotionMiss.AddListener(HandleEmotionMiss);
        }

        void OnDisable()
        {
            _moodAdjuster.onEmotionHit.RemoveListener(HandleEmotionHit);
            _moodAdjuster.onEmotionMiss.RemoveListener(HandleEmotionMiss);
        }

        void HandleEmotionHit()
        {

        }

        void HandleEmotionMiss()
        {
            StartCoroutine(Grow());
        }

        IEnumerator Grow()
        {
            Vector3 newSize = transform.localScale + (Vector3.one * _sizeChangeStep);
            while (transform.localScale.x < newSize.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, newSize, Time.deltaTime * _sizeChangeSpeed);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}