using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmotionHandler : MonoBehaviour
{
    [SerializeField] GameObject currentSpringPivotPoint;
    [SerializeField] Emotion currentEmotion;
    [SerializeField] List<Emotion> availableEmotions = new List<Emotion>();
    [SerializeField] GameObject selectedEmotion;
    [SerializeField] float emotionLifetime = 5f;
    [SerializeField] float emotionReleaseRange = .5f;
    

    bool isFlying = false;
    bool isBeingLaunched = false;

    void Awake()
    {
        currentSpringPivotPoint = GameObject.FindGameObjectWithTag("Pivot");
    }

    void OnEnable()
    {
        Tower.onTowerDestroyed += HandleTowerDestroyed;
        Emotion.onEmotionDestroy += HandleEmotionDestroyed;
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
        Emotion.onEmotionDestroy -= HandleEmotionDestroyed;
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
        currentEmotion.transform.position = targetPosition;

        emotionRb.isKinematic = true;
        emotionRb.velocity = new Vector2(0, 0);
        isBeingLaunched = true;
    }

    bool IsInReleaseRange()
    {
        if (currentEmotion == null || currentSpringPivotPoint == null) return false;

        SpringJoint2D emotionSpringJoint = currentEmotion.GetComponent<SpringJoint2D>();

        if (emotionSpringJoint == null ) return false;

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
        currentEmotion.DestroyEmotion();
        isFlying = true;
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
        RespawnEmotion();
    }
}
