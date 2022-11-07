using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    [SerializeField] List<Transform> blocks = new List<Transform>();
    
    public static event Action onTowerDestroyed;

    void Awake()
    {
        foreach (Transform block in transform)
        {
            blocks.Add(block);
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
        blocks.Remove(block);
    }

    void HandleBlockDestroyed(Transform block)
    {
        blocks.Remove(block);
        if (blocks.Count == 0)
        {
            onTowerDestroyed?.Invoke();
            DestroyTower();
        }
    }

    void DestroyTower()
    {
        Debug.Log("Player won, tower destroyed!");
    }
}
