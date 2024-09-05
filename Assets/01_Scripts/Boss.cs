using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // Variables p�blicas configurables
    [Header("Stats")]
    public float speed = 6f;
    public float life = 2f;
    public float maxLife = 2f;

    public float damage = 1f;
    public float timeBtwFisicAttack = 2f;
    public float timeBtwRangeAttack = 1f;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;

    private float xpGiven = 20f;

    [Header("Referencias")]
    public GameObject bulletFirePrefab;
    public GameObject bulletIcePrefab;
    //public GameObject bulletFirePrefab;
    //public GameObject bulletFirePrefab;
    public Rigidbody rb;
    public BossType bossType;

    public Transform leftRangeMovement;
    public Transform rightRangeMovement;

    [Header("UI")]
    public Slider healthSlider;


    public RoomBehaviour room;

    // Variables privadas
    private float timerFisicAttack = 0f;
    private float timerRangeAttack = 0f;
    private bool canAttack = true;
    private Transform target;

    private Player player;

    [Header("Sounds")]
    public AudioClip vampireAttackSound;
    public AudioClip wolfAttackSound;
    public AudioClip batAttackSound;

    public AudioClip playerDeathSound;
    public AudioClip GameOverSound;
    public AudioClip levelUpSound;

    #region BossBehaviourLogic





    //private Transform objetivo;

    //bool isMovingRight = true;

    ////int currentPhase = 1;

    //float timerMovement = 0f;
    //float timerBtwMovement = 3.5f;



    #endregion



    void Start()
    {
        //objetivo = rightRangeMovement;
        //EnemiesStats();
        PlayerLocation();
        AssignStats();
    }

    private void EnemiesStats()
    {

        if (player.level % 2 == 0)
        { 
        
        
        }

    }

    void AssignStats()
    {
        // cada 3 niveles que el player suba, los enemigos suben de nivel
        if (player.level % 2 == 0)
        {
            //segun el tipo de enemigo, se le asignan stats diferentes
            switch (bossType)
            {
                case BossType.Dracula:
                    maxLife = maxLife + maxLife * 0.3f;
                    life = maxLife;

                    damage += 1f;
                    xpGiven = xpGiven + xpGiven * 0.3f;
                    break;
                case BossType.Licantropo:
                    life += 1f;
                    maxLife += 1f;
                    damage += 1f;
                    xpGiven += 1f;


                    break;
                case BossType.Gargola:
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
        switch (bossType)
        {
            case BossType.Dracula:
                DraculaBehaviour();
                break;
            case BossType.Licantropo:
                LycanthropeBehaviour();
                break;
            case BossType.Gargola:
                GargolaBehaviour();
                break;
        }
    }

    private void GargolaBehaviour()
    {

    }

    private void LycanthropeBehaviour()
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

                if (canAttack && timerFisicAttack >= timeBtwFisicAttack)
                {
                    timerFisicAttack = 0f;
                    Debug.Log("El Lobo Atac�");
                    // Hacer daño al jugador
                    MakeDamageToPlayer();
                    //AudioManager.instance.PlaySFX(wolfAttackSound);

                }
                else
                {
                    timerFisicAttack += Time.deltaTime;
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



    private void DraculaBehaviour()
    {
        if (life / maxLife > 0.75) //0.83
        {
            //currentPhase = 1;
            FirstPhase(); //Primera Fase Ataque apuntado estatico
        }
        else if (life / maxLife >= 0.5)
        {
            //currentPhase = 2;
            SecondPhase(); //Segunda Fase Ataque apuntado con movimiento del Boss
        }
        else if (life / maxLife > 0)
        {
            //currentPhase = 3;
            ThirdPhase(); // Tercera Fase Ataque multiple estatico 

        }
        //else if (life / maxLife > 0f) //Ultima Fasw
        //{
        //    currentPhase = 4;
        //    FourPhase();
        //}



    }

    private void ThirdPhase()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        if (Vector3.Distance(transform.position, target.position) > 10f)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, target.position) <= 10f)
        {
            if (canAttack)
            {
                Instantiate(bulletIcePrefab, firePoint.position, transform.rotation);
                bulletIcePrefab.GetComponent<Bullet>().bulletType = Bullet.BulletType.Boss;
                bulletIcePrefab.GetComponent<Bullet>().typeOfEnemy = Bullet.TypeOfEnemy.Dracula;
                bulletIcePrefab.GetComponent<Bullet>().damage = damage * 0.5f;
                bulletIcePrefab.GetComponent<Bullet>().timeVelocityDebuff = 2f;
                bulletIcePrefab.GetComponent<Bullet>().percentVelocityDebuff = 0.5f;


                Instantiate(bulletIcePrefab, firePoint2.position, transform.rotation);
                Debug.Log("El Jefe Dracula lanzo su ataque");
                canAttack = false;
            }
        }
        if (!canAttack)
        {
            timerRangeAttack += Time.deltaTime;
            if (timerRangeAttack >= timeBtwRangeAttack)
            {
                canAttack = true;
                timerRangeAttack = 0f;
            }
        }
    }

    private void ThirdPhase33()
    {
        if (target != null)
        {
            //Shoot();
            /*
            if (timerMovement < timerBtwMovement)
            {
                timerMovement += Time.deltaTime;
                if (isMovingRight)
                {
                    objetivo = leftRangeMovement;
                    //anim.SetFloat("x", -1);
                }
                else
                {
                    objetivo = rightRangeMovement;
                    //anim.SetFloat("x", 1);
                }
            }
            else
            {

                timerMovement = 0;
                isMovingRight = !isMovingRight;
                //probabilidad de 30% de teletransportarse


            } */


            if (Vector3.Distance(transform.position, target.position) > 9.3f && canAttack)
            {

                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);


                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, target.position) <= 2.3f)
            {
                if (canAttack)
                {

                    // Calcula la dirección hacia el jugador
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                    // Calcula la nueva posición
                    Vector3 newPosition = player.transform.position - directionToPlayer * 2f;

                    // Teletransporta a Drácula a la nueva posición
                    transform.position = newPosition;

                    Debug.Log("Dracula se teletransporta");
                    Debug.Log("El Jefe Dracula Ataco");
                    player.TakeDamage(damage* 0.5f);
                    canAttack = false;

                }
            }

            if (!canAttack)
            {
                timerFisicAttack += Time.deltaTime;
                if (timerFisicAttack >= timeBtwFisicAttack)
                {
                    canAttack = true;
                    timerFisicAttack = 0f;
                }
            }

        }


    }

    private void SecondPhase()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > 2.3f && canAttack)
            {

                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);


                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            //si la distancia entre el enemigo y el jugador es menor a 3 el enemigo se queda quieto
            if (Vector3.Distance(transform.position, target.position) <= 2.3f)
            {
                if (canAttack)
                {
                    Debug.Log("El Jefe Dracula Ataco");
                    player.TakeDamage(damage);
                    // el boss se recarga la vida con el mitad del daño
                    life += damage * 0.5f;
                    if (maxLife < life)
                    {
                        maxLife = life;
                    }
                    healthSlider.value = life / maxLife;

                    canAttack = false;

                }
            }

            if (!canAttack)
            {
                timerFisicAttack += Time.deltaTime;
                if (timerFisicAttack >= timeBtwFisicAttack)
                {
                    canAttack = true;
                    timerFisicAttack = 0f;
                }
            }

        }
    }

    private void FirstPhase()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        if (Vector3.Distance(transform.position, target.position) > 10f)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, target.position) <= 10f)
        {
            if (canAttack)
            {
                Bullet bullet1 = Instantiate(bulletFirePrefab, firePoint2.position, transform.rotation).GetComponent<Bullet>();
                bullet1.bulletType = Bullet.BulletType.Boss;
                bullet1.typeOfEnemy = Bullet.TypeOfEnemy.Dracula;
                bullet1.damage = damage * 2;

                Bullet bullet2 = Instantiate(bulletFirePrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
                bullet2.bulletType = Bullet.BulletType.Boss;
                bullet2.typeOfEnemy = Bullet.TypeOfEnemy.Dracula;
                bullet2.damage = damage * 2;
                Debug.Log("El Jefe Dracula lanzo su ataque");
                canAttack = false;

            }
        }
        if (!canAttack)
        {
            timerRangeAttack += Time.deltaTime;
            if (timerRangeAttack >= timeBtwRangeAttack)
            {
                canAttack = true;
                timerRangeAttack = 0f;
            }
        }

    }

    void MakeDamageToPlayer()
    {
        player.TakeDamage(damage);
    }

    void FourPhase()
    {
        //if (Vector3.Distance(transform.position, target.position) < 15f)
        //{
        //    Action();
        //}
        //else
        //{
        //    Movement();
        //}
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
        if (canAttack && timerFisicAttack >= timeBtwFisicAttack)
        {
            timerFisicAttack = 0f;
            Instantiate(bulletFirePrefab, firePoint.position, transform.rotation);
        }
        else
        {
            timerFisicAttack += Time.deltaTime;
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


            FindObjectOfType<Player>().mapLevel++;
            PlayerPref.SaveStats();
            SceneManager.LoadScene("PlayerScene");
        }
    }

    void OnDestroy()
    {
        if (room != null)
        {
            room.OnEnemyDefeated();
        }

    }

    public enum BossType
    {
        Dracula,
        Licantropo,
        Gargola,
        Drider
    }
}
