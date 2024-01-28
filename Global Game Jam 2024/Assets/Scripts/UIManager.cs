using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [SerializeField] private string m_MainScene;

    [SerializeField] private GameObject cutSceneObject;
    public UnityEngine.Video.VideoPlayer videoPlayer;
    private bool checkForVidOver = false;
    
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

        videoPlayer.Pause();

        videoPlayer.isLooping = false;


    }

    private void Update()
    {
        if (checkForVidOver == true)
        {
            if (videoPlayer.isPlaying == false)
            {
                SceneManager.LoadScene(m_MainScene);
                checkForVidOver = false;
            }
        }
    }

    public void playGame () {

        cutSceneObject.SetActive(true);
        videoPlayer.Play();
        checkForVidOver = true;
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
