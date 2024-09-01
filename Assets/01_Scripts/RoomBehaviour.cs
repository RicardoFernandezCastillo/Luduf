using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{

	//public bool[] test;
	public GameObject[] walls; // 0 - Up 1 -Down 2 - Right 3- Left
	public GameObject[] doors;
	public RoomType type = RoomType.StartRoom;
	
	public List<Transform> positions;
	
	public List<GameObject> prefabsEnemys;


	public GameObject prefCopper;
	public List<Transform> copperPosition; //la  posicion de donde se va a instanciar eel cofre

	public enum RoomType
	{
		StartRoom,
		NormalRoom, //generaar cofres aleatorios
		SpiderRoom, //Inst
		VampireRoom,
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
				CloseDoors();//Serramos las Puertas
				InstanceEnemySpider();//Intanciamos los nemigos
				break;
			case RoomType.BossRoom:
				CloseDoors();
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



	void InstanceEnemySpider()
	{
		if (visited)
		{
			instanceEnemy();
			visited = false;
		}
	}



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
			Invoke("TypoRoomInstance", 1f);
		}
	}


	//para poder instanciar enemigos -------------------------------------------------------------------
	void  instanceEnemy()
	{
		for (int i = 0; i < walls.Length; i++)
		{
			if (!auxWall[i])
			{
				GameObject enemy = Instantiate(prefabsEnemys[0], positions[i].position, positions[i].rotation);
				Enemy enemyComponent = enemy.GetComponent<Enemy>();
				if (enemyComponent != null)
				{
					enemyComponent.room = this;
					enemiesRemaining++;
				}
			}
		}
	}

}
