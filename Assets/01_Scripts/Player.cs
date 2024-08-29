using System;
using System.Collections;
using System.Collections.Generic;
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

    private float lerpSpeed = 0.03f;


    public int cuantityDashes = 2;
    public float timeToRechargeDash = 5f;
    private float timerDash = 0f;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shoot();
        Aim();


        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed);
        }
        if (lvlSlider.value != easeLvlSlider.value)
        {
            lvlSlider.value = Mathf.Lerp(lvlSlider.value,  easeLvlSlider.value, lerpSpeed);
        }
    }

    private void HealthCheck()
    {
        // healthSlider.value es un valor entre 0 y 1
        // cambiar el valor si los valores no coinciden
        if (health<=0)
        {
            healthSlider.value = 0;
        }
        else if (healthSlider.value != health / maxHealth)
        {
            healthSlider.value = health / maxHealth;
        }

        //if (healthSlider.value != easeHealthSlider.value)
        //{
        //    // Animar el slider de vida
        //    // Mathf.Lerp(valorInicial, valorFinal, velocidad)
        //    easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed * Time.deltaTime);
        //}

    }
    private void XpCheck()
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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(x, 0, y) * speed;

        //Normalizar la velocidad para que el jugador no se mueva más rápido en diagonal
        //if (rb.velocity.magnitude > 1)
        //{
        //    rb.velocity = rb.velocity.normalized;
        //}

        // Agregar el boton tab para que el jugador haga un dash hacia la dirección en la que se esta moviendo
        if (Input.GetKeyDown(KeyCode.Tab) && cuantityDashes>0)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }

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
        // Busca el enemigo más cercano y menos vida
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
        }
    }
    public void IncreaseXp(float xpToIncrease)
    {
        exp += xpToIncrease;
        //XpCheck();
        if (exp > maxExp)
        {
            level++;
            exp = exp - maxExp;
        }
        XpCheck();
    }



}
