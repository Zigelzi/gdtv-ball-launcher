using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using DD.Environment;


namespace DD.Core
{
    public class EmotionHandler : MonoBehaviour
    {
        [SerializeField] GameObject _currentSpringPivotPoint;
        [SerializeField] Emotion _currentEmotion;
        [SerializeField] List<Emotion> _availableEmotions = new List<Emotion>();
        [SerializeField] Emotion _selectedEmotion;
        [SerializeField] float _emotionReleaseRange = .5f;
        [SerializeField] Vector2 _dragLimit;

        EmotionStock _emotionStock;

        bool _isDragged = false;
        bool _isFlying = false;
        bool _isBeingLaunched = false;

        public static Action onEmotionsExhausted;

        void Awake()
        {
            _currentSpringPivotPoint = GameObject.FindGameObjectWithTag("Pivot");
            _emotionStock = GetComponent<EmotionStock>();
            _selectedEmotion = _availableEmotions[0];
        }

        void OnEnable()
        {
            Tower.onTowerDestroyed += HandleTowerDestroyed;
            Emotion.onEmotionDemolish += HandleEmotionDestroyed;
        }

        void Start()
        {
            RespawnEmotion(_selectedEmotion);
        }

        void Update()
        {
            HandleTouchInput();

            if (_isBeingLaunched && IsInReleaseRange())
            {
                LaunchEmotion();
            }
        }

        void OnDisable()
        {
            Tower.onTowerDestroyed -= HandleTowerDestroyed;
            Emotion.onEmotionDemolish -= HandleEmotionDestroyed;
        }

        public void NextEmotion()
        {
            int currentEmotionIndex = _availableEmotions.IndexOf(_selectedEmotion);
            int nextEmotionIndex = currentEmotionIndex + 1;
            if (nextEmotionIndex >= _availableEmotions.Count)
            {
                _selectedEmotion = _availableEmotions[0];
            }
            else
            {
                _selectedEmotion = _availableEmotions[nextEmotionIndex];
            }

            SwapEmotion(_selectedEmotion);
        }

        public void PreviousEmotion()
        {
            int currentEmotionIndex = _availableEmotions.IndexOf(_selectedEmotion);
            int previousEmotionIndex = currentEmotionIndex - 1;
            if (previousEmotionIndex < 0)
            {
                _selectedEmotion = _availableEmotions[_availableEmotions.Count - 1];
            }
            else
            {
                _selectedEmotion = _availableEmotions[previousEmotionIndex];
            }

            SwapEmotion(_selectedEmotion);
        }

        void SwapEmotion(Emotion newEmotion)
        {
            if (_availableEmotions == null || 
                _availableEmotions.Count == 0 ||
                _isFlying ||
                _currentEmotion == null) return;

            Destroy(_currentEmotion.gameObject);
            RespawnEmotion(newEmotion);
        }

        void HandleTouchInput()
        {
            // Prevent player from moving the emotion if it's already flying
            if (_isFlying || IsTouchingUI()) return;

            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                DragEmotion();
            }
            else
            {
                if (_currentEmotion == null) return;
                _currentEmotion.GetComponent<Rigidbody2D>().isKinematic = false;
                _isDragged = false;
            }
        }

        bool IsTouchingUI()
        {
            if (Touchscreen.current.primaryTouch.press.isPressed && 
                EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            return false;
        }

        void DragEmotion()
        {
            Rigidbody2D emotionRb = _currentEmotion.GetComponent<Rigidbody2D>();
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            Vector2 clampedPosition = ClampDragPosition(targetPosition);

            _currentEmotion.transform.position = clampedPosition;

            emotionRb.isKinematic = true;
            emotionRb.velocity = new Vector2(0, 0);

            _isDragged = true;
            _isBeingLaunched = true;
        }

        Vector2 ClampDragPosition(Vector2 currentDragPosition)
        {
            Vector2 currentSprintPivotPosition = _currentSpringPivotPoint.transform.position;
            float dragAreaXPositiveLimit = currentSprintPivotPosition.x + _dragLimit.x;
            float dragAreaXNegativeLimit = currentSprintPivotPosition.x - _dragLimit.x;
            float dragAreaYPositiveLimit = currentSprintPivotPosition.y + _dragLimit.y;
            float dragAreaYNegativeLimit = currentSprintPivotPosition.y - _dragLimit.y;

            float clampedDragXPosition = Mathf.Clamp(currentDragPosition.x, dragAreaXNegativeLimit, dragAreaXPositiveLimit);
            float clampedDragYPosition = Mathf.Clamp(currentDragPosition.y, dragAreaYNegativeLimit, dragAreaYPositiveLimit);

            Vector2 clampedPosition = new Vector2(clampedDragXPosition, clampedDragYPosition);

            return clampedPosition;
        }

        bool IsInReleaseRange()
        {
            if (_currentEmotion == null || 
                _currentSpringPivotPoint == null ||
                _isDragged) return false;

            SpringJoint2D emotionSpringJoint = _currentEmotion.GetComponent<SpringJoint2D>();

            if (emotionSpringJoint == null) return false;

            float emotionDistanceFromPivotPoint = Vector2.Distance(_currentSpringPivotPoint.transform.position, _currentEmotion.transform.position);

            if (emotionDistanceFromPivotPoint <= _emotionReleaseRange)
            {
                return true;
            }

            return false;
        }

        void LaunchEmotion()
        {
            SpringJoint2D emotionSpringJoint = _currentEmotion.GetComponent<SpringJoint2D>();

            emotionSpringJoint.enabled = false;
            _isBeingLaunched = false;
            _isFlying = true;

            _currentEmotion.StartLifetimeExpiry();
            _emotionStock.Consume();
        }

        void RespawnEmotion(Emotion emotion)
        {
            SpringJoint2D newEmotionSpringJoint;
            int spawnedEmotionIndex;

            if (_availableEmotions == null || _availableEmotions.Count == 0) return;

            spawnedEmotionIndex = _availableEmotions.IndexOf(emotion);

            _currentEmotion = Instantiate(_availableEmotions[spawnedEmotionIndex],
                _currentSpringPivotPoint.transform.position,
                Quaternion.identity);

            newEmotionSpringJoint = _currentEmotion.GetComponent<SpringJoint2D>();
            newEmotionSpringJoint.connectedBody = _currentSpringPivotPoint.GetComponent<Rigidbody2D>();
            _isFlying = false;
        }

        void HandleTowerDestroyed()
        {
            enabled = false;
        }

        void HandleEmotionDestroyed()
        {
            if (_emotionStock.HasEmotionsRemaining())
            {
                RespawnEmotion(_selectedEmotion);
            }
            else
            {
                onEmotionsExhausted?.Invoke();
            }
        }

        void OnDrawGizmos()
        {
            if (_currentSpringPivotPoint == null) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_currentSpringPivotPoint.transform.position, _dragLimit * 2);
        }
    }
}
