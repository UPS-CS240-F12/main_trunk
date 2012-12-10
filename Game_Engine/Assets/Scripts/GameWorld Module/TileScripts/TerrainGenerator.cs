using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {
	
	/*
	 * Establishes default values for fields, and calls the SetGameWorld 
	 * in order to determine the gameworld to play on.
	 */
	void Start () 
	{
		universalNormColor = Color.white;
		totalTiles = 0;
		m_tileClone = null;
		emptyLocations = new List<Vector3>();
		deletedTiles = new List<Vector3>();
		occupiedLocations = new List<Vector2>();
		StartCoroutine ("SetGameWorld");
	}
	
	/*
	 * After initialization, the gameworld will begin to deteriorate by calling
	 * the DecomposeWorld method, which will loop until the game ends.
	 */
	void Awake()
	{
		StartCoroutine ("DecomposeWorld");
	}
	
	/*
	 * Gets the values from PlayerPrefs to determine the map to play.
	 */
	void SetGameWorld()
	{
		int map = PlayerPrefs.GetInt("Map");
		// Map setting one is the default gameworld. This map has a grid of 8x8 tiles.
		if(map == 1)
		{
			// Percentage values for the size of tiles compared to total gameboard size.
			xWidth = 0.125f;
			zWidth = 0.125f;
			m_tileClone = m_tileOne;
		}
		// Map setting two is the second gameplay mode. This map has 8x8 tiles, with
		// tiles of a different texture.
		if(map == 2)
		{
			// Percentage values for the size of tiles compared to total gameboard size.
			xWidth = 0.125f;
			zWidth = 0.125f;
			m_tileClone = m_tileTwo;
		}
		// Sets the tile dimensions to the actual values (gameboard size / tiles per axis).
		xWidth = (int)(m_xSize * xWidth);
		zWidth = (int)(m_zSize * zWidth);
		// Calculates the number of tiles along the x and z axes.
		xTiles = m_xSize / (int)xWidth;
		zTiles = m_zSize / (int)zWidth;
		// Static maximum game size established by the 2D array for the gameTiles.
		gameTiles = new GameObject[xTiles, zTiles];
		// Once game tile dimensions are established, we are ready to GenerateTiles.
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
				yield return StartCoroutine ("DestroyTile");
			int temp = Random.Range (0,100);
			if(temp < recoveryChance)
				yield return StartCoroutine ("RespawnTile");
			
				
		}
		
	}
	
	/* 
	 * Generates tiles with the specified tile dimensions and board size.
	 */
	void GenerateTiles()
	{
		xOffset = (int) xTiles/2;
		zOffset = (int) zTiles/2;
		// Here, the x and z integers refer to the row and column of each tile.
		// Begins counting from half of the maximum xTiles to center on origin.
		for(int x = -xTiles/2; x < xTiles/2; x++)
		{
			// Begins counting from half of the maximum zTiles to center on origin.
			for (int z = -zTiles/2; z < zTiles/2; z++)
			{   
				GameObject tile = Instantiate(m_tileClone, new Vector3(xWidth*x,-10,zWidth*z), transform.rotation) as GameObject;
				GameObject grid = Instantiate(m_grid, new Vector3(xWidth*x,-10,zWidth*z), transform.rotation) as GameObject;
				// Scales the tile to be exactly the size needed for the gameworld.
				tile.transform.localScale = new Vector3(xWidth, 30, zWidth);
				grid.transform.localScale = new Vector3(xWidth - 20, 1, zWidth - 20);
				// Alert the TileState script's method SetValues.
				tile.SendMessage("SetValues", new Vector2(x,z));
				tile.name = "tile x" + x + " z" + z;
				// Because the 2D array cannot consist of negative coordinates, we add
				// half of the number of xTiles.
				int tempX = x + xOffset;
				int tempZ = z + zOffset;
				gameTiles[tempX, tempZ] = tile;
				occupiedLocations.Add(new Vector2(tempX, tempZ));
				totalTiles++;
			}
		}
		// If map 2 is selected, we will RepeatChangeColor
		int map = PlayerPrefs.GetInt("Map");
		if(map == 2)
		{
			StartCoroutine("RepeatChangeColor");
		}
	}
	
	/*
	 * Returns the list of deletedTiles.
	 */
	public List<Vector3> GetDeletedTiles()
	{
		return emptyLocations;
	}
		
	
	/*
	 * Creates a TileMessenger which contains the Vector3 location of a random existing tile.
	 */
	void GetRandomTile(TileMessenger messenger)
	{
		int index = Random.Range (0,occupiedLocations.Count-1);
		int xVal = (int)occupiedLocations[index].x;
		int zVal = (int)occupiedLocations[index].y;
		Vector3 ret = gameTiles[xVal, zVal].transform.position;
		messenger.message = ret;
	}
	
	/*
	 * Sets TileMessenger to contain the float rgb values for the default color.
	 */
	void ReturnColor(TileMessenger messenger)
	{
		Vector3 ret = new Vector3(universalNormColor.r, universalNormColor.g, universalNormColor.b);
		messenger.message = ret;
	}
	
	/*
	 * Destroys a random existing tile.
	 */
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
			deletedTiles.Add(addMe);
			gameTiles[xVal, zVal].SendMessage("DeleteTile");
			totalTiles--;
		}
	}
	
	/*
	* Receives the tile location of a tile to remove from the networking list of deleted tiles.
	*/
	void RemoveFromList(TileMessenger messenger)
	{
		Vector3 addMe = messenger.message;
		emptyLocations.Add(addMe);
	}
	
	/*
	 * Continuously calls a random tile to change colors.
	 */
	IEnumerator RepeatChangeColor()
	{
		yield return StartCoroutine("MyWaitFunction",2.0f); //Brace yourself.
		while(true)
		{
			yield return StartCoroutine("MyWaitFunction",0.05f);
			TileMessenger messenger = new TileMessenger();
			StartCoroutine("GetRandomTile", messenger);
			Vector2 ret = new Vector2(messenger.message.x, messenger.message.y); 
			StartCoroutine("ChangeColor", ret);
		}
	}
	
	/*
	 * Locates the tile at the given coordinates, and changes the tile's color.
	 */
	void ChangeColor(Vector2 coords)
	{
		int xVal = (int)coords.x;
		int zVal = (int)coords.y;
		float r = Random.Range (0.0f,1.0f);
		float g = Random.Range (0.0f,1.0f);
		float b = Random.Range (0.0f,1.0f);
		gameTiles[xVal, zVal].SendMessage("ColorShift", new Color(r,g,b));
	}
	
	/*
	 * Respawns a tile at a vacant location.
	 */
	void RespawnTile()
	{
		Vector3 newLocation = emptyLocations[0];
		emptyLocations.RemoveAt(0); // Remove from the mobile phones list of deleted tiles.
		deletedTiles.RemoveAt(0); // Remove from the local list of deleted tiles.
		Vector3 tempLocation = newLocation;
		tempLocation.Scale (new Vector3(xWidth, 1, zWidth));
		GameObject tile = Instantiate(m_tileClone, tempLocation, transform.rotation) as GameObject;
		tile.transform.localScale = new Vector3(xWidth, 30, zWidth);
		tile.transform.position -= new Vector3(0,10,0); // Elevate tile to standard height.
		tile.name = "tile x" + newLocation.x + " z" + newLocation.z;
		gameTiles[(int)newLocation.x + xOffset, (int)newLocation.z + zOffset] = tile;
		occupiedLocations.Add(new Vector2(newLocation.x + xOffset, newLocation.z + zOffset));
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
	
	public int Rows
	{
		get { return xTiles; }
	}
	
	public int Columns
	{
		get { return zTiles; }
	}
	
	int totalTiles;
	float xWidth;
	float zWidth;
	
	int xTiles; // The number of tiles along the X axis.
	int zTiles; // The number of tiles along the Z axis.
	
	int xOffset;
	int zOffset;
	
	Color universalNormColor;
	
	private GameObject[,] gameTiles;
	private volatile List<Vector3> emptyLocations; // Tiles that are deleted for the mobile phones (For network).
	private volatile List<Vector3> deletedTiles; // Tiles that are falling, or in the process of falling (For game engine).
	private volatile List<Vector2> occupiedLocations;
	
	GameObject m_tileClone;
	
	[SerializeField]
	public GameObject m_tileOne;
	[SerializeField]
	public GameObject m_tileTwo;
	[SerializeField]
	public GameObject m_tileExtreme;
	[SerializeField]
	public GameObject m_grid;
	
	[SerializeField]
	int m_xSize;
	[SerializeField]
	int m_zSize;
}