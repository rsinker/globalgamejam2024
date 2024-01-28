using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossScript : ProjectileEnemy
{
    public float endScreenDelay = 3.0f;
    public override void Die()
    {
        base.Die();

        StartCoroutine(EndScreen());
        
    }

    IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(endScreenDelay);
        SceneManager.LoadScene("Win Scene");
    }
}
