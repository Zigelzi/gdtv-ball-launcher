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

        MoodAdjuster _moodAdjuster;

        void Awake()
        {
            _victoryCanvas = GameObject.FindGameObjectWithTag("UI_Victory").GetComponent<Canvas>();
            _defeatedCanvas = GameObject.FindGameObjectWithTag("UI_Defeat").GetComponent<Canvas>();

            _victoryCanvas.enabled = false;
            _defeatedCanvas.enabled = false;

            _moodAdjuster = FindObjectOfType<MoodAdjuster>();
        }
        void OnEnable()
        {
            _moodAdjuster.onHappyMood.AddListener(HandleVictory);
        }

        void OnDisable()
        {
            _moodAdjuster.onHappyMood.RemoveListener(HandleVictory);
        }

        void HandleVictory()
        {
            _victoryCanvas.enabled = true;
        }

    }

}