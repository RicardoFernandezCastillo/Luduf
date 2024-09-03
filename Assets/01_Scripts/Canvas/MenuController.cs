using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Level to load")]
    public string _newGameLevel;
    private string _loadGameLevel;

    [Header("Music / Sounds")]
    public AudioClip BackgroundMusic;
    public AudioClip MenuSound;
    public AudioClip MenuSound2;

    void Start()
    {
        if (BackgroundMusic != null)
        {
            AudioManager.instance.SetMusic(BackgroundMusic);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeSound()
    {
        AudioManager.instance.PlaySFX(MenuSound);
    }
    public void MakeSound2()
    {
        AudioManager.instance.PlaySFX(MenuSound2);
    }

    public void NewGameDialogYes()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYex()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void LoadMainMenu()
    { 
       Time.timeScale = 1;
       SceneManager.LoadScene("MainMenu");
       
    }

    public void PauseGame()
    {

       Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
