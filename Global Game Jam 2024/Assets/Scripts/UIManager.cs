using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void playGame () {

        //SceneManager.LoadScene(1);
        SceneManager.LoadScene(sceneName);

    }

    public void quitGame () {

        Application.Quit();

    }

    //Whos up playing with their Thaaaangg


}
