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
				break;
			case powerUps.powerRun:
				break;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			player = collision.gameObject.GetComponent<Player>();

			Destroy(gameObject);

            typePower();
        }
	}

}
