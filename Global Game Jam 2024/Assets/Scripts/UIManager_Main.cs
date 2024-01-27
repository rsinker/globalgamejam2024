using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private string sceneName;


    public void Pause() {

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

    }

    public void Resume() {

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

    }

    public void Home() {

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);

    }

}
