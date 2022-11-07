using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    Canvas menuCanvas;
    void Awake()
    {
        menuCanvas = GetComponent<Canvas>();
        menuCanvas.enabled = false;
    }
    void OnEnable()
    {
        Tower.onTowerDestroyed += HandleTowerDestroyed;
    }

    void OnDisable()
    {
        Tower.onTowerDestroyed -= HandleTowerDestroyed;
    }
    public void RestartLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
        Debug.Log("Button clicked");
    }

    public void Exit()
    {
        Application.Quit();
    }

    void HandleTowerDestroyed()
    {
        menuCanvas.enabled = true;
    }
}
