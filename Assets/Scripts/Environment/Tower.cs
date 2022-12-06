using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Environment
{

    public class Tower : MonoBehaviour
    {
        [SerializeField] List<Transform> _blocks = new List<Transform>();

        // TODO: Get rid of static event
        public static event Action onTowerDestroyed;

        void Awake()
        {
            foreach (Transform block in transform)
            {
                _blocks.Add(block);
            }
        }

        void OnEnable()
        {
            Block.onBlockDestroyed += HandleBlockDestroyed;
        }

        void OnDisable()
        {
            Block.onBlockDestroyed -= HandleBlockDestroyed;
        }

        public void RemoveBlock(Transform block)
        {
            _blocks.Remove(block);
        }

        void HandleBlockDestroyed(Transform block)
        {
            _blocks.Remove(block);
            if (_blocks.Count == 0)
            {
                DestroyTower();
            }
        }

        void DestroyTower()
        {
            onTowerDestroyed?.Invoke();
        }
    }

}