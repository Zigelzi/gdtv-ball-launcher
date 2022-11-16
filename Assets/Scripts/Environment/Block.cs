using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Environment
{

    public class Block : MonoBehaviour
    {
        [SerializeField] float minimumDestroyDistance = .5f;
        [SerializeField] float distanceFromStart = 0f;
        [SerializeField] float destroyRotation = 85f;
        Rigidbody2D blockRb;

        Vector3 startingPosition = new Vector2();

        public static event Action<Transform> onBlockDestroyed;

        void Awake()
        {
            blockRb = GetComponent<Rigidbody2D>();
            startingPosition = transform.position;
        }


        void Update()
        {
            distanceFromStart = Vector2.Distance(startingPosition, transform.position);

            if (distanceFromStart >= minimumDestroyDistance
                && Mathf.Approximately(blockRb.velocity.magnitude, 0)
                && Mathf.Abs(blockRb.rotation) >= destroyRotation
                )
            {
                transform.gameObject.SetActive(false);
                onBlockDestroyed?.Invoke(transform);
            }
        }
    }

}