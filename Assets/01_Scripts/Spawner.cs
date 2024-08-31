using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	public List<GameObject> prefabsEnemys;
	public static Spawner instance;
	public List<Transform> positions;
	// Start is called before the first frame update


	void Start()
    {
        
    }

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void SetSpawnPositions(List<Transform> newPositions)
	{
		positions = newPositions; // Asigna las posiciones del cuarto actual al spawner.
	}

	public int generateEnemy(string typeRoom, bool[] walls)
	{
		int enemyCount = 0;
		switch (typeRoom)
		{
			case "SpiderRoom":
			enemyCount = instanceEnemy(typeRoom, walls);
				break;
		}
		return enemyCount;
	}


	int instanceEnemy(string typeroom, bool[] walls)
	{
		int enemyCount = 0;
		for (int i = 0; i < walls.Length; i++)
		{
			if (!walls[i])
			{
				GameObject enemy = Instantiate(prefabsEnemys[0], positions[i].position, positions[i].rotation);
				Enemy enemyComponent = enemy.GetComponent<Enemy>();
				if (enemyComponent != null)
				{
					enemyComponent.room = RoomBehaviour.instance;
					enemyCount++;
				}
			}
		}
		return enemyCount;
	}


}
