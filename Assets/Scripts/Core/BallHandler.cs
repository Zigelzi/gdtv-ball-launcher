using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    GameObject ball;

    void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }
    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            ball.transform.position = targetPosition;
        }
    }
}
