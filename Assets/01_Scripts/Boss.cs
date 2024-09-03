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
    public float timeBtwAttack = 2f;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;

    private float xpGiven = 5f;

    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Rigidbody rb;
    public BossType enemyType;

    public Transform leftRangeMovement;
    public Transform rightRangeMovement;

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

    #region BossBehaviourLogic





    private Transform objetivo;

    bool isMovingRight = true;


    int currentPhase = 1;

    float timerMovement = 0f;
    float timerBtwMovement = 3.5f;



    #endregion



    void Start()
    {
        objetivo = rightRangeMovement;
        EnemiesStats();
        PlayerLocation();
        AssignStats();
    }

    private void EnemiesStats()
    {
        switch (enemyType)
        {
            case BossType.Dracula:
                //speed = Random.Range(2f, 4f);
                break;
            case BossType.Licantropo:
                //speed = Random.Range(4f, 6f);
                break;
            case BossType.Gargola:
                //speed = Random.Range(2f, 4f);
                break;
        }
    }

    void AssignStats()
    {
        // cada 3 niveles que el player suba, los enemigos suben de nivel
        if (player.level % 3 == 0)
        {
            //segun el tipo de enemigo, se le asignan stats diferentes
            switch (enemyType)
            {
                case BossType.Dracula:
                    life += 2f;
                    maxLife += 2f;
                    damage += 1f;
                    xpGiven += 2f;
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
        switch (enemyType)
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



    private void DraculaBehaviour()
    {
        if (life / maxLife > 0.5) //0.83
        {
            currentPhase = 1;
            FirstPhase(); //Primera Fase Ataque apuntado estatico
        }
        else //if (life / maxLife > 0.67)
        {
            currentPhase = 2;
            SecondPhase(); //Segunda Fase Ataque apuntado con movimiento del Boss
        }
        //else if (life / maxLife > 0.25)
        //{
        //    currentPhase = 3;
        //    ThirdPhase(); // Tercera Fase Ataque multiple estatico 

        ////Increase
        //}
        //else if (life / maxLife > 0f) //Ultima Fasw
        //{
        //    currentPhase = 4;
        //    FourPhase();
        //}



    }

    private void ThirdPhase()
    {
        //Dracula se teletransporta en frente del jugador y lo ataca, luego vuelve a su posicion original
        if (target != null)
        {
            // tranportar a Dracula en frente del jugador es decir a 4 unidades de distancia
            Transform aux = transform;
            transform.position = Vector3.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);

        }

    }

    private void SecondPhase()
    {

        rb.velocity = new Vector3(0, 0, 0);
        if (target != null)
        {
            Vector3 dir = objetivo.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            //Shoot();




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

                //if (UnityEngine.Random.Range(0, 100) < 30)
                //{
                //    // Calcula la dirección hacia el jugador
                //    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                //    // Calcula la nueva posición
                //    Vector3 newPosition = player.transform.position - directionToPlayer * 2f;

                //    // Teletransporta a Drácula a la nueva posición
                //    transform.position = newPosition;

                //    Debug.Log("Dracula se teletransporta");
                //}
            }
        }
    }

    private void FirstPhase()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > 1.5f && canAttack)
            {

                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);


                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            //si la distancia entre el enemigo y el jugador es menor a 3 el enemigo se queda quieto
            if (Vector3.Distance(transform.position, target.position) <= 1.5f)
            {
                if (canAttack)
                {
                    Debug.Log("El Jefe Dracula Atac�");
                    player.TakeDamage(damage);
                    canAttack = false;

                }

            }

            if (!canAttack)
            {
                timer += Time.deltaTime;
                if (timer >= timeBtwAttack)
                {
                    canAttack = true;
                    timer = 0f;
                }
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
