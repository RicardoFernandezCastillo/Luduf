using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Stats")]
    public float speed = 4f;
    public float health = 10f;
    public float maxHealth = 10f;

    public int level = 1;
    public float exp = 0f;
    public float maxExp = 10f;


    public int cuantityDashes = 2;
    public float timeToRechargeDash = 5f;
    private float timerDash = 0f;
    private bool isRechargingDash = false;


    public int total_Ammo = 70;
    public int magazineSize = 15;
    public int currentAmmo = 15;
    public float timeToReload = 3f;
    private float timerReload = 0f;
    private bool isReloading = false;

    private bool increaseHealth = false;

    public int mapLevel = 1;

    private float lerpSpeed = 0.03f;
    [Header("Referencias")]
    public Rigidbody rb;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("UI")]
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Slider lvlSlider;
    public Slider easeLvlSlider;
    public Slider reloadSlider;
    public TextMeshProUGUI lvlText;

    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI totalAmmoText;




    [Header("Sounds")]
    public AudioClip rifleShootSound;
    public AudioClip rifleReloadSound;
    public AudioClip powerUpSound;
    public AudioClip playerDeathSound;
    public AudioClip GameOverSound;
    public AudioClip levelUpSound;

    public List<AudioClip> musicBackground;
    //public AudioClip gam;


    public Animator animator;


    NewInputSystem inputActions;//--
    Vector2 dir = Vector2.zero;



    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        inputActions = new NewInputSystem();//--
        inputActions.Player.Movement.performed += ctx => dir = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => dir = Vector2.zero;
		inputActions.Player.Shoot.performed += ctx => Shoot(); //Se ejecuta cada ves que dispare
        inputActions.Player.Reload.performed += ctx => CheckReload(); //Se ejecuta cada ves que dispare
        inputActions.Player.Dash.performed += ctx => DashMovement(); //Se ejecuta cada ves que dispare
	}



    private void OnEnable()
	{
		inputActions.Enable();
	}

	private void OnDisable()
	{
		inputActions.Disable();
	}

	void Start()
    {
        if (PlayerPrefs.GetInt("level") > level || PlayerPrefs.GetFloat("exp") > exp)
        {
            Debug.Log($"Level = {PlayerPrefs.GetInt("level")}  --- {level} Exp = {PlayerPrefs.GetFloat("exp")} ------- {exp}  ");
            PlayerPref.LoadStats();
        }

        currentAmmoText.text = $"{currentAmmo} / {magazineSize}";
        totalAmmoText.text = $"{total_Ammo}";
        lvlText.text = level.ToString();
        UpdateMusicBackgroud();
    }


    void UpdateMusicBackgroud()
    {
        // cada nivel se cambia la música de fondo y si no hay más música se reinicia
        int aux = mapLevel;
        if (aux > musicBackground.Count)
        {
            aux = aux % musicBackground.Count;
        }
        AudioManager.instance.SetMusic(musicBackground[aux]);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmmo <= 0 && !isReloading && total_Ammo >0)
        {
            // Solo inicia la recarga si no estás recargando
            CheckReload(); // Esto reproducirá el sonido
        }
        if (isReloading)
        {

            reloadSlider.value = 0f;
            timerReload += Time.deltaTime;

            float reloadProgress = Mathf.Clamp01(timerReload / timeToReload);
            reloadSlider.value = reloadProgress;
            //reloadSlider.value = Mathf.Lerp(reloadSlider.value, reloadProgress, Time.deltaTime * 3f);

            if (timerReload >= timeToReload)
            {
                reloadSlider.value = 1f;
                Reload();
            }
        }

        Movement();
        Aim();
        Pruebitas();
        RechargeDash();

        UpdateSliders(increaseHealth);
    }

    void UpdateSliders(bool p)
    {
        if (healthSlider.value != easeHealthSlider.value)
        {
            if (!p)
            {
                //healthSlider.value = Mathf.Lerp(healthSlider.value, easeHealthSlider.value, lerpSpeed);
                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed);
            }
            else
            {
                //easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed); k
                healthSlider.value = Mathf.Lerp(healthSlider.value, easeHealthSlider.value, lerpSpeed);

            }
        }
        if (lvlSlider.value != easeLvlSlider.value)
        {
            lvlSlider.value = Mathf.Lerp(lvlSlider.value, easeLvlSlider.value, lerpSpeed);
        }

    }


    private void CheckReload()
    {
        if (currentAmmo != magazineSize && !isReloading && total_Ammo > 0)
        {
            isReloading = true;
            AudioManager.instance.PlaySFX(rifleReloadSound);
            //AudioManager.instance.PlaySFXDelay(rifleReloadSound, timeToReload);
        }
    }
    public void IncreaseHealth(float healthToIncrease)
    {
        if (health + healthToIncrease > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += healthToIncrease;
        }

        HealthCheck(true);
    }
    public void HealthCheck(bool posi)
    {
        // posicion true es para aumentar la vida
        if (health<=0)
        {
            healthSlider.value = 0;
        }
        else
        {
            if(!posi)
            {
                healthSlider.value = health / maxHealth;
                increaseHealth = false;
            }
            else
            {
                easeHealthSlider.value = health / maxHealth;
                increaseHealth = true;
            }
        }
    }

    public void XpCheck()
    {
        if (easeLvlSlider.value != exp / maxExp)
        {
            easeLvlSlider.value = exp / maxExp;
        }
    }

	void Movement()
	{
		// Mueve al jugador en la dirección del joystick

		rb.velocity = new Vector3(dir.x * speed, 0, dir.y * speed);
        float n = 0;
        if (dir.x != 0)
        {
            n = Mathf.Abs(dir.x);
		}else if (dir.y != 0)
        {
           n = Mathf.Abs(dir.y);
        }
        else
        {
            n = 0 ;
        }


		Debug.Log("Eje x : " + n);
		animator.SetFloat("Speed", n );
		// Si hay movimiento, rota al jugador hacia la dirección del joystick
		if (dir != Vector2.zero)
		{
			float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, angle, 0);
		}
	}

    void DashMovement()
    {
        // Dash hacia la dirección del movimiento cuando se presiona la tecla Tab
        if (cuantityDashes > 0)
        {
            rb.AddForce(rb.velocity * 40, ForceMode.Impulse);
            cuantityDashes--;
            isRechargingDash = true;
        }
    }

    void RechargeDash()
    {
        if (isRechargingDash)
        {
            timerDash += Time.deltaTime;
            if (timerDash >= timeToRechargeDash)
            {
                if (cuantityDashes < 2)
                {
                    cuantityDashes++;
                }
                else
                {
                    isRechargingDash = false;
                }
                timerDash = 0;
            }
        }
    }


    void Shoot()
    {
        if (currentAmmo > 0)
        {
            AudioManager.instance.PlaySFX(rifleShootSound);
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            currentAmmo--;
            currentAmmoText.text = $"{currentAmmo} / {magazineSize}";
            animator.SetTrigger("Shoot");
            //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //bullet.GetComponent<Bullet>().playerBullet = true;
        }
    }


    public void AddShoot(int n)
    {      
       currentAmmo += n;
    }

    void Reload()
    {
        if (total_Ammo >= magazineSize)
        {
            int aux = magazineSize - currentAmmo;
            currentAmmo = magazineSize;
            total_Ammo -= aux;
        }
        else
        {
            currentAmmo = total_Ammo;
            total_Ammo = 0;
        }
        currentAmmoText.text = $"{currentAmmo} / {magazineSize}";
        totalAmmoText.text = $"{total_Ammo}";
        timerReload = 0;
        isReloading = false;
    }



    void Pruebitas()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1f);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            IncreaseXp(9f);
            Debug.Log($"CurrentXp: {exp} --- {level}");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentAmmo != magazineSize)
            {
                isReloading = true;
            }

        }

    }
    

    void Aim()
    {
        List<Enemy> enemies = new List<Enemy>();
        GameObject[] enemiesGo = GameObject.FindGameObjectsWithTag("Enemy"); // Busca todos los objetos con la etiqueta "Enemy" y los guarda en un array
        foreach (GameObject go in enemiesGo) 
        {
            enemies.Add(go.GetComponent<Enemy>());
        }
        // Busca el enemigo m�s cercano y menos vida
        Enemy enemyToAttack = null;
        float minDistance = 15f;
        foreach (Enemy e in enemies)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                enemyToAttack = e;
            }
        }

        if (enemyToAttack != null)
        {
            Vector3 dir = enemyToAttack.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }


    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log("tiene danio :" + health);
        health -= damage;
        HealthCheck(false);
        if (health <= 0)
        {
            
            //Borrar player prefs
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("PlayerScene");
            //PlayerPrefs.SetInt("Level", level);
            //PlayerPrefs.SetFloat("Exp", exp);

        }
    }
    public void IncreaseXp(float xpToIncrease)
    {
        exp += xpToIncrease;
        //XpCheck();
        if (exp > maxExp)
        {
            level++;
            lvlText.text = level.ToString();
            //hacer el text bold
            lvlText.fontStyle = FontStyles.Bold;
            exp = exp - maxExp;
            IncreaseStats();
        }
        XpCheck();
    }
    void IncreaseStats()
    {        
        //incrementar la velocidad del jugador un 1.5%
        speed += 0.015f;

        //incrementar el daño de las balas un 5%
        bulletPrefab.GetComponent<Bullet>().damage += 0.05f; // bulletPrefab es el prefab de la bala

        //incrementar la velocidad de las balas un 2%
        bulletPrefab.GetComponent<Bullet>().speed += 0.02f;
        //incrementar la velocidad de recarga de las balas un 2%
        timeToReload -= 0.02f;

        // cada 5 niveles Incrementar
        if (level % 5 == 0)
        {
            // la cantidad de balas que se pueden cargar en el cargador
            magazineSize += 5;
            
            // aumentar la vida máxima un 5%
            maxHealth += 0.05f;
        }

        //maxHealth += 5;
        //health = maxHealth;
        //maxExp += 10;
        //exp = 0;
        //level++;
        //XpCheck();
        //HealthCheck();
    }



}
