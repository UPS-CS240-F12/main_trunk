using UnityEngine;
using System.Collections;

public class RotateMe : MonoBehaviour {

	 //Use this for initialization
	void Start ()
	{
	}
	
	 //Update is called once per frame
	void Update () 
	{
		
	}
	
	public IEnumerator RotateRight() 
	{
		float yVal = 0.2f;
		for(int i = 0; i < 28; i++)
		{
			transform.Rotate(0.0f,yVal,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.02f));
			yVal += 0.101f;
		}
		for(int i = 0; i < 29; i++)
		{
			transform.Rotate(0.0f,yVal,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.02f));
			yVal -= 0.1f;
		}
	}
	
	public IEnumerator RotateLeft() 
	{
		float yVal = -0.2f;
		for(int i = 0; i < 28; i++)
		{
			transform.Rotate(0.0f,yVal,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.02f));
			yVal -= 0.101f;
		}
		for(int i = 0; i < 29; i++)
		{
			transform.Rotate(0.0f,yVal,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.02f));
			yVal += 0.1f;
		}
	}
	
  	IEnumerator MyWaitFunction (float delay) 
	{
       float timer = Time.time + delay;
        while (Time.time < timer) 
		{
            yield return null;
        }
    }
}
