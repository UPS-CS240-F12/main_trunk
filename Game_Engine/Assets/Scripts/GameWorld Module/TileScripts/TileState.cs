using UnityEngine; 
using System.Collections; 

public class TileState : MonoBehaviour { 
    bool despawn = false;
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
			this.gameObject.renderer.material.color = Color.yellow;
			normColor = Color.yellow;
        	yield return StartCoroutine(MyWaitFunction (5.0f));
			this.gameObject.renderer.material.color = Color.red;
			normColor = Color.red;
			StartCoroutine(FallSequence(true));
			despawn = true;
			yield return StartCoroutine(MyWaitFunction (5.0f));
			//magnitude = initialMagnitude;
			Destroy(this.gameObject);
		}
    }
	
	void Update()
	{
		if(despawn)
		{
			//transform.position += new Vector3(0,-0.5f,0);
		}
		//magnitude *= 1.01f;
	}
	
	IEnumerator FallSequence(bool destroy)
	{
		float xRotation = Random.Range(-1.0f, 0.5f);
		float yRotation = Random.Range(-1.0f, 0.5f);
		float zRotation = Random.Range(-1.0f, 0.5f);
		float fallSpeed;
		if(destroy)
		{
			fallSpeed = Random.Range (0.0f, 0.5f);
		}
		else
		{
			fallSpeed = 0.0f;
		}
		
		for(int i = 360; i > 0; i--)
		{
			transform.Rotate(xRotation,yRotation,zRotation);
			if(destroy)
			{
				transform.position -= new Vector3(0.0f, fallSpeed, 0.0f);
				fallSpeed += 0.1f;
			}
			yield return StartCoroutine(MyWaitFunction (0.05f));
		}
	}
	
	void OnMouseDown()
	{
		remove = false;
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