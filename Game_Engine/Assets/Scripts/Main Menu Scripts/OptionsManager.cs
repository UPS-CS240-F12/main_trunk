using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine("SetDifficulty", mapSetting);
	}
	
	/*
	 * Determine the game difficulty
	 * @param difficulty Int value; 1 = easy, 2 = hard, 3 = extreme
	 */
	void SetDifficulty(int map) 
	{
		PlayerPrefs.SetInt("Map", map);
		PlayerPrefs.Save();
	}
	
	[SerializeField]
	int mapSetting; // An integer between 1 and 3 corresponding to gameboards.
}
