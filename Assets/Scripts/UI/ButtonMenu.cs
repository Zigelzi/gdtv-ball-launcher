using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    [SerializeField] bool _nextLevelButton = false;
    int _nextLevelIndex = -1;

    void Awake()
    {
        _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        DisableNextLevelButtonOnLastLevel();
    }
    public void NextLevel()
    {
        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings) return;
        SceneManager.LoadScene(_nextLevelIndex);
    }
    public void RestartLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    void DisableNextLevelButtonOnLastLevel()
    {
        if (_nextLevelButton && _nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            gameObject.SetActive(false);
        }
    }
}
