using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [SerializeField] private string m_MainScene;

    public UnityEngine.Video.VideoPlayer videoPlayer;
    
    //private static UIManager Instance; //{ get; private set; }
    private void Start()
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

        //videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();

        //videoPlayer.Pause();

        //videoPlayer.isLooping = false;


    }

    public void playGame () {

        //videoPlayer.Play();
        SceneManager.LoadScene(m_MainScene);

    }

    public void quitGame () {

        Application.Quit();

    }

    void update(){

        if(Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(m_MainScene);
        }

    }

}
