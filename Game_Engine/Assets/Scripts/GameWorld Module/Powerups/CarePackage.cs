using UnityEngine;
using System.Collections;

public class CarePackage : Powerup
{
    protected override void Start()
	{
		base.Start();
		m_pointKeeper = (GameObject) GameObject.FindGameObjectWithTag("PointKeeper");
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
	}
	
    protected override void OnPowerupReceived(Collider player)
    {
		audio.Play ();
		m_pointKeeper.SendMessage("AddPoints", m_pointValue);
		int counter = m_respawnNumber;
		while(counter > 0)
		{
			terrainFactory.SendMessage("RespawnTile");
			counter--;
		}
    }

    [SerializeField]
    private int m_energyGain;
	
	private GameObject m_pointKeeper;
	private GameObject terrainFactory;
	
	[SerializeField]
	private int m_pointValue;
	[SerializeField]
	private int m_respawnNumber;
	[SerializeField]
	string m_displayMessage;
}