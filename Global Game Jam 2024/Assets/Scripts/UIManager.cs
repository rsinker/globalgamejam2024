using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private string m_MainScene;
    
    //private static UIManager Instance; //{ get; private set; }
    private void Awake()
    {
        // DontDestroyOnLoad(this);

        // if (Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Destroy(this.gameObject);
        // }


    }

    public void playGame () {

        SceneManager.LoadScene(m_MainScene);

    }

    public void quitGame () {

        Application.Quit();
    }

}
