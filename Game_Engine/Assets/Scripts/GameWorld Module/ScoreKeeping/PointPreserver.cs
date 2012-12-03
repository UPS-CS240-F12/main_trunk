using UnityEngine;
using System.Collections;

public class PointPreserver : MonoBehaviour {
	void Start () {
		
	}
	
	void WinStatus(bool winBool)
	{
		win = winBool; // True means the character won.
	}
	
	void CumulateScore(int points)
	{
		score = points;
	}
	
	void SendResults(PointMessenger messenger)
	{
		messenger.score = this.score;
		messenger.win = this.win;
	}
	
	public int score;
	public bool win;
}
