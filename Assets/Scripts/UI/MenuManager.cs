using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DD.Core;
using DD.Environment;

namespace DD.UI
{

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] Canvas _victoryCanvas;
        [SerializeField] Canvas _defeatedCanvas;

        bool _isVictorious = false;

        void Awake()
        {
            _victoryCanvas = GameObject.FindGameObjectWithTag("UI_Victory").GetComponent<Canvas>();
            _defeatedCanvas = GameObject.FindGameObjectWithTag("UI_Defeat").GetComponent<Canvas>();

            _victoryCanvas.enabled = false;
            _defeatedCanvas.enabled = false;
        }
        void OnEnable()
        {
            Tower.onTowerDestroyed += HandleTowerDestroyed;
            DefeatManager.onGracePeriodEnd += HandleGracePeriodEnd;
        }

        void OnDisable()
        {
            Tower.onTowerDestroyed -= HandleTowerDestroyed;
            DefeatManager.onGracePeriodEnd -= HandleGracePeriodEnd;
        }

        void HandleTowerDestroyed()
        {
            _victoryCanvas.enabled = true;
            _isVictorious = true;
        }

        void HandleGracePeriodEnd()
        {
            if (!_isVictorious)
            {
                _defeatedCanvas.enabled = true;
            }
        }
    }

}