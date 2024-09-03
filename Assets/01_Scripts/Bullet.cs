using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    public float timeToDestroy = 4;
    public float damage = 1f;

    public BulletType bulletType;
    public TypeOfEnemy typeOfEnemy;

    public bool hasPenetration = false;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                HandleEnemyCollision(collision);
                break;
            case "Player":
                HandlePlayerCollision(collision);
                break;
            case "Bullet":
                HandleBulletCollision(collision);
                break;
            case "Boss":
                HandleBossCollision(collision);
                break;
        }
    }

    private void HandleBossCollision(Collider collision)
    {
        Boss boss = collision.gameObject.GetComponent<Boss>();
        if (bulletType == BulletType.Player)
        {
            boss.TakeDamage(damage, true);
            Destroy(gameObject);
        }
    }

    private void HandleBulletCollision(Collider collider)
    {
        if (hasPenetration) // si no penetra, se destruye al colisionar con otra bala
        {
            // si ambas balas son penetrantes, no se destruyen
            if (collider.gameObject.GetComponent<Bullet>().hasPenetration)
            {
                return;
            }
            else
            {
                // Si la bala es del jugador y es penetrante y colisiona con una bala enemiga, se destruye la bala enemiga
                if (bulletType == BulletType.Player)
                {
                    Destroy(collider.gameObject);
                }
                // Si la bala es de un enemigo y es penetrante y colisiona con una bala del jugador, se destruye la bala del jugador
                else
                {
                    Destroy(gameObject);
                }
            }


        }
        // Si las balas del player colisionan entre si no destroza ninguna
        else if (bulletType == BulletType.Player && collider.gameObject.GetComponent<Bullet>().bulletType == BulletType.Player)
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void HandlePlayerCollision(Collider collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        switch (typeOfEnemy)
        {
            case TypeOfEnemy.Spider:
                player.TakeDebuffVelocity(2f, 0.5f);
                player.TakeDamage(damage);
                break;
            case TypeOfEnemy.Vampire:
                player.TakeDamage(damage);
                break;
            case TypeOfEnemy.Bat:
                //player.TakeDebuffVelocity(2f, 0.2f);
                player.TakeDamage(damage);
                break;

            //Boses:
            case TypeOfEnemy.Dracula:
                player.TakeDamage(damage * 0.5f);
                break;
        }
        Destroy(gameObject);
    }

    private void HandleEnemyCollision(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (bulletType == BulletType.Player) // si la bala es del jugador, hace da�o al enemigo
        {
            enemy.TakeDamage(damage, true); // true porque es da�o del jugador
        }
        else
        {
            enemy.TakeDamage(damage, false); // false porque es da�o de un enemigo
        }
        Destroy(gameObject);
    }

    public enum BulletType
    {
        Player,
        Enemy,
        Boss
    }
    public enum TypeOfEnemy
    {
        Vampire,
        Spider,
        Bat,
        Player,
        Dracula,
        Lican,
        Gargola
    }
}
