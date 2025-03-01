using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    // Variables p�blicas configurables
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

    private float xpGiven = 5f;

    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Rigidbody rb;
    public EnemyType enemyType;

    [Header("UI")]
    public Slider healthSlider;


    public RoomBehaviour room;

    // Variables privadas
    private float timer = 0f;
    private bool canAttack = false;
    private Transform target;

    private Player player;

    [Header("Sounds")]
    public AudioClip vampireAttackSound;
    public AudioClip wolfAttackSound;
    public AudioClip batAttackSound;

    public AudioClip playerDeathSound;
    public AudioClip GameOverSound;
    public AudioClip levelUpSound;




    void Start()
    {

        EnemiesStats();
        PlayerLocation();
        AssignStats();
    }

    private void EnemiesStats()
    {
        switch (enemyType)
        {
            case EnemyType.Vampire:
                //speed = Random.Range(2f, 4f);
                Destroy(gameObject, timeToDestroy);
                break;
            case EnemyType.Wolf:
                //speed = Random.Range(4f, 6f);
                Destroy(gameObject, timeToDestroy);
                break;
            case EnemyType.Bat:
                //speed = Random.Range(2f, 4f);
                Destroy(gameObject, timeToDestroy);
                break;
        }
    }

    void AssignStats()
    {
        // cada 3 niveles que el player suba, los enemigos suben de nivel
        if(player.level % 3 == 0)
        {
            //segun el tipo de enemigo, se le asignan stats diferentes
            switch (enemyType)
            {
                case EnemyType.Vampire:
                    life += 2f;
                    maxLife += 2f;
                    damage += 1f;
                    xpGiven += 2f;
                    break;
                case EnemyType.Wolf:
                    life += 1f;
                    maxLife += 1f;
                    damage += 1f;
                    xpGiven += 1f;
                    break;
                case EnemyType.Bat:
                    life += 1f;
                    maxLife += 1;
                    damage += 1f;
                    xpGiven += 1f;
                    break;
            }
        }

    }

    private void PlayerLocation()
    {
        GameObject targetGo = GameObject.FindGameObjectWithTag("Player");
        player = targetGo.GetComponent<Player>();
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
                BatBehaviour();
                break;
        }
    }

    private void BatBehaviour()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        if (Vector3.Distance(transform.position, target.position) > 10f)
        {

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, target.position) < 10f)
        {
            //transform.rotation = Quaternion.Euler(0, 180, 0);

            if (timer >= timeBtwAttack)
            {
                timer = 0f;
                Instantiate(bulletPrefab, firePoint.position, transform.rotation);
                //AudioManager.instance.PlaySFX(batAttackSound);
            }
            else
            {
                timer += Time.deltaTime;
            }
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


                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            //si la distancia entre el enemigo y el jugador es menor a 3 el enemigo se queda quieto
            else if (Vector3.Distance(transform.position, target.position) < 3f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

                if (canAttack && timer >= timeBtwAttack)
                {
                    timer = 0f;
                    Debug.Log("El Lobo Atac�");
                    // Hacer daño al jugador
                    MakeDamageToPlayer();
                    //AudioManager.instance.PlaySFX(wolfAttackSound);

                }
                else
                {
                    timer += Time.deltaTime;
                }
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


                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            //si la distancia entre el enemigo y el jugador es menor a 3 el enemigo se queda quieto
            else if (Vector3.Distance(transform.position, target.position) < 3f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

                if (canAttack && timer >= timeBtwAttack)
                {
                    timer = 0f;
                    Debug.Log("El Vampiro Atac�");
                    // Hacer daño al jugador
                    MakeDamageToPlayer();
                    //AudioManager.instance.PlaySFX(vampireAttackSound);

                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
    }
    void MakeDamageToPlayer()
    {
        player.TakeDamage(damage);
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


    public void TakeDamage(float damage, bool p)
    {
        life -= damage;
        HealthCheck();

        if (life <= 0)
        {
            if (p)
            {

                player.IncreaseXp(xpGiven);
            }
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
