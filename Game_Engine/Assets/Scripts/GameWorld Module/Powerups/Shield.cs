using UnityEngine;
using System.Collections;

public class Shield : Powerup
{
    protected override void Start()
	{
		base.Start();
		m_pointKeeper = (GameObject) GameObject.FindGameObjectWithTag("PointKeeper");
	}
	
    protected override void OnPowerupReceived(Collider player)
    {
        audio.Play();
        player.SendMessage("AddShield");
		m_pointKeeper.SendMessage("AddPoints", m_pointValue);
    }

    public float ActiveTime
    {
        get { return m_activeTime; }
        set { m_activeTime = value; }
    }

	private GameObject m_pointKeeper;
	
	[SerializeField]
	private int m_pointValue;
	[SerializeField]
	private float m_activeTime;
	[SerializeField]
	string m_displayMessage;
}
