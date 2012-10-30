using UnityEngine;
using System.Collections;

public class TileDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	void Awake ()
	{
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void DestroyRandom()
	{
		TileMessenger messenger = new TileMessenger();
		terrainFactory.SendMessage("GetRandomTile", messenger);
	}
	
	GameObject terrainFactory;
}
