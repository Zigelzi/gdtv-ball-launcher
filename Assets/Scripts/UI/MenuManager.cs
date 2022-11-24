using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DD.Core;
using DD.Environment;

namespace DD.UI
{

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] Canvas victoryCanvas;
        [SerializeField] Canvas defeatedCanvas;

        bool _isVictorious = false;

        void Awake()
        {
            victoryCanvas = GameObject.FindGameObjectWithTag("UI_Victory").GetComponent<Canvas>();
            defeatedCanvas = GameObject.FindGameObjectWithTag("UI_Defeat").GetComponent<Canvas>();

            victoryCanvas.enabled = false;
            defeatedCanvas.enabled = false;
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
            victoryCanvas.enabled = true;
            _isVictorious = true;
        }

        void HandleGracePeriodEnd()
        {
            if (!_isVictorious)
            {
                defeatedCanvas.enabled = true;
            }
        }
    }

}