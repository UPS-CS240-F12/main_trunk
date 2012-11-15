using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	void Start () 
	{
		index = 0;
		totalTiles = 0;
		flipping = false;
		gameTiles = new List<GameObject>();
		emptyLocations = new List<Vector3>();
		xWidth = m_tileDimensions;
		zWidth = m_tileDimensions;
		StartCoroutine ("SetDifficulty");
		//StartCoroutine("GenerateTiles");
	}
	
	//Gets the values from PlayerPrefs to overwrite default difficulty values.
	void SetDifficulty()
	{
		int difficulty = PlayerPrefs.GetInt("Difficulty");
		if(difficulty == 1)
		{
			xWidth = 0.125f; // 10% of total size
			zWidth = 0.125f;
			m_tileClone = m_tileEasy;
			respawnNum = 50;
			flipNum = 0;
			Debug.Log ("Easy Mode");
		}
		if(difficulty == 2)
		{
			xWidth = 0.075f;
			zWidth = 0.075f;
			m_tileClone = m_tileHard;
			respawnNum = 45;
			flipNum = 0;
			Debug.Log ("Hard Mode");
		}
		if(difficulty == 3)
		{
			xWidth = 0.05f;
			zWidth = 0.05f;
			m_tileClone = m_tileExtreme;
			respawnNum = 100;
			flipNum = 101;
			Debug.Log ("Extreme Mode");
		}
		xWidth = (int)(m_xSize * xWidth);
		zWidth = (int)(m_zSize * zWidth);
		xTiles = m_xSize / (int)xWidth;
		zTiles = m_zSize / (int)zWidth;
		StartCoroutine("GenerateTiles");
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
				gameTiles.Add(tile);
				totalTiles++;
			}
		}
	}
	
	Vector2 GetNumColumnsRows()
	{
		return new Vector2(xWidth,zWidth);
	}
	
	void GetRandomTile(TileMessenger messenger)
	{
		totalTiles = gameTiles.Count;
		int index = (int)Random.Range(0, totalTiles);
		Vector3 ret = gameTiles[index].transform.position;
		messenger.message = ret;
	}
	
	/*void ColorCycle()
	{
		totalTiles = gameTiles.Count;
		int index = 0;
		while(index < totalTiles)
		{
			gameTiles[index].SendMessage("ColorShift");
			index++;
		}
	}*/
			
	void RemoveTile(string name)
	{
		for(int i = gameTiles.Count - 1; i >= 0; i--)
		{
			GameObject checkMe = gameTiles[i];
			if(name == checkMe.name)
			{
				TileMessenger messenger = new TileMessenger();
				gameTiles[i].SendMessage("SendLocation", messenger);
				Vector3 addMe = messenger.message;
				gameTiles.RemoveAt(i);
				emptyLocations.Add(addMe);
				totalTiles--;
			}
		}
		if(totalTiles < respawnNum)
			StartCoroutine("RespawnTile");
	}
	
	void PatternFlip(int toFlip)
	{
		flipping = true;
		while(toFlip > 0)
		{
			gameTiles[toFlip].SendMessage("CommandRotate", 6);
			toFlip--;
		}
		flipping = false;
	}
	
	//Respawns a tile at a vacant location.
	void RespawnTile()
	{
		Vector3 newLocation = emptyLocations[0];
		emptyLocations.RemoveAt(0);
		newLocation.Scale (new Vector3(xWidth, 1, zWidth));
		GameObject tile = Instantiate(m_tileClone, newLocation, transform.rotation) as GameObject;
		tile.transform.localScale = new Vector3(xWidth, 30, zWidth);
		tile.transform.position -= new Vector3(0,10,0); // Temporary fix to respawn height error.
		tile.name = "tile x" + newLocation.x.ToString () + " z" + newLocation.z.ToString (); // Renaming Error
		//tile.SendMessage("CommandRotate", 6);
		//tile.SendMessage("RespawnMovement", 6);
		//Debug.Log ("Tile Respawning");
		gameTiles.Add(tile);
		totalTiles++;
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
		
	private List<GameObject> gameTiles;
	private List<Vector3> emptyLocations;
	
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