using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
	public class Cell
	{
		public bool visited = false;
		public bool[] status = new bool[4];
	}

	public List<GameObject> RoomType;

	public Vector2Int size;
	public int startPos = 0;
	public Vector2 offset;


	public GameObject startRoom;
	public GameObject endRoom;

	List<Cell> board;
	void Start()
    {
		MazeGenerator();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void GenerateDungeon()
	{
		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
				if (currentCell.visited)
				{
					RoomBehaviour newRoom = null;

					if (i == 0 && j == 0)
					{
						newRoom = Instantiate(startRoom, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
					
					}else if (i == size.x - 1 && j == size.y - 1)
					{
						newRoom = Instantiate(endRoom, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
					}
					else
					{
						int room = Random.Range(0, RoomType.Count);
						newRoom = Instantiate(RoomType[room], new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
					}

					newRoom.UpdateRoom(currentCell.status);
					newRoom.name += " " + i + "-" + j;
				}	
			}
		}

	}


	void MazeGenerator()
	{
		board = new List<Cell>();

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				board.Add(new Cell());
			}
		}

		int currentCell = startPos;

		Stack<int> path = new Stack<int>();

		int k = 0;

		while (k < 900)
		{
			k++;

			board[currentCell].visited = true;

			if (currentCell == board.Count - 1)
			{
				break;
			}

			//Check the cell's neighbors
			List<int> neighbors = CheckNeighbors(currentCell);

			if (neighbors.Count == 0)
			{
				if (path.Count == 0)
				{
					break;
				}
				else
				{
					currentCell = path.Pop();
				}
			}
			else
			{
				path.Push(currentCell);

				int newCell = neighbors[Random.Range(0, neighbors.Count)];

				if (newCell > currentCell)
				{
					//down or right
					if (newCell - 1 == currentCell)
					{
						board[currentCell].status[2] = true;
						currentCell = newCell;
						board[currentCell].status[3] = true;
					}
					else
					{
						board[currentCell].status[1] = true;
						currentCell = newCell;
						board[currentCell].status[0] = true;
					}
				}
				else
				{
					//up or left
					if (newCell + 1 == currentCell)
					{
						board[currentCell].status[3] = true;
						currentCell = newCell;
						board[currentCell].status[2] = true;
					}
					else
					{
						board[currentCell].status[0] = true;
						currentCell = newCell;
						board[currentCell].status[1] = true;
					}
				}

			}

		}
		GenerateDungeon();
	}
	List<int> CheckNeighbors(int cell)
	{
		List<int> neighbors = new List<int>();

		//check up neighbor
		if (cell - size.x >= 0 && !board[(cell - size.x)].visited)
		{
			neighbors.Add((cell - size.x));
		}

		//check down neighbor
		if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
		{
			neighbors.Add((cell + size.x));
		}

		//check right neighbor
		if ((cell + 1) % size.x != 0 && !board[(cell + 1)].visited)
		{
			neighbors.Add((cell + 1));
		}

		//check left neighbor
		if (cell % size.x != 0 && !board[(cell - 1)].visited)
		{
			neighbors.Add((cell - 1));
		}

		return neighbors;
	}

}
