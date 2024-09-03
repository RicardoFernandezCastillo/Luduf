using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copper : MonoBehaviour
{

    public List<GameObject> PowerPrefas;
    public Transform transCopper;
    bool isInstanse = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstancePower()
    {
        if (isInstanse)
        {
			 int n = Random.Range(0, 2);
             n = 1;
			if (n == 1)
			{
				int p = Random.Range(0, PowerPrefas.Count);
				Instantiate(PowerPrefas[p], transCopper.position, transCopper.rotation);
			}
			isInstanse=false;
		}
    }


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Bullet"))
		{
            //Invoke("InstancePower", 0.3f);
            Destroy(gameObject);
            Debug.Log("Entro para el cofre ");
            InstancePower();
			Destroy(other.gameObject);
		}
	}
}
