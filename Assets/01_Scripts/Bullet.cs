using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 6;
    public float timeToDestroy = 4;
    public float damage = 1f;
    public bool playerBullet = false;

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
        if (playerBullet && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!playerBullet && collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Destroy(gameObject);
        }

		//else if (collision.gameObject.CompareTag("Bullet"))
		//{
		//    Destroy(gameObject);
		//}
		//else if (collision.gameObject.CompareTag("Boss"))
		//{
		//    Boss b = collision.gameObject.GetComponent<Boss>();
		//    b.TakeDamage(damage);
		//    Destroy(gameObject);
		//}
	}
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Bullet"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
