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
			xWidth = 250;
			zWidth = 250;
			m_tileClone = m_tileEasy;
			respawnNum = 50;
			flipNum = 0;
			Debug.Log ("Easy Mode");
		}
		if(difficulty == 2)
		{
			xWidth = 150;
			zWidth = 150;
			m_tileClone = m_tileHard;
			respawnNum = 45;
			flipNum = 0;
			Debug.Log ("Hard Mode");
		}
		if(difficulty == 3)
		{
			xWidth = 75;
			zWidth = 75;
			m_tileClone = m_tileExtreme;
			respawnNum = 100;
			flipNum = 101;
			Debug.Log ("Extreme Mode");
		}
		StartCoroutine("GenerateTiles");
	}
	
	void GenerateTiles()
	{
		int x = 0;
		while(x < m_xSize)
		{
			for (int z = 0; z < m_zSize; z += zWidth)
			{    
				GameObject tile = Instantiate(m_tileClone, new Vector3(x,-10,z), transform.rotation) as GameObject;
				tile.name = "tile xVal" + x.ToString() + " zVal" + z.ToString();
				gameTiles.Add(tile);
				//Debug.Log ("Tile " + tile.name + " added");
				totalTiles++;
			}
			x += xWidth;
		}
	}
	
	void GetRandomTile(TileMessenger messenger)
	{
		totalTiles = gameTiles.Count;
		int index = (int)Random.Range(0, totalTiles);
		Vector3 ret = gameTiles[index].transform.position;
		//Debug.Log ("I just sent " + gameTiles[index].name + "'s location!");
		messenger.message = ret;
	}
			
	void RemoveTile(string name)
	{
		for(int i = gameTiles.Count - 1; i >= 0; i--)
		{
			GameObject checkMe = gameTiles[i];
			if(name == checkMe.name)
			{
				Vector3 addMe = gameTiles[i].transform.position;
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
		GameObject tile = Instantiate(m_tileClone, newLocation += new Vector3(0,500,0), 
				transform.rotation) as GameObject;
		tile.name = "tile xVal" + newLocation.x + " zVal" + newLocation.z;
		tile.SendMessage("CommandRotate", 6);
		tile.SendMessage("RespawnMovement", 6);
		//Debug.Log ("Tile Respawning");
		gameTiles.Add(tile);
		totalTiles++;
	}
	
	int totalTiles;
	int index; // Location in the gameTiles list for the searched tile.
	int xWidth;
	int zWidth;
	
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
	private int m_tileDimensions;
	

}