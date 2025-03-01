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
            float h_aux = player.health;
            player.health = PlayerPrefs.GetFloat("health", player.health);
            player.maxHealth = PlayerPrefs.GetFloat("maxHealth", player.maxHealth);
            player.level = PlayerPrefs.GetInt("level", player.level);
            player.exp = PlayerPrefs.GetFloat("exp", player.exp);
            player.maxExp = PlayerPrefs.GetFloat("maxExp", player.maxExp);
            player.cuantityDashes = PlayerPrefs.GetInt("cuantityDashes", player.cuantityDashes);
            player.timeToRechargeDash = PlayerPrefs.GetFloat("timeToRechargeDash", player.timeToRechargeDash);
            player.total_Ammo = PlayerPrefs.GetInt("totalAmmo", player.total_Ammo);

            if (h_aux > PlayerPrefs.GetFloat("health", player.health))
            {
                player.HealthCheck(false);
                Debug.Log("Cargando Disminuir vida");
            }
            else
            {
                player.HealthCheck(true);
                Debug.Log("Cargando Aumentar vida");
            }

            player.XpCheck();
        }
    }
    public static void LoadStats()
    {
        Player player = FindObjectOfType<Player>();
        float h_aux = player.health;
        player.health = PlayerPrefs.GetFloat("health", player.health);
        player.maxHealth = PlayerPrefs.GetFloat("maxHealth", player.maxHealth);
        player.level = PlayerPrefs.GetInt("level", player.level);
        player.exp = PlayerPrefs.GetFloat("exp", player.exp);
        player.maxExp = PlayerPrefs.GetFloat("maxExp", player.maxExp);
        player.cuantityDashes = PlayerPrefs.GetInt("cuantityDashes", player.cuantityDashes);
        player.timeToRechargeDash = PlayerPrefs.GetFloat("timeToRechargeDash", player.timeToRechargeDash);
        player.total_Ammo = PlayerPrefs.GetInt("totalAmmo", player.total_Ammo);
        player.mapLevel = PlayerPrefs.GetInt("mapLevel", player.mapLevel);

        if (h_aux > PlayerPrefs.GetFloat("health", player.health))
        {
            player.HealthCheck(false);
            Debug.Log("Cargando Disminuir vida");
        }
        else
        {
            player.HealthCheck(true);
            Debug.Log("Cargando Aumentar vida");
        }

        player.XpCheck();
    }

    public static void SaveStats()
    {
        Player player = FindObjectOfType<Player>();
        PlayerPrefs.SetFloat("health", player.health);
        PlayerPrefs.SetFloat("maxHealth", player.maxHealth);
        PlayerPrefs.SetInt("level", player.level);
        PlayerPrefs.SetFloat("exp", player.exp);
        PlayerPrefs.SetFloat("maxExp", player.maxExp);
        PlayerPrefs.SetInt("cuantityDashes", player.cuantityDashes);
        PlayerPrefs.SetFloat("timeToRechargeDash", player.timeToRechargeDash);
        PlayerPrefs.SetInt("totalAmmo", player.total_Ammo);
        PlayerPrefs.SetInt("mapLevel", player.mapLevel);
    }
    //private void OnLevelWasLoaded(int level)
    //{

    //}
}
