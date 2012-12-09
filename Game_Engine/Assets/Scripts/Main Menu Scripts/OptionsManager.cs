using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

	void Start () 
	{
		StartCoroutine("SetDifficulty", mapSetting);
	}
	
	/*
	 * Determines which map the game will be played on.
	 * Map refers to the map number to play on. 1 = default, 2 = other.
	 */
	void SetDifficulty(int map) 
	{
		PlayerPrefs.SetInt("Map", map);
		PlayerPrefs.Save();
	}
	
	[SerializeField]
	int mapSetting; // Should be an integer value, 1 or 2.
}
