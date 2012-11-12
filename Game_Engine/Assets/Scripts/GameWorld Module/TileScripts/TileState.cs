using UnityEngine; 
using System.Collections; 

public class TileState : MonoBehaviour { 
    bool rotating = false;
	bool remove = true;
	//float initialMagnitude;
	Color normColor = Color.white;
	float life;
	GameObject terrainFactory;
	
	void Awake()
	{
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
	}
		
	IEnumerator Start () 
	{
		//float initialMagnitude = magnitude;
		life = Random.Range(lowerRange, upperRange);
        yield return StartCoroutine(MyWaitFunction (life));
		if(remove)
		{
			terrainFactory.SendMessage ("RemoveTile", name);
			int difficulty = PlayerPrefs.GetInt("Difficulty");
			this.gameObject.renderer.material.color = Color.yellow;
			normColor = Color.yellow;
        	yield return StartCoroutine(MyWaitFunction (3.0f / difficulty));
			this.gameObject.renderer.material.color = Color.red;
			normColor = Color.red;
			StartCoroutine(FallSequence());
			//Begin rotation at random, not 360 degree flip.
			StartCoroutine(RotateSequence(false, 1.0f));
			yield return StartCoroutine(MyWaitFunction (5.0f));
			Destroy(this.gameObject);
		}
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
	
	void OnMouseDown()
	{
		remove = false;
		StartCoroutine(RotateSequence(true, 18.0f));
		normColor = new Color(0.2f,0.9f,0);
		this.gameObject.renderer.material.color = normColor;
	}

    void OnMouseEnter()
    {
		this.gameObject.renderer.material.color = new Color(0,0,1);
    }
	
	IEnumerator OnMouseExit()
	{
		float newColor = 0.0f;
		while (newColor < 1.0f)
		{
			this.gameObject.renderer.material.color = new Color(newColor,newColor,1);
			yield return StartCoroutine(MyWaitFunction (0.05f));
			newColor += 0.1f;
		}
		this.gameObject.renderer.material.color = normColor;
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