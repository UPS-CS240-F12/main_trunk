using UnityEngine; 
using System.Collections; 

public class MainMenuButton : MonoBehaviour { 
	Vector3 offsetPosition;
	Vector3 offsetStep;
	
	void Start()
	{
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
		int curStep = 0; // Total of 10 steps
		while(curStep <= 10)
		{
			transform.position-= offsetStep;
			yield return StartCoroutine(MyWaitFunction (0.01f));
			curStep++;
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