using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	void Start () 
	{
		totalTiles = 0;
		gameTiles = new List<GameObject>();
		StartCoroutine("GenerateTiles");
	}
	
	void GenerateTiles()
	{
		int xWidth = m_tileDimensions;
		int zWidth = m_tileDimensions;
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
		//Debug.Log ("Tile removing?");
		for(int i = gameTiles.Count - 1; i >= 0; i--)
		{
			GameObject removeMe = gameTiles[i];
			if(name == removeMe.name)
			{
				gameTiles.RemoveAt(i);
				//Debug.Log ("Tile " + name + " removed");
				i = -1;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	int totalTiles;
		
	private List<GameObject> gameTiles;

	[SerializeField]
	private GameObject m_tileClone;
	[SerializeField]
	private int m_xSize;
	[SerializeField]
	private int m_zSize;
	[SerializeField]
	private int m_tileDimensions;
	

}