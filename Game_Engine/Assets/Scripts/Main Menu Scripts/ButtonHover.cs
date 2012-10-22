using UnityEngine; 
using System.Collections; 

public class ButtonHover : MonoBehaviour { 
	Vector3 defaultLocation;
	Vector3 offsetPosition;
	Vector3 offsetStep;
	float displacement;
	
	void Start()
	{
		defaultLocation = transform.position;
		offsetStep = new Vector3(offsetX/10, offsetY/10, offsetZ/10);
	}
	
	// Called if we collide with something else
    IEnumerator OnMouseEnter()
    {
		int curStep = 0; // Total of 10 steps
		while (curStep <= 10)
		{
			transform.position += offsetStep;
			yield return StartCoroutine(MyWaitFunction (0.01f));
			curStep++;
		}
    }
	
	IEnumerator OnMouseExit()
	{
		int curStep = 0; // Total of 12 steps
		while(curStep <= 15)
		{
			transform.position-= offsetStep;
			yield return StartCoroutine(MyWaitFunction (0.01f));
			curStep++;
		}
		int counter = 5;
		while (counter > 0)
		{
			transform.position+= offsetStep;
			yield return StartCoroutine(MyWaitFunction (0.01f));
			counter--;
		}
	}
	
    IEnumerator MyWaitFunction (float delay) 
	{
        float timer = Time.time + delay;
        while (Time.time < timer) 
            yield return null;
    }
	
	[SerializeField]
	float offsetX;
	[SerializeField]
	float offsetY;
	[SerializeField]
	float offsetZ;
}