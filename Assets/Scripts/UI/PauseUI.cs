using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private InputActionReference pauseAction;

    private bool isPaused = false;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver && pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            isPaused = false;
        }
    }


    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
        pauseAction.action.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
