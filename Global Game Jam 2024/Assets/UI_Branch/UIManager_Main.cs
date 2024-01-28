using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager_Main : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject pauseMenu;

    private GameManager1 GM;


    void Start() {
        GM = GameManager1.Instance;
    }

    //Access Health
    //GM.playerMaxHealth

    //Pause Menu
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
    public Sprite fullHeart, halfHeart, emptyHeart;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }

        //empty heart sprites vs full heart sprites
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < GM.playerCurrentHealth) {

                if ((i+1) % 2 == 0) {

                    Hearts[i].sprite = fullHeart;

                }  else {

                    Hearts[i].sprite = halfHeart;

                }

            } else {
                
                Hearts[i].sprite = emptyHeart;

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

