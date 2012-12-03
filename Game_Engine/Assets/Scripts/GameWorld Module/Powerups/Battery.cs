using UnityEngine;
using System.Collections;

public class Battery : Powerup
{
	protected virtual void Start()
	{
		base.Start();
		m_pointKeeper = (GameObject) GameObject.FindGameObjectWithTag("PointKeeper");
	}
	
    protected override void OnPowerupReceived(Collider player)
    {
		audio.Play ();
        player.SendMessage("AddEnergy", m_energyGain);
		m_pointKeeper.SendMessage("AddPoints", m_pointValue);
    }

    public int EnergyGain
    {
        get { return m_energyGain; }
        set { m_energyGain = value; }
    }

    [SerializeField]
    private int m_energyGain;
	
	private GameObject m_pointKeeper;
	
	[SerializeField]
	int m_pointValue;
	[SerializeField]
	string m_displayMessage;
}
