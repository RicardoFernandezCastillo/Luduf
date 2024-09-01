using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPref : MonoBehaviour
{
    [SerializeField] private Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>(); // Busca el objeto con el script Player y lo asigna a la variable player
        //player.health = PlayerPrefs.GetFloat("health", player.health);
        //player.maxHealth = PlayerPrefs.GetFloat("maxHealth", player.maxHealth);
        //player.level = PlayerPrefs.GetInt("level", player.level);
        //player.exp = PlayerPrefs.GetFloat("exp", player.exp);
        //player.maxExp = PlayerPrefs.GetFloat("maxExp", player.maxExp);
        //player.cuantityDashes = PlayerPrefs.GetInt("cuantityDashes", player.cuantityDashes);
        //player.timeToRechargeDash = PlayerPrefs.GetFloat("timeToRechargeDash", player.timeToRechargeDash);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Save();
        Load();
    }
    void Save()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetFloat("health", player.health);
            PlayerPrefs.SetFloat("maxHealth", player.maxHealth);
            PlayerPrefs.SetInt("level", player.level);
            PlayerPrefs.SetFloat("exp", player.exp);
            PlayerPrefs.SetFloat("maxExp", player.maxExp);
            PlayerPrefs.SetInt("cuantityDashes", player.cuantityDashes);
            PlayerPrefs.SetFloat("timeToRechargeDash", player.timeToRechargeDash);
            PlayerPrefs.SetInt("totalAmmo", player.total_Ammo);
            
        }
    }
    void Load()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            player.health = PlayerPrefs.GetFloat("health", player.health);
            player.maxHealth = PlayerPrefs.GetFloat("maxHealth", player.maxHealth);
            player.level = PlayerPrefs.GetInt("level", player.level);
            player.exp = PlayerPrefs.GetFloat("exp", player.exp);
            player.maxExp = PlayerPrefs.GetFloat("maxExp", player.maxExp);
            player.cuantityDashes = PlayerPrefs.GetInt("cuantityDashes", player.cuantityDashes);
            player.timeToRechargeDash = PlayerPrefs.GetFloat("timeToRechargeDash", player.timeToRechargeDash);
            player.total_Ammo = PlayerPrefs.GetInt("totalAmmo", player.total_Ammo);
            
            player.HealthCheck();
            player.XpCheck();
        }
    }
    //private void OnLevelWasLoaded(int level)
    //{

    //}
}
