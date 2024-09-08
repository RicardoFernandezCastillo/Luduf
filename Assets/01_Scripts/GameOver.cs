using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip GameOverMusic;
    void Start()
    {
        if (GameOverMusic != null)
        {
            AudioManager.instance.SetMusic(GameOverMusic);
        }
    }

    // Update is called once per frame

    public void Continue()
    {
        Debug.Log("Continue");
        SceneManager.LoadScene("PlayerScene");
    }

    public void GoMenu()
    {

        Debug.Log("GoMenu");
       SceneManager.LoadScene("MainMenu");
    }
}
