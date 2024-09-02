using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
	public powerUps type = powerUps.powerLife;

	public Player player;
	// Start is called before the first frame update
	public enum powerUps
	{
		powerLife,
		powerBullet,
		powerRun
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void typePower()
	{
		switch (type)
		{
			case powerUps.powerLife:
				player.IncreaseHealth(1f);
                break;
			case powerUps.powerBullet:
				Debug.Log("Si entro para aniadir Balas");
				int n = Random.Range(5, 10);
				player.AddShoot(10);
				break;
			case powerUps.powerRun:
				break;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Entro a la Colicion");
			player = collision.gameObject.GetComponent<Player>();
			Destroy(gameObject);
            typePower();
        }
	}

}
