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

        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();

        videoPlayer.Stop();

        videoPlayer.isLooping = false;

        videoPlayer.loopPointReached += checkCutscene;

        if(Input.GetKeyDown(KeyCode.Space)) {
            videoPlayer.Stop();
            SceneManager.LoadScene(m_MainScene);
        }

    }

    public void playCutscene () {

        videoPlayer.Play();

        // if (videoPlayer.isPlaying == false) {

        //     SceneManager.LoadScene(m_MainScene);

        // }

    }
    
    
    public void playGame () {

        videoPlayer.Stop();
        SceneManager.LoadScene(m_MainScene);

    }

    public void quitGame () {

        Application.Quit();

    }

    void checkCutscene (UnityEngine.Video.VideoPlayer videoPlayer) {

        SceneManager.LoadScene(m_MainScene);

    }
    
    
    void update(){

        //videoPlayer.loopPointReached += checkCutscene;
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            videoPlayer.Stop();
            SceneManager.LoadScene(m_MainScene);
        }

    }

}
