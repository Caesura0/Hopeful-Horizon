using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{

    bool isPaused = false;
    [SerializeField] private GameObject pauseMenuUI;

    private void Start()
    {
        PlayerInput.Instance.OnMenuAction += Instance_OnMenuAction;
        pauseMenuUI.SetActive(false);
        isPaused = gameObject.activeSelf;
    }

    private void Instance_OnMenuAction(object sender, System.EventArgs e)
    {
        PauseUnpause();
    }

    public void PauseUnpause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

}
