using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI_Main : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private string sceneName;

    public GameManager1 GM;

    void Awake() {
        GM = GameManager1.Instance;
    }

    //Access Health
    //GM.playerMaxHealth

    public void Pause() {

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

    }

    public void Resume() {

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

    }
    //hhh
    public void Home() {

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);

    }

    //hearts UI
    public Image[] Hearts;
    public Sprite fullHeartL;
    public Sprite fullHeartR;
    public Sprite emptyHeartL;
    public Sprite emptyHeartR;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
        //empty heart sprites vs full heart sprites
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < GM.playerCurrentHealth) {
                if (i % 2 > 0) {
                    Hearts[i].sprite = fullHeartR;
                } else {
                    Hearts[i].sprite = fullHeartL;
                }
            } else {
                if (i % 2 > 0) {
                    Hearts[i].sprite = emptyHeartR;
                } else {
                    Hearts[i].sprite = emptyHeartL;
                }
            }
            if (i < GM.playerMaxHealth)
            {
                Hearts[i].enabled = true;
            } else {
                Hearts[i].enabled = false;
            }
        }
    }
}

