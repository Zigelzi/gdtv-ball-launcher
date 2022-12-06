using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DD.Core;
using DD.Environment;
using DD.Mood;

namespace DD.UI
{

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] Canvas _victoryCanvas;
        [SerializeField] Canvas _defeatedCanvas;

        bool _isVictorious = false;

        DefeatManager _defeatManager;
        MoodAdjuster _moodAdjuster;

        void Awake()
        {
            _victoryCanvas = GameObject.FindGameObjectWithTag("UI_Victory").GetComponent<Canvas>();
            _defeatedCanvas = GameObject.FindGameObjectWithTag("UI_Defeat").GetComponent<Canvas>();

            _victoryCanvas.enabled = false;
            _defeatedCanvas.enabled = false;

            _defeatManager = FindObjectOfType<DefeatManager>();
            _moodAdjuster = FindObjectOfType<MoodAdjuster>();
        }
        void OnEnable()
        {
            Tower.onTowerDestroyed += HandleVictory;
            _defeatManager.onGracePeriodEnd.AddListener(HandleGracePeriodEnd);
            _moodAdjuster.onHappyMood.AddListener(HandleVictory);
        }

        void OnDisable()
        {
            Tower.onTowerDestroyed -= HandleVictory;
            _defeatManager.onGracePeriodEnd.RemoveListener(HandleGracePeriodEnd);
            _moodAdjuster.onHappyMood.RemoveListener(HandleVictory);
        }

        void HandleVictory()
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