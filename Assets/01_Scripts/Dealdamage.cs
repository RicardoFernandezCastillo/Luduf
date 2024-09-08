using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealdamage : MonoBehaviour
{

	public float damage = 0.5f;
	public Transform firePoint;

	private Player player;

    private void Start()
    {
		player = FindObjectOfType<Player>();
        
    }
    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			Enemy enemy = other.GetComponent<Enemy>();
			enemy.TakeDamage(damage, true);
			Debug.Log("Atacanddo con la culata de la arma");
		}
		else if (other.CompareTag("Bullet"))
		{
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                // Cambia la dirección de la bala
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                if (bulletRb != null)
                {
                    // Detener la bala
                    bulletRb.velocity = Vector3.zero;

                    // Calcular la nueva dirección hacia el jugador
                    Vector3 returnDirection = (firePoint.position - bullet.transform.position).normalized;

                    // Asignar la nueva velocidad
                    bulletRb.velocity = returnDirection * bullet.speed; 

                    // Opcional: Actualiza las propiedades de la bala
                    bullet.bulletType = Bullet.BulletType.Player;
                    bullet.hasPenetration = true;
                }

                Debug.Log("Parry");
            }
        }
	}
}
