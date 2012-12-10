using UnityEngine;
using System.Collections;

public class LoadProgress : MonoBehaviour {

	// Use this for initialization
	void Start () {
		progressIndex = 0;
	}
	
	void Awake()
	{
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
		StartCoroutine("Progress");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator Progress()
	{
		while(progressIndex < 64)
		{
			
			yield return StartCoroutine(MyWaitFunction ((float)Random.Range (1,30)/100));
			terrainFactory.SendMessage("ChangeColor", progressIndex);
			terrainFactory.SendMessage("ChangeColor", progressIndex+1);
			progressIndex += 2;
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
	
	int progressIndex;
	GameObject terrainFactory;
}
