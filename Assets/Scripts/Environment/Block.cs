﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DD.Core;

namespace DD.Environment
{

    public class Block : MonoBehaviour, IDemolishable
    {
        [SerializeField] float _minimumDestroyDistance = .5f;
        [SerializeField] float _distanceFromStart = 0f;
        [SerializeField] float _destroyRotation = 85f;
        
        Rigidbody2D _blockRb;
        Vector3 _startingPosition = new Vector2();

        // TODO: Get rid of static event
        public static event Action<Transform> onBlockDestroyed;

        void Awake()
        {
            _blockRb = GetComponent<Rigidbody2D>();
            _startingPosition = transform.position;
        }


        void Update()
        {
            _distanceFromStart = Vector2.Distance(_startingPosition, transform.position);

            if (_distanceFromStart >= _minimumDestroyDistance
                && Mathf.Approximately(_blockRb.velocity.magnitude, 0)
                && Mathf.Abs(_blockRb.rotation) >= _destroyRotation
                )
            {
                Demolish(gameObject);
            }
        }

        public void Demolish(GameObject source)
        {
            transform.gameObject.SetActive(false);
            onBlockDestroyed?.Invoke(transform);
        }
    }
}