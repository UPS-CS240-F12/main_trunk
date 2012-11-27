using UnityEngine;
using System.Collections;

public class PointKeeper : MonoBehaviour {

	int points;
	int rank;
	bool gain = false;
	string gainCause;
	
	// Use this for initialization
	void Start () 
	{
		points = 0;
		rank = 0;
		StartCoroutine("PointGainRoutine");
		gainCause = "\nYour rank is currently " + rank;
	}

    private IEnumerator PointGainRoutine()
    {
        while(true)
        {
            yield return StartCoroutine(MyWaitFunction (1.0f/ m_pointGainRate));
            points++;
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	void LateUpdate()
	{
		if(!gain)
		{
		this.guiText.text = "Points: " + points + gainCause;
		}
		else
		{
			this.guiText.text = "Points: " + points + gainCause;
		}
	}
	
	void EndGame()
	{
		Application.LoadLevel("EndGameScene");
	}
	
	IEnumerator AddPoints(int newPoints)
	{
		points += newPoints;
		gainCause = "\nYou gained " + newPoints + " points!";
		yield return StartCoroutine(MyWaitFunction (3.0f));
		gain = true;
		gainCause = "\nYour rank is currently + rank";
	}
	
	IEnumerator MyWaitFunction (float delay) 
	{
        float timer = Time.time + delay;
        while (Time.time < timer) 
            yield return null;
    }
	
	[SerializeField]
	int energyNeeded;
	[SerializeField]
	int m_pointGainRate;
}
