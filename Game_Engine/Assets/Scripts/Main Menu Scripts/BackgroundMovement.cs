using UnityEngine;
using System.Collections;

public class BackgroundMovement : MonoBehaviour {
	Vector3 offset;
	Vector3 maxHeight;
	int counter;
	bool flag;
	
	void Start () 
	{
		flag = true;
		maxHeight = new Vector3 (maxX, maxY, maxZ);
		offset = new Vector3 (maxX/200.0f, 
				maxY/200.0f, maxZ/200.0f);
		counter = 200;
		StartCoroutine(Movement());
	}
	
	IEnumerator Movement () 
	{
		float moveMagnitude = 0.0f;
		while(flag)
		{
			while(counter > 0)
			{
				transform.position -= Vector3.Scale(offset, new Vector3(moveMagnitude, moveMagnitude, moveMagnitude));
				yield return StartCoroutine(MyWaitFunction (0.01f));
				counter --;
				if (counter > 100)
					moveMagnitude += 0.02f;
				else
					moveMagnitude -= 0.02f;
			}
			Debug.Log(moveMagnitude);
			while(counter < 200)
			{
				transform.position += Vector3.Scale(offset, new Vector3(moveMagnitude, moveMagnitude, moveMagnitude));
				yield return StartCoroutine(MyWaitFunction (0.01f));
				counter++;
				if (counter > 101)
					moveMagnitude -= 0.02f;
				else
					moveMagnitude += 0.02f;
			}
		}
	}
	
	IEnumerator MyWaitFunction (float delay) 
	{
        float timer = Time.time + delay;
        while (Time.time < timer) 
            yield return null;
    }
	
	[SerializeField]
	float maxX;
	[SerializeField]
	float maxY;
	[SerializeField]
	float maxZ;
}
