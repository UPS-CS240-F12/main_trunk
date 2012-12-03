using UnityEngine;
using System.Collections;

public class GameOverDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		pointPreserver = GameObject.FindGameObjectWithTag("PointPreserver");
		PointMessenger messenger = new PointMessenger();
		pointPreserver.SendMessage("SendResults", messenger);
		int finalScore = messenger.score;
		bool finalWinResult = messenger.win;
		pointResults.text = "Points: " + finalScore;
		if(finalWinResult == true)
		{
			winResults.text = "You won! You got an additional 5,000 points.";
			pointResults.text = "Points: " + (finalScore + 5000);
		}
		else
		{
			winResults.text = "You lost! Better luck next time";
		}
		Destroy(pointPreserver);
	}
	
	GameObject pointPreserver;
	
	[SerializeField]
	GUIText pointResults;
	[SerializeField]
	GUIText winResults;
}
