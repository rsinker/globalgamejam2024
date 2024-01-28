using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathSceneManager : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("Start Menu");
    }
}
