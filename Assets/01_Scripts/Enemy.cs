using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    // Variables públicas configurables
    [Header("Stats")]
    public float speed = 6f;
    public float life = 2f;
    public float maxLife = 2f;
    public float timeToDestroy = 10f;
    public float damage = 1f;
    public float timeBtwAttack = 1f;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;

    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Rigidbody rb;
    public EnemyType enemyType;

    public Slider healthSlider;

    public RoomBehaviour room;

    // Variables privadas
    private float timer = 0f;
    private bool canAttack = false;
    private Transform target;





    void Start()
    {

        EnemiesStats();
        PlayerLocation();
    }

    private void EnemiesStats()
    {
        switch (enemyType)
        {
            case EnemyType.Vampire:
                speed = Random.Range(2f, 4f);
                //canAttack = Random.Range(0, 2) == 0;
                Destroy(gameObject, timeToDestroy);
                break;
            case EnemyType.Wolf:
                speed = Random.Range(4f, 6f);
                //canAttack = Random.Range(0, 2) == 0;
                Destroy(gameObject, timeToDestroy);
                break;
            case EnemyType.Bat:
                speed = Random.Range(2f, 4f);
                //canAttack = Random.Range(0, 2) == 0;
                Destroy(gameObject, timeToDestroy);
                break;
        }
    }

    private void PlayerLocation()
    {
        GameObject targetGo = GameObject.FindGameObjectWithTag("Player");
        if (targetGo != null)
        {
            target = targetGo.transform;
        }
    }

    void Update()
    {
        HealthCheck();
        switch (enemyType)
        {
            case EnemyType.Vampire:
                VampireBehaviour();
                break;
            case EnemyType.Wolf:
                WolfBehaviour();
                break;
            case EnemyType.Bat:
                //SniperBehaviour();
                break;
        }
    }

    private void WolfBehaviour()
    {
        if (target != null)
        {
            //si la distancia entre el enemigo y el jugador es menor a 15 y mayor a 3 el enemigo se mueve hacia el jugador
            if (Vector3.Distance(transform.position, target.position) < 15f && Vector3.Distance(transform.position, target.position) > 3f)
            {

                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);

                if (canAttack && timer >= timeBtwAttack)
                {
                    timer = 0f;
                    Debug.Log("El Lobo Atacó");
                }
                else
                {
                    timer += Time.deltaTime;
                }
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            //si la distancia entre el enemigo y el jugador es menor a 3 el enemigo se queda quieto
            else if (Vector3.Distance(transform.position, target.position) < 3f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void HealthCheck()
    {
        if (life <= 0)
        {
            healthSlider.value = 0;
        }
        else if (healthSlider.value != life / maxLife)
        {
            healthSlider.value = life / maxLife;
        }
    }

    private void VampireBehaviour()
    {
        if (target != null)
        {
            //si la distancia entre el enemigo y el jugador es menor a 15 y mayor a 3 el enemigo se mueve hacia el jugador
            if (Vector3.Distance(transform.position, target.position) < 15f && Vector3.Distance(transform.position, target.position) > 3f)
            {

                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);

                if (canAttack && timer >= timeBtwAttack)
                {
                    timer = 0f;
                    Debug.Log("El Vampiro Atacó");
                }
                else
                {
                    timer += Time.deltaTime;
                }
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            //si la distancia entre el enemigo y el jugador es menor a 3 el enemigo se queda quieto
            else if (Vector3.Distance(transform.position, target.position) < 3f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Kamikaze()
    {
        if (Vector3.Distance(transform.position, target.position) < 15f)
        {
            Action();
        }
        else
        {
            Movement();
        }
    }

    private void Action()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        if (rb.velocity.magnitude > 1)
        {
            rb.velocity = rb.velocity.normalized;
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Movement()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Shoot()
    {
        if (canAttack && timer >= timeBtwAttack)
        {
            timer = 0f;
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void SniperBehaviour()
    {
        if (Vector3.Distance(transform.position, target.position) < 15f)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);

            if (timer >= timeBtwAttack)
            {
                timer = 0f;
                Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            Movement();
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        HealthCheck();
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (room != null)
        {
            room.OnEnemyDefeated();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public enum EnemyType
    {
        Vampire,
        Wolf,
        Bat,
        Spider
    }
}
