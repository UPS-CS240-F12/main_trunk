using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	void Start () 
	{
		universalNormColor = Color.white;
		
		index = 0;
		totalTiles = 0;
		flipping = false;
		emptyLocations = new List<Vector3>();
		occupiedLocations = new List<Vector2>();
		xWidth = m_tileDimensions;
		zWidth = m_tileDimensions;
		respawnNum = 50;
		StartCoroutine ("SetGameWorld");
		//StartCoroutine("GenerateTiles");
	}
	
	void Awake()
	{
		StartCoroutine ("DecomposeWorld");
	}
	
	//Gets the values from PlayerPrefs to overwrite default difficulty values.
	void SetGameWorld()
	{
		//int map = PlayerPrefs.GetInt("Map");
		int map = 1; //Hard coded to only create the first map
		if(map == 1)
		{
			xWidth = 0.125f; // 10% of total size
			zWidth = 0.125f;
			m_tileClone = m_tileEasy;
			flipNum = 0;
		}
		/*if(map == 2)
		{
			xWidth = 0.075f;
			zWidth = 0.075f;
			m_tileClone = m_tileHard;
			respawnNum = 45;
			flipNum = 0;
		}
		if(map == 3)
		{
			xWidth = 0.05f;
			zWidth = 0.05f;
			m_tileClone = m_tileExtreme;
			//respawnNum = 100;
			flipNum = 101;
		}*/
		xWidth = (int)(m_xSize * xWidth);
		zWidth = (int)(m_zSize * zWidth);
		xTiles = m_xSize / (int)xWidth;
		zTiles = m_zSize / (int)zWidth;
		gameTiles = new GameObject[xTiles, zTiles];
		StartCoroutine("GenerateTiles");
	}
	
	IEnumerator DecomposeWorld()
	{
		yield return StartCoroutine(MyWaitFunction(5.0f)); // Safe period.
		float destroyInterval = 2.0f;
		int recoveryChance = 15; // 0 = no respawns, 100 = full respawns.
		while(true)
		{
			yield return StartCoroutine(MyWaitFunction(destroyInterval));
				StartCoroutine ("DestroyTile");
			int temp = Random.Range (0,100);
			if(temp < recoveryChance)
				StartCoroutine ("RespawnTile");
			
				
		}
		
	}
	
	// Generates tiles with the specified tile dimensions and board size.
	void GenerateTiles()
	{
		for(int x = -xTiles/2; x < xTiles/2; x++)
		{
			for (int z = -zTiles/2; z < zTiles/2; z++)
			{    
				GameObject tile = Instantiate(m_tileClone, new Vector3(xWidth*x,-10,zWidth*z), transform.rotation) as GameObject;
				tile.transform.localScale = new Vector3(xWidth, 30, zWidth);
				tile.SendMessage("SetValues", new Vector2(x,z));
				tile.name = "tile x" + x + " z" + z;
				int tempX = x + 4;
				int tempZ = z + 4;
				gameTiles[tempX, tempZ] = tile;
				occupiedLocations.Add(new Vector2(tempX, tempZ));
				totalTiles++;
			}
		}
	}
	
	List<Vector3> GetDeletedTiles()
	{
		return emptyLocations;
	}
		
	
	void GetRandomTile(TileMessenger messenger)
	{
		int index = Random.Range (0,occupiedLocations.Count-1);
		if(occupiedLocations[index] != null)
		{
			int xVal = (int)occupiedLocations[index].x;
			int zVal = (int)occupiedLocations[index].y;
			Vector3 ret = gameTiles[xVal, zVal].transform.position;
			messenger.message = ret;
		}
		else
		{
			messenger.message = new Vector3(-5000, 0 -5000);
		}
		
	}
	
	void ReturnColor(TileMessenger messenger)
	{
		Vector3 ret = new Vector3(universalNormColor.r, universalNormColor.g, universalNormColor.b);
		messenger.message = ret;
	}
	
	void DestroyTile()
	{
		if(totalTiles > 0)
		{
			int index = Random.Range (0,occupiedLocations.Count-1);
			int xVal = (int)occupiedLocations[index].x;
			int zVal = (int)occupiedLocations[index].y;
			occupiedLocations.RemoveAt(index);
			TileMessenger messenger = new TileMessenger();
			gameTiles[xVal, zVal].SendMessage("SendLocation", messenger);
			Vector3 addMe = messenger.message;
			emptyLocations.Add(addMe);
			gameTiles[xVal, zVal].SendMessage("DeleteTile");
			totalTiles--;
		}
	}
	
	void ChangeColor(Vector2 coords)
	{
		int xVal = (int)coords.x;
		int zVal = (int)coords.y;
		gameTiles[xVal, zVal].SendMessage("ColorShift", Color.blue);
	}
	
	//Respawns a tile at a vacant location.
	void RespawnTile()
	{
		Vector3 newLocation = emptyLocations[0];
		emptyLocations.RemoveAt(0);
		Vector3 tempLocation = newLocation;
		tempLocation.Scale (new Vector3(xWidth, 1, zWidth));
		GameObject tile = Instantiate(m_tileClone, tempLocation, transform.rotation) as GameObject;
		tile.transform.localScale = new Vector3(xWidth, 30, zWidth);
		tile.transform.position -= new Vector3(0,10,0); // Temporary fix to respawn height error.
		tile.name = "tile x" + newLocation.x + " z" + newLocation.z;
		//tile.SendMessage("CommandRotate", 6);
		//tile.SendMessage("RespawnMovement", 6);
		//Debug.Log ("Tile Respawning");
		gameTiles[(int)newLocation.x + 4, (int)newLocation.z + 4] = tile;
		occupiedLocations.Add(new Vector2(newLocation.x + 4, newLocation.z + 4));
		totalTiles++;
	}
	
	IEnumerator MyWaitFunction (float delay) 
	{
        float timer = Time.time + delay;
        while (Time.time < timer) 
		{
            yield return null;
        }
    }
	
	int totalTiles;
	int index; // Location in the gameTiles list for the searched tile.
	float xWidth;
	float zWidth;
	
	int xTiles; // The number of tiles along the X axis.
	int zTiles; // The number of tiles along the Z axis.
	
	int respawnNum; // The minimum number of tiles possible.
	int flipNum; // The number of tiles remaining at which point the game will start getting extremely hard.
	bool flipping;
	
	Color universalNormColor;
	
	private GameObject[,] gameTiles;
	private List<Vector3> emptyLocations;
	private List<Vector2> occupiedLocations;
	
	[SerializeField]
	public GameObject m_tileClone;
	
	[SerializeField]
	public GameObject m_tileEasy;
	[SerializeField]
	public GameObject m_tileHard;
	[SerializeField]
	public GameObject m_tileExtreme;
	
	[SerializeField]
	private int m_xSize;
	[SerializeField]
	private int m_zSize;
	[SerializeField]
	private float m_tileDimensions;
	

}