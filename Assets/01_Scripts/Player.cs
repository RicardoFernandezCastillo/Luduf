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
    public float health = 5f;
    public float maxHealth = 5f;

    public int level = 1;
    public float exp = 0f;
    public float maxExp = 10f;
    [Header("Referencias")]
    public Rigidbody rb;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Slider healthSlider;
    public Slider easeHealthSlider;

    public Slider lvlSlider;
    public Slider easeLvlSlider;

    public TextMeshProUGUI lvlText;

    private float lerpSpeed = 0.03f;


    public int cuantityDashes = 2;
    public float timeToRechargeDash = 5f;
    private float timerDash = 0f;


    public int total_Ammo = 70;
    public int magazineSize = 15;
    public int currentAmmo = 15;
    public float timeToReload = 3f;
    private float timerReload = 0f;



    NewInputSystem inputActions;//--
    Vector2 dir = Vector2.zero;


    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        inputActions = new NewInputSystem();//--
        inputActions.Player.Movement.performed += ctx => dir = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => dir = Vector2.zero;
		inputActions.Player.Shoot.performed += ctx => Shoot(); //Se ejecuta cada ves que dispare

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmmo <= 0)
        {
            timerReload += Time.deltaTime;

            if (timerReload >= timeToReload)
            {
                Reload();
            }
        }

        Movement();
        Aim();
        Pruebitas();


        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed);
        }
        if (lvlSlider.value != easeLvlSlider.value)
        {
            lvlSlider.value = Mathf.Lerp(lvlSlider.value,  easeLvlSlider.value, lerpSpeed);
        }
    }

    public void HealthCheck()
    {
        if (health<=0)
        {
            healthSlider.value = 0;
        }
        else if (healthSlider.value != health / maxHealth)
        {
            healthSlider.value = health / maxHealth;
        }
    }
    public void XpCheck()
    {
        //if (lvlSlider.value != exp / maxExp)
        //{
        //    lvlSlider.value = exp / maxExp;
        //}

        if (easeLvlSlider.value != exp / maxExp)
        {
            easeLvlSlider.value = exp / maxExp;
        }
    }

	void Movement()
	{
		// Mueve al jugador en la dirección del joystick
		rb.velocity = new Vector3(dir.x * speed, 0, dir.y * speed);

		// Si hay movimiento, rota al jugador hacia la dirección del joystick
		if (dir != Vector2.zero)
		{
			float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, angle, 0);
		}

		// Dash hacia la dirección del movimiento cuando se presiona la tecla Tab
		if (Input.GetKeyDown(KeyCode.Tab) && cuantityDashes > 0)
		{
			rb.AddForce(rb.velocity * 60, ForceMode.Impulse);
			cuantityDashes--;
		}
		else
		{
			timerDash += Time.deltaTime;
			if (timerDash >= timeToRechargeDash)
			{
				if (cuantityDashes < 2)
				{
					cuantityDashes++;
				}
				timerDash = 0;
			}
		}
	}
    void Shoot()
    {
        if (currentAmmo > 0)
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            currentAmmo--;

            //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //bullet.GetComponent<Bullet>().playerBullet = true;
        }
    }

    void Reload()
    {
        if (total_Ammo >= magazineSize)
        {
            currentAmmo = magazineSize;
            total_Ammo -= magazineSize;
        }
        else
        {
            currentAmmo = total_Ammo;
            total_Ammo = 0;
        }
        timerReload = 0;
    }



    void Pruebitas()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1f);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            IncreaseXp(2f);
            Debug.Log($"CurrentXp: {exp} --- {level}");
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

        health -= damage;
        HealthCheck();
        if (health <= 0)
        {
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
