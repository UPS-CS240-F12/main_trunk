using UnityEngine; 
using System.Collections; 

public class TileState : MonoBehaviour { 
	
	/*
	* Initiated once the game is ready, at which point we will find the TerrainFactory.
	*/
	void Awake()
	{
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
	}
	
	/*
	* Allows for hte tile to be controlled to rotate from outside sources. Magnitude refers
	* to the speed at which it will rotate, with higher magnitudes being faster.
	*/
	void CommandRotate(float magnitude)
	{
		StartCoroutine(RotateSequence(true, magnitude));
	}
	
	/*
	* Rotates the tile. If circle is true, it will complete 360 degrees. If not it will rotate
	* until the tile is deleted. The magnitude refers to the speed of rotation
	*/
	public IEnumerator RotateSequence(bool circle, float magnitude)
	{
		float xRotation;
		float yRotation;
		float zRotation;
		
		if(circle)
		{
			xRotation = 1.0f * magnitude;
			yRotation = 0.0f * magnitude;
			zRotation = 0.0f * magnitude;
		}
		else
		{
			xRotation = Random.Range(-1.0f, 0.5f) * magnitude;
			yRotation = Random.Range(-1.0f, 0.5f) * magnitude;
			zRotation = Random.Range(-1.0f, 0.5f) * magnitude;
		}
		for(int i = 180 / (int)magnitude; i > 0; i--)
		{
			transform.Rotate(xRotation,yRotation,zRotation);
			yield return StartCoroutine(MyWaitFunction (0.05f));
		}
		this.gameObject.renderer.material.color = normColor;
	}
	
	/*
	* Moves the tile downwards before it despawns.
	*/
	IEnumerator FallSequence()
	{
		
		float fallSpeed = Random.Range (0.0f, 0.5f);
		for(int i = 360; i > 0; i--)
		{
				transform.position -= new Vector3(0.0f, fallSpeed, 0.0f);
				fallSpeed += 0.2f;
			yield return StartCoroutine(MyWaitFunction (0.05f));
		}
	}
	
	/*
	* Procedure for deleting the tile. Begins with color changing, then commands to fall and delete.
	*/
	IEnumerator DeleteTile()
	{
		this.gameObject.renderer.material.color = Color.yellow;
		normColor = Color.yellow;
       	yield return StartCoroutine(MyWaitFunction (1.0f));
		// Flashes between black and red to notify the player that the tile is falling.
		int counter = 40;
		bool flag = false;
		float shade = 1.0f;
		while(counter > 0)
		{
			yield return StartCoroutine (MyWaitFunction (0.05f));
			this.gameObject.renderer.material.color = new Color(shade, 0.0f, 0.0f);
			if(shade > 0.9f)
				flag = true;
			if(shade < 0.1f)
				flag = false;
			if(flag)
				shade -= 0.2f;
			else
				shade += 0.2f;
			counter--;
		}
		this.gameObject.renderer.material.color = Color.red;
		normColor = Color.red;
		StartCoroutine(FallSequence());
		//Begin rotation at random, not 360 degree flip.
		StartCoroutine(RotateSequence(false, 1.0f));
		// Create a message to remove this tile to remove it from the mobile phones' view.
		TileMessenger messenger = new TileMessenger();
		messenger.message = new Vector3(xVal, 0, zVal);
		terrainFactory.SendMessage("RemoveFromList", messenger);
		yield return StartCoroutine(MyWaitFunction (5.0f));
		Destroy(this.gameObject);
	}
	
	/*
	* Sets the tile's coordinate values.
	*/
	void SetValues(Vector2 vals)
	{
		xVal = (int)vals.x;
		zVal = (int)vals.y;
	}
	
	/*
	* Takes the given TileMessenger and sets the message as the tile's location.
	*/
	void SendLocation(TileMessenger messenger)
	{
		Vector3 ret = new Vector3(xVal,0,zVal);
		messenger.message = ret;
	}
	
	/*
	* Changes the default color of the tile.
	*/
	void ColorShift(Color newColor)
	{
		this.gameObject.renderer.material.color = newColor;
		normColor = newColor;
	}
	
	/* 
	* Wait function.
	*/
    IEnumerator MyWaitFunction (float delay) 
	{
        float timer = Time.time + delay;
        while (Time.time < timer) 
		{
            yield return null;
        }
    }

	Color normColor = Color.white;
	GameObject terrainFactory;
	
	// The column and row values for this tile.
	int xVal;
	int zVal;
}