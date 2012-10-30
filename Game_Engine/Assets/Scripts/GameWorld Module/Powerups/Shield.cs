using UnityEngine;
using System.Collections;

public class Shield : Powerup
{
	void Start()
	{
		m_pointKeeper = (GameObject) GameObject.FindGameObjectWithTag("PointKeeper");
	}
	
    protected override void OnPowerupReceived(Collider player)
    {
        player.SendMessage("AddShield", m_activeTime);
		m_pointKeeper.SendMessage("AddPoints", m_pointValue);
    }

    [SerializeField]
    private int m_energyGain;
	
	private GameObject m_pointKeeper;
	
	[SerializeField]
	private int m_pointValue;
	[SerializeField]
	private float m_activeTime;
	[SerializeField]
	string m_displayMessage;
}
