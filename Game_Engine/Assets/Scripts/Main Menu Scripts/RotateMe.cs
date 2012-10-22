using UnityEngine;
using System.Collections;

public class RotateMe : MonoBehaviour {

	 //Use this for initialization
	void Start () {
		Debug.Log ("cameraRotate");
	}
	
	 //Update is called once per frame
	void Update () {
		
	}
	
	public IEnumerator RotateRight() {
		float yVal = 1.0f;
		for(int i=0; i< 90; i++){
			transform.Rotate(0.0f,yVal,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.01f));
		}
	}
	
	public IEnumerator RotateLeft() {
		float yVal = -1.0f;
		for(int i=0; i< 90; i++){
			transform.Rotate(0.0f,yVal,0.0f);
			yield return StartCoroutine(MyWaitFunction (0.01f));
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
