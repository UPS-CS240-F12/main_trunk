using UnityEngine;
using System.Collections;

public class Battery : Powerup
{
    protected override void Start()
    {
        m_id = s_idTracker;
        s_idTracker++;
        base.Start();
        m_pointKeeper = (GameObject)GameObject.FindGameObjectWithTag("PointKeeper");
    }

    protected override void OnPowerupReceived(Collider player)
    {
        if (audio != null)
            audio.Play();
        player.SendMessage("AddEnergy", m_energyGain);
        m_pointKeeper.SendMessage("AddPoints", m_pointValue);
    }

    void OnDestroy()
    {
        NetworkInterface.ClearBattery(m_id.ToString());
    }

    public int EnergyGain
    {
        get { return m_energyGain; }
        set { m_energyGain = value; }
    }

    public int ID
    {
        get { return m_id; }
    }

    [SerializeField]
    private int m_energyGain;

    private GameObject m_pointKeeper;

    [SerializeField]
    int m_pointValue;
    [SerializeField]
    string m_displayMessage;

    private int m_id;

    private static int s_idTracker = 0;
}
