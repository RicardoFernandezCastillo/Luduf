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

    public float xpGiven = 20f;

    [Header("Referencias")]
    public GameObject bulletDriderPrefab;
    public GameObject bulletGargolaPrefab;
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

    private int currentPhase = 1;

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
            case BossType.Drider:
                DriderBehaviour();
                break;
        }
    }

    private void DriderBehaviour()
    {
        if (life / maxLife > 0.5) //0.83
        {
            currentPhase = 1;
            FirstPhaseDrider(); //Primera Fase Ataque apuntado estatico
        }
        else if (life / maxLife >= 0)
        {
            if (currentPhase == 1)
            {
                timeBtwRangeAttack = timeBtwRangeAttack * 1.7f;
                currentPhase = 2;
            }
            //currentPhase = 2;
            SecondPhaseDrider(); //Segunda Fase Ataque apuntado con movimiento del Boss
        }
        //else if (life / maxLife > 0)
        //{
        //    //currentPhase = 3;
        //    ThirdPhaseDrider(); // Tercera Fase Ataque multiple estatico 

        //}
    }

    private void ThirdPhaseDrider()
    {
        throw new NotImplementedException();
    }

    private void SecondPhaseDrider()
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
            if (timerRangeAttack >= timeBtwRangeAttack)
            {
                timerRangeAttack = 0f;

                // Instanciar bala y asignar a la bala el daño que hace
                Bullet bullet1 = Instantiate(bulletDriderPrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
                bullet1.bulletType = Bullet.BulletType.Boss;
                bullet1.typeOfEnemy = Bullet.TypeOfEnemy.Spider;
                bullet1.timeVelocityDebuff = 2f;


                Quaternion rotacionB2 = Quaternion.Euler(0, -13f, 0);
                Bullet bullet2 = Instantiate(bulletDriderPrefab, firePoint.position, transform.rotation * rotacionB2).GetComponent<Bullet>();
                bullet2.bulletType = Bullet.BulletType.Boss;
                bullet2.typeOfEnemy = Bullet.TypeOfEnemy.Spider;
                bullet2.timeVelocityDebuff = 2f;


                Quaternion rotacionB3 = Quaternion.Euler(0, -13f, 0);
                Bullet bullet3 = Instantiate(bulletDriderPrefab, firePoint.position, transform.rotation * rotacionB3).GetComponent<Bullet>();
                bullet3.bulletType = Bullet.BulletType.Boss;
                bullet3.typeOfEnemy = Bullet.TypeOfEnemy.Spider;
                bullet3.timeVelocityDebuff = 2f;


                //bulletPrefab.GetComponent<Bullet>().damage = 1f;

                //AudioManager.instance.PlaySFX(batAttackSound);
            }
            else
            {
                timerRangeAttack += Time.deltaTime;
            }
        }
    }

    private void FirstPhaseDrider()
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
            if (timerRangeAttack >= timeBtwRangeAttack)
            {
                timerRangeAttack = 0f;
                // Instanciar bala y asignar a la bala el daño que hace

                Bullet bullet1 = Instantiate(bulletDriderPrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
                bullet1.bulletType = Bullet.BulletType.Enemy;
                bullet1.typeOfEnemy = Bullet.TypeOfEnemy.Spider;
                bullet1.timeVelocityDebuff = 2.5f;
                bullet1.hasPenetration = true;
                //bulletPrefab.GetComponent<Bullet>().damage = 1f;

                //AudioManager.instance.PlaySFX(batAttackSound);
            }
            else
            {
                timerRangeAttack += Time.deltaTime;
            }
        }
    }

    private void GargolaBehaviour()
    {
        if (life / maxLife > 0.5) //0.83
        {
            currentPhase = 1;
            FirstPhaseGargola(); //Primera Fase Ataque apuntado estatico
        }
        else if (life / maxLife >= 0)
        {
            if (currentPhase == 1)
            {
                timeBtwRangeAttack = timeBtwRangeAttack * 1.7f;
                currentPhase = 2;
            }
            //currentPhase = 2;
            SecondPhaseGargola(); //Segunda Fase Ataque apuntado con movimiento del Boss
        }
        //else if (life / maxLife > 0)
        //{
        //    //currentPhase = 3;
        //    ThirdPhaseDracula(); // Tercera Fase Ataque multiple estatico 

        //}
    }

    private void SecondPhaseGargola()
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
            if (timerRangeAttack >= timeBtwRangeAttack)
            {
                timerRangeAttack = 0f;

                // Instanciar bala y asignar a la bala el daño que hace
                Bullet bullet1 = Instantiate(bulletGargolaPrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
                bullet1.bulletType = Bullet.BulletType.Boss;
                bullet1.typeOfEnemy = Bullet.TypeOfEnemy.Bat;


                Quaternion rotacionB2 = Quaternion.Euler(0, -13f, 0);
                Bullet bullet2 = Instantiate(bulletGargolaPrefab, firePoint.position, transform.rotation * rotacionB2).GetComponent<Bullet>();
                bullet2.bulletType = Bullet.BulletType.Boss;
                bullet2.typeOfEnemy = Bullet.TypeOfEnemy.Bat;

                Quaternion rotacionB3 = Quaternion.Euler(0, -13f, 0);
                Bullet bullet3 = Instantiate(bulletGargolaPrefab, firePoint.position, transform.rotation * rotacionB3).GetComponent<Bullet>();
                bullet3.bulletType = Bullet.BulletType.Boss;
                bullet3.typeOfEnemy = Bullet.TypeOfEnemy.Bat;

                //bulletPrefab.GetComponent<Bullet>().damage = 1f;

                //AudioManager.instance.PlaySFX(batAttackSound);
            }
            else
            {
                timerRangeAttack += Time.deltaTime;
            }
        }
    }

    private void FirstPhaseGargola()
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
            if (timerRangeAttack >= timeBtwRangeAttack)
            {
                timerRangeAttack = 0f;
                // Instanciar bala y asignar a la bala el daño que hace

                Bullet bullet1 = Instantiate(bulletGargolaPrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
                bullet1.bulletType = Bullet.BulletType.Enemy;
                bullet1.typeOfEnemy = Bullet.TypeOfEnemy.Bat;
                bullet1.hasPenetration = true;
                //bulletPrefab.GetComponent<Bullet>().damage = 1f;

                //AudioManager.instance.PlaySFX(batAttackSound);
            }
            else
            {
                timerRangeAttack += Time.deltaTime;
            }
        }

    }

    private void LycanthropeBehaviour()
    {
        if (life / maxLife > 0.5) //0.83
        {
            currentPhase = 1;
            FirstPhaseLycan(); //Primera Fase Ataque apuntado estatico
        }
        else if (life / maxLife >= 0f)
        {
            if (currentPhase == 1)
            {
                speed = speed * 1.3f;
                damage = damage * 1.3f;
                timeBtwFisicAttack = timeBtwFisicAttack * 1.3f;
                currentPhase = 2;
            }
            //currentPhase = 2;
            SecondPhaseLycan(); //Segunda Fase Ataque apuntado con movimiento del Boss
        }
        //else if (life / maxLife > 0)
        //{
        //    //currentPhase = 3;
        //    ThirdPhaseLycan(); // Tercera Fase Ataque multiple estatico 

        //}
    }

    private void SecondPhaseLycan()
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
                    // Hacer daño al jugador
                    player.TakeDamage(damage);
                    //AudioManager.instance.PlaySFX(wolfAttackSound);

                }
                else
                {
                    timerFisicAttack += Time.deltaTime;
                }
            }
        }
    }

    private void FirstPhaseLycan()
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
                    // Hacer daño al jugador
                    player.TakeDamage(damage);
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
            FirstPhaseDracula(); //Primera Fase Ataque apuntado estatico
        }
        else if (life / maxLife >= 0.4)
        {
            //currentPhase = 2;
            SecondPhaseDracula(); //Segunda Fase Ataque apuntado con movimiento del Boss
        }
        else if (life / maxLife > 0)
        {
            //currentPhase = 3;
            ThirdPhaseDracula(); // Tercera Fase Ataque multiple estatico 

        }
        //else if (life / maxLife > 0f) //Ultima Fasw
        //{
        //    currentPhase = 4;
        //    FourPhase();
        //}



    }

    private void ThirdPhaseDracula()
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

    private void SecondPhaseDracula()
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

    private void FirstPhaseDracula()
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
                bullet1.timeVelocityDebuff = 0;

                Bullet bullet2 = Instantiate(bulletFirePrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
                bullet2.bulletType = Bullet.BulletType.Boss;
                bullet2.typeOfEnemy = Bullet.TypeOfEnemy.Dracula;
                bullet2.damage = damage * 2;
                bullet2.timeVelocityDebuff = 0;

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
