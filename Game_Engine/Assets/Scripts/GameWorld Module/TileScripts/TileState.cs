using UnityEngine; 
using System.Collections; 

public class TileState : MonoBehaviour { 
    bool despawn = false;
	bool remove = true;
	//float initialMagnitude;
	Color normColor = Color.white;
	float life;
	
	IEnumerator Start () 
	{
		//float initialMagnitude = magnitude;
		life = Random.Range(lowerRange, upperRange);
        yield return StartCoroutine(MyWaitFunction (life));
		if(remove)
		{
			this.gameObject.renderer.material.color = Color.yellow;
			normColor = Color.yellow;
        	yield return StartCoroutine(MyWaitFunction (5.0f));
			this.gameObject.renderer.material.color = Color.red;
			normColor = Color.red;
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
			transform.position += new Vector3(0,-0.5f,0);
		}
		//magnitude *= 1.01f;
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