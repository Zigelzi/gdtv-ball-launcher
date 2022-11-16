using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using DD.Environment;

namespace DD.Core
{
    public class EmotionHandler : MonoBehaviour
    {
        [SerializeField] GameObject currentSpringPivotPoint;
        [SerializeField] Emotion currentEmotion;
        [SerializeField] List<Emotion> availableEmotions = new List<Emotion>();
        [SerializeField] GameObject selectedEmotion;
        [SerializeField] float emotionReleaseRange = .5f;
        [SerializeField] Vector2 dragLimit;

        EmotionStock emotionStock;

        bool isFlying = false;
        bool isBeingLaunched = false;

        public static Action onEmotionsExhausted;

        void Awake()
        {
            currentSpringPivotPoint = GameObject.FindGameObjectWithTag("Pivot");
            emotionStock = GetComponent<EmotionStock>();
        }

        void OnEnable()
        {
            Tower.onTowerDestroyed += HandleTowerDestroyed;
            Emotion.onEmotionDemolish += HandleEmotionDestroyed;
        }

        void Start()
        {
            RespawnEmotion();
        }

        void Update()
        {
            HandleTouchInput();

            if (isBeingLaunched && IsInReleaseRange())
            {
                LaunchEmotion();
            }
        }

        void OnDisable()
        {
            Tower.onTowerDestroyed -= HandleTowerDestroyed;
            Emotion.onEmotionDemolish -= HandleEmotionDestroyed;
        }

        void HandleTouchInput()
        {
            // Prevent player from moving the emotion if it's already flying
            if (isFlying) return;

            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                DragEmotion();
            }
            else
            {
                if (currentEmotion == null) return;
                currentEmotion.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }

        void DragEmotion()
        {
            Rigidbody2D emotionRb = currentEmotion.GetComponent<Rigidbody2D>();
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            Vector2 clampedPosition = ClampDragPosition(targetPosition);
            currentEmotion.transform.position = clampedPosition;

            emotionRb.isKinematic = true;
            emotionRb.velocity = new Vector2(0, 0);
            isBeingLaunched = true;
        }

        Vector2 ClampDragPosition(Vector2 currentDragPosition)
        {
            Vector2 currentSprintPivotPosition = currentSpringPivotPoint.transform.position;
            float dragAreaXPositiveLimit = currentSprintPivotPosition.x + dragLimit.x;
            float dragAreaXNegativeLimit = currentSprintPivotPosition.x - dragLimit.x;
            float dragAreaYPositiveLimit = currentSprintPivotPosition.y + dragLimit.y;
            float dragAreaYNegativeLimit = currentSprintPivotPosition.y - dragLimit.y;

            float clampedDragXPosition = Mathf.Clamp(currentDragPosition.x, dragAreaXNegativeLimit, dragAreaXPositiveLimit);
            float clampedDragYPosition = Mathf.Clamp(currentDragPosition.y, dragAreaYNegativeLimit, dragAreaYPositiveLimit);

            Vector2 clampedPosition = new Vector2(clampedDragXPosition, clampedDragYPosition);

            return clampedPosition;
        }

        bool IsInReleaseRange()
        {
            if (currentEmotion == null || currentSpringPivotPoint == null) return false;

            SpringJoint2D emotionSpringJoint = currentEmotion.GetComponent<SpringJoint2D>();

            if (emotionSpringJoint == null) return false;

            float emotionDistanceFromPivotPoint = Vector2.Distance(currentSpringPivotPoint.transform.position, currentEmotion.transform.position);

            if (emotionDistanceFromPivotPoint <= emotionReleaseRange)
            {
                return true;
            }

            return false;
        }

        void LaunchEmotion()
        {
            SpringJoint2D emotionSpringJoint = currentEmotion.GetComponent<SpringJoint2D>();
            emotionSpringJoint.enabled = false;
            isBeingLaunched = false;
            currentEmotion.StartLifetimeExpiry();
            isFlying = true;
            emotionStock.Consume();
        }

        void RespawnEmotion()
        {
            if (availableEmotions == null) return;
            currentEmotion = Instantiate(availableEmotions[0],
                currentSpringPivotPoint.transform.position,
                Quaternion.identity);

            SpringJoint2D newEmotionSpringJoint = currentEmotion.GetComponent<SpringJoint2D>();
            newEmotionSpringJoint.connectedBody = currentSpringPivotPoint.GetComponent<Rigidbody2D>();
            isFlying = false;
        }

        void HandleTowerDestroyed()
        {
            enabled = false;
        }

        void HandleEmotionDestroyed()
        {
            if (emotionStock.HasEmotionsRemaining())
            {
                RespawnEmotion();
            }
            else
            {
                onEmotionsExhausted?.Invoke();
            }
        }

        void OnDrawGizmos()
        {
            if (currentSpringPivotPoint == null) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(currentSpringPivotPoint.transform.position, dragLimit * 2);
        }
    }
}
