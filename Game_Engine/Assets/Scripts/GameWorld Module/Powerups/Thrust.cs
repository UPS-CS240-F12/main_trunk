using UnityEngine;
using System.Collections;

public class Thrust : Powerup
{
	protected virtual void Start()
	{
		base.Start();
		m_pointKeeper = (GameObject) GameObject.FindGameObjectWithTag("PointKeeper");
	}
	
    protected override void OnPowerupReceived(Collider player)
    {
        player.SendMessage("AddThrust");
		m_pointKeeper.SendMessage("AddPoints", m_pointValue);
    }

    [SerializeField]
    private int m_energyGain;
	
	private GameObject m_pointKeeper;
	
	[SerializeField]
	private int m_pointValue;
	[SerializeField]
	string m_displayMessage;
}