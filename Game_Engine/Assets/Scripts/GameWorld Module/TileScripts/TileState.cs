using UnityEngine; 
using System.Collections; 

public class TileState : MonoBehaviour { 
    bool rotating = false;
	bool remove = true;
	//float initialMagnitude;
	Color normColor = Color.white;
	float life;
	GameObject terrainFactory;
	int xVal;
	int zVal;
	
	void Awake()
	{
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
		/*TileMessenger colorMessage = new TileMessenger();
		terrainFactory.SendMessage ("ReturnColor", colorMessage);
		Vector3 temp = colorMessage.message;
		this.gameObject.renderer.material.color = new Color (temp.x, temp.y, temp.z);*/
	}
		
	void Start () 
	{
    }
	
	void Update()
	{
	}
	
	void CommandRotate(float magnitude)
	{
		this.gameObject.renderer.material.color = Color.red;
		StartCoroutine(RotateSequence(true, magnitude));
	}
	
	IEnumerator RespawnMovement()
	{
		float moveDistance = -25.0f;
		for(int i = 0; i < 20; i++)
		{
			transform.position += new Vector3(0.0f,moveDistance,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.05f));
		}
	}
	
	public IEnumerator RotateSequence(bool circle, float magnitude)
	{
		rotating = true;
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
		rotating = false;
		this.gameObject.renderer.material.color = normColor;
	}
	
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
	
	IEnumerator DeleteTile()
	{
		this.gameObject.renderer.material.color = Color.yellow;
		normColor = Color.yellow;
       	yield return StartCoroutine(MyWaitFunction (1.0f));
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
		yield return StartCoroutine(MyWaitFunction (5.0f));
		Destroy(this.gameObject);
	}
	
	/* Temporary method for safe zones. */
	void OnMouseDown()
	{
		remove = false;
		StartCoroutine(RotateSequence(true, 18.0f));
		normColor = new Color(0.2f,0.9f,0);
		this.gameObject.renderer.material.color = normColor;
	}
	
	void SetValues(Vector2 vals)
	{
		xVal = (int)vals.x;
		zVal = (int)vals.y;
	}
	
	void SendLocation(TileMessenger messenger)
	{
		Vector3 ret = new Vector3(xVal,0,zVal);
		messenger.message = ret;
	}
	
	void ColorShift(Color newColor)
	{
		this.gameObject.renderer.material.color = newColor;
		normColor = newColor;
	}
	
    IEnumerator MyWaitFunction (float delay) 
	{
        float timer = Time.time + delay;
        while (Time.time < timer) 
		{
            yield return null;
        }
    }
	
	[SerializeField]
	float lowerRange;
	[SerializeField]
	float upperRange;
	
}