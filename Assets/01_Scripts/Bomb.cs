using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public float delay = 2f; // Tiempo antes de que la bomba explote
	public float explosionRadius = 5f; // Radio de la explosión
	public float explosionForce = 700f; // Fuerza de la explosión
	public float damage = 5f; // Daño que inflige la bomba

	//public GameObject explosionEffect; // Efecto de explosión

	private bool hasExploded = false;

	public ParticleSystem explosionEffect;

	void Start()
	{
		// Inicia la cuenta regresiva para la explosión
		Invoke("Explode", delay);
	}

	void Explode()
	{
		// Verifica si ya ha explotado
		if (hasExploded) return;
		hasExploded = true;

		// Instanciar el efecto de explosión
		//if (explosionEffect != null)
		//{
		//	Instantiate(explosionEffect, transform.position, transform.rotation);
		//}

		// Detectar todos los objetos dentro del radio de la explosión
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach (Collider nearbyObject in colliders)
		{
			// Aplicar una fuerza a los objetos con Rigidbody
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
			}

			// Aplicar daño al jugador si está dentro del radio
			Player player = nearbyObject.GetComponent<Player>();
			if (player != null)
			{
				Debug.Log("Si hay danio en el player");
				player.TakeDamage(damage);

			}

			Enemy enemy = nearbyObject.GetComponent<Enemy>();
			if (enemy != null)
			{
				Debug.Log("Si hay danio en el player");
				enemy.TakeDamage(damage, false);

			}


		}
		explosionEffect.Play();
		// Destruir la bomba después de la explosión
		Destroy(gameObject);
	}

	void OnDrawGizmosSelected()
	{
		// Dibuja un gizmo en el editor para visualizar el radio de la explosión
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}

}
