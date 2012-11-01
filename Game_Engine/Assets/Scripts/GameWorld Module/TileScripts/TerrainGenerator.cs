using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	void Start () 
	{
		index = 0;
		totalTiles = 0;
		gameTiles = new List<GameObject>();
		emptyLocations = new List<Vector3>();
		xWidth = m_tileDimensions;
		zWidth = m_tileDimensions;
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
		//if(totalTiles == 100)
			//StartCoroutine("PatternFlip", (int)totalTiles);
		if(totalTiles < 50)
			StartCoroutine("RespawnTile");
	}
	
	void PatternFlip(int toFlip)
	{
		while(toFlip > 0)
		{
			int randIndex = Random.Range(0,99);
			gameTiles[randIndex].SendMessage("CommandRotate", 6);
			toFlip--;
		}
	}
	
	//Respawns a tile at a vacant location.
	void RespawnTile()
	{
		Vector3 newLocation = emptyLocations[0];
		emptyLocations.RemoveAt(0);
		GameObject tile = Instantiate(m_tileClone, newLocation, 
				transform.rotation) as GameObject;
		tile.name = "tile xVal" + newLocation.x + " zVal" + newLocation.z;
		gameTiles.Add(tile);
		totalTiles++;
	}
	
	int totalTiles;
	int index; // Location in the gameTiles list for the searched tile.
	int xWidth;
	int zWidth;
		
	private List<GameObject> gameTiles;
	private List<Vector3> emptyLocations;
	
	[SerializeField]
	private GameObject m_tileClone;
	[SerializeField]
	private int m_xSize;
	[SerializeField]
	private int m_zSize;
	[SerializeField]
	private int m_tileDimensions;
	

}