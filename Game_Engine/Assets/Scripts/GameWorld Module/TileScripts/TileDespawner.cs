using UnityEngine; 
using System.Collections; 

public class TileDespawner : MonoBehaviour { 
    bool despawn = false;
	//float initialMagnitude;
	Color normColor = Color.white;
	
	IEnumerator Start () 
	{
		//float initialMagnitude = magnitude;
		float life = Random.Range(10.0F, 250.0F);
        yield return StartCoroutine(MyWaitFunction (life));
        print ("Block has reached Half-Life.");
		this.gameObject.renderer.material.color = Color.yellow;
		normColor = Color.yellow;
        yield return StartCoroutine(MyWaitFunction (5.0f));
        print ("Block has despawned!");
		this.gameObject.renderer.material.color = Color.red;
		normColor = Color.red;
		despawn = true;
		yield return StartCoroutine(MyWaitFunction (5.0f));
		//magnitude = initialMagnitude;
		Destroy(this.gameObject);
    }
	
	void Update()
	{
		if(despawn)
		{
			transform.position += new Vector3(0,-0.5f,0);
		}
		//magnitude *= 1.01f;
	}
	
	// Called if we collide with something else
    IEnumerator OnMouseEnter()
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