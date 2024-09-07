using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealdamage : MonoBehaviour
{

	public float damage = 0.5f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			Enemy enemy = other.GetComponent<Enemy>();
			enemy.TakeDamage(damage, true);
			Debug.Log("Atacanddo con la culata de la arma");
		}
	}
}
