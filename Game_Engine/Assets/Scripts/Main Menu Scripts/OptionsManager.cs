using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine("SetDifficulty", difficultySetting);
	}
	
	/*
	 * Determine the game difficulty
	 * @param difficulty Int value; 1 = easy, 2 = hard, 3 = extreme
	 */
	void SetDifficulty(int difficulty) 
	{
		PlayerPrefs.SetInt("Difficulty", difficulty);
		PlayerPrefs.Save();
	}
	
	[SerializeField]
	int difficultySetting; // 1 = easy, 2 = hard, 3 = extreme
}
