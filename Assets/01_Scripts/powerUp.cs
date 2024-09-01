using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
	public powerUps type = powerUps.powerLife;
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
			Debug.Log("Player recojio el entro" + type);
			typePower();
			Destroy(gameObject);
		}
	}

}
