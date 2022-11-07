using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] GameObject currentSpringPivotPoint;
    [SerializeField] GameObject currentBall;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] float ballLifetime = 5f;
    [SerializeField] float ballReleaseRange = .5f;
    

    bool isFlying = false;
    bool isBeingLaunched = false;

    void Awake()
    {
        currentBall = GameObject.FindGameObjectWithTag("Ball");
        currentSpringPivotPoint = GameObject.FindGameObjectWithTag("Pivot");
    }
    void Update()
    {
        HandleTouchInput();

        if (isBeingLaunched && IsInReleaseRange())
        {
            LaunchBall();
        }
    }

    void HandleTouchInput()
    {
        // Prevent player from moving the ball if it's already flying
        if (isFlying) return;

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            DragBall();
        }
        else
        {
            currentBall.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    void DragBall()
    {
        Rigidbody2D ballRb = currentBall.GetComponent<Rigidbody2D>();
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        currentBall.transform.position = targetPosition;

        ballRb.isKinematic = true;
        ballRb.velocity = new Vector2(0, 0);
        isBeingLaunched = true;
    }

    bool IsInReleaseRange()
    {
        SpringJoint2D ballSpringJoint = currentBall.GetComponent<SpringJoint2D>();
        if (ballSpringJoint == null || currentSpringPivotPoint == null) return false;

        float ballDistanceFromPivotPoint = Vector2.Distance(currentSpringPivotPoint.transform.position, currentBall.transform.position);
        
        if (ballDistanceFromPivotPoint <= ballReleaseRange)
        {
            return true;
        }

        return false;
    }

    void LaunchBall()
    {
        SpringJoint2D ballSpringJoint = currentBall.GetComponent<SpringJoint2D>();
        ballSpringJoint.enabled = false;
        isBeingLaunched = false;
        isFlying = true;
        StartCoroutine(DestroyBallWhenLifetimeExpires());
    }
    IEnumerator DestroyBallWhenLifetimeExpires()
    {
        while (isFlying)
        {
            yield return new WaitForSeconds(ballLifetime);
            Destroy(currentBall);
            isFlying = false;
        }
        RespawnBall();
    }

    void RespawnBall()
    {
        if (ballPrefab == null) return;
        currentBall = Instantiate(ballPrefab,
            currentSpringPivotPoint.transform.position,
            Quaternion.identity,
            transform.parent);

        SpringJoint2D newBallSpringJoint = currentBall.GetComponent<SpringJoint2D>();
        newBallSpringJoint.connectedBody = currentSpringPivotPoint.GetComponent<Rigidbody2D>();
    }
}
