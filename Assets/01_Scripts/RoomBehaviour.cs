using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;

//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomBehaviour : MonoBehaviour
{

	//public bool[] test;
	public GameObject[] walls; // 0 - Up 1 -Down 2 - Right 3- Left
	public GameObject[] doors;
	public RoomType type = RoomType.StartRoom;
	
	public List<Transform> positions;

	//public List<GameObject> prefabsEnemys;
	public GameObject prefabEnemy;

	public GameObject prefabGargola;
	public GameObject prefabDrider;
	public GameObject prefabLican;
	public GameObject prefabDracula;

	//para la Iluminasion
	public List<GameObject> FirePoint;
	public float activationDelay = 0.5f;  // Tiempo entre la activación de cada punto.

	//------------------------------------------------
	public GameObject prefCopper;
	public List<Transform> copperPosition; //la  posicion de donde se va a instanciar eel cofre

	public enum RoomType
	{
		StartRoom,
		NormalRoom,
		SpiderRoom, 
		Bat,
		WolfRoom,
		BossRoom
	}

	public int enemiesRemaining = 0;
	private bool roomCleared = false;

	public bool[] auxWall = new bool[4];
	public bool visited;

	public static RoomBehaviour instance;

	void Awake()
	{
		instance = this;
	}
	void Start()
	{
		visited = true;//para poder Iniialisar Cuando es la primera ves que visita el cuarto
	}
	void Update()
	{
		//UpdateRoom(auxDoor);
	}

	public void UpdateRoom(bool[] status)
	{
		auxWall = (bool[])status.Clone();
		for (int i = 0; i < status.Length; i++)
		{
			doors[i].SetActive(status[i]);
			walls[i].SetActive(!status[i]);
		}
	}

	

	void TypoRoomInstance()
	{
		switch (type)
		{
			case RoomType.NormalRoom:
				copperInstanse();
				break;
			case RoomType.SpiderRoom:
				CloseDoors();
				InstanceEnemy();
				break;
			case RoomType.WolfRoom:
				CloseDoors();
				InstanceEnemy();
				break;
			case RoomType.Bat:
				CloseDoors();
				InstanceEnemy();
				break;
			case RoomType.BossRoom:
				CloseDoors();
				InstanceBoss();
                break;
		}
	}


	//public List<Transform> copperPosition; //la  posicion de donde se va a instanciar eel cofre
	//para NormalRoom
	void copperInstanse()
	{
		if (visited)
		{
			int p = Random.Range(0, copperPosition.Count);
			Instantiate(prefCopper, copperPosition[p].position, copperPosition[p].rotation);
			visited = false;
		}		
	}


	//para poder cerrar las Puestas
	void CloseDoors()
	{
		if (visited)
		{
			for (int i = 0; i < walls.Length; i++)
			{
				if (auxWall[i])
				{
					Debug.Log($"Closing door {walls[i].name}");
					walls[i].SetActive(true);
				}
			}
		}
	}
	
	//para poder Activcar las luces del Cuarto
	IEnumerator ActivateFirePointsConsecutively()
	{
		for (int i = 0; i < FirePoint.Count; i++)
		{
			FirePoint[i].SetActive(true);  // Activa el FirePoint actual.
			yield return new WaitForSeconds(activationDelay);  // Espera el tiempo especificado antes de activar el siguiente.
		}
	}




	//Para poder Abrir las puertas
	void OpenDoors()
	{
		if (!roomCleared)
		{
			for (int i = 0; i < walls.Length; i++)
			{
				if (auxWall[i])
				{
					walls[i].SetActive(false);
				}
			}
			roomCleared = true;
		}
	}


	// Llamado cuando un enemigo es derrotado
	public void OnEnemyDefeated()
	{
		Debug.Log("Entro y la cantidad es :" + enemiesRemaining);
		enemiesRemaining-- ;	
		if (enemiesRemaining <= 0)
		{
			OpenDoors();
		}
	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player entered room: "+ type);
			StartCoroutine(ActivateFirePointsConsecutively());
			Invoke("TypoRoomInstance", 1f);
		}
	}




	//Para poder Instanciar al enemigo
	void InstanceEnemy()
	{
		if (visited)
		{
			instanceEnemy();
			visited = false;
		}
	}

	//para poder instanciar enemigos -------------------------------------------------------------------
	void  instanceEnemy()
	{
		for (int i = 0; i < walls.Length; i++)
		{
			if (!auxWall[i])
			{
				GameObject enemy = Instantiate(prefabEnemy, positions[i].position, positions[i].rotation);
				Enemy enemyComponent = enemy.GetComponent<Enemy>();
				if (enemyComponent != null)
				{
					enemyComponent.room = this;
					enemiesRemaining++;
				}
			}
		}
	}
	void InstanceBoss()
	{
		//Obtener al player para poder instanciar al Boss
		Player player = FindObjectOfType<Player>();
		int lvl = player.mapLevel;

        // Existen 8 niveles y hay 4 jefes, por lo que cada jefe aparece en 2, 4, 6 y 8.
        if (lvl % 8 == 0)
        {
            // Dracula
            Instantiate(prefabDracula, positions[0].position, positions[0].rotation);
        }
        else if (lvl % 6 == 0)
        {
            // Drider
            Instantiate(prefabDrider, positions[0].position, positions[0].rotation);
        }
        else if (lvl % 4 == 0)
        {
            // Lican
            Instantiate(prefabLican, positions[0].position, positions[0].rotation);
        }
        else if (lvl % 2 == 0)
        {
            // Gargola
            Instantiate(prefabGargola, positions[0].position, positions[0].rotation);
        }
        else
        {
            FindObjectOfType<Player>().mapLevel++;
            PlayerPref.SaveStats();
            SceneManager.LoadScene("PlayerScene");
            Debug.Log("No hay boss en este nivel");
        }



        //GameObject boss = Instantiate(prefabEnemy, positions[0].position, positions[0].rotation);
        //      Boss bossComponent = boss.GetComponent<Boss>();
        //      if (bossComponent != null)
        //      {
        //	bossComponent.room = this;
        //      }
    }
}
