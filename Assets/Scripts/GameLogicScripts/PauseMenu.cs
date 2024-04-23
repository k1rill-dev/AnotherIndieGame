using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private bool _isPaused;

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    public void ActivateMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _pauseMenuUI.SetActive(false);
    }

    public void ExitMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Debug.Log("выход из игрушки");
        Application.Quit();
    }
}