using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float maxSpeed = 30f;

    Rigidbody2D ballRb;
    void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        LimitVelocity();
    }

    void LimitVelocity()
    {
        float maxSpeedX = Mathf.Clamp(ballRb.velocity.x, -maxSpeed, maxSpeed);
        float maxSpeedY = Mathf.Clamp(ballRb.velocity.y, -maxSpeed, maxSpeed);
        ballRb.velocity = new Vector2(maxSpeedX, maxSpeedY);
    }
}
