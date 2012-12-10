using UnityEngine; 
using System.Collections; 

public class ButtonHover : MonoBehaviour { 
	bool hovering;
	Vector3 offsetPosition;
	Vector3 offsetStep;
	float displacement;
	ButtonPush pushMe;
	
	void Start()
	{
		if(pos == 0)
		{
			hovering = true;
		}
		else
		{
			hovering = false;
		}
		offsetStep = new Vector3(offsetX/10, offsetY/10, offsetZ/10);
		pushMe = GetComponent<ButtonPush>();
	}
	
	// Called when we want to begin hovering over a button.
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
	
	void Update()
	{
		if(Input.GetButtonDown("Jump") && renderer.isVisible)
		{
			if(!hovering && pos == 0)
			{
				pos = maxPos;
				StartCoroutine ("OnMouseEnter");
				hovering = true;
			}
			else if(hovering)
			{
				StartCoroutine("OnMouseExit");
				hovering = false;
				//WaitForClick
			}
			else if(pos > 0)
			{
				pos--;
			}
		}
		//If Enter pressed, we will call this button's buttonPush
		if(Input.GetButtonDown("Fire3") && hovering && renderer.isVisible)
		{
			pushMe.SendMessage("setClick");
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
	
	[SerializeField]
	int pos; // 0 for origin, 1 for next button, 2 for next, etc.
	[SerializeField]
	int maxPos; // The total number of buttons in the page (zero inclusive).
}