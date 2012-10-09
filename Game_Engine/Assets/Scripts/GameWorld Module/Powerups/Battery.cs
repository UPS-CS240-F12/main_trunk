using UnityEngine;
using System.Collections;

public class Battery : Powerup
{
    protected override void OnPowerupReceived(Collider player)
    {
        player.SendMessage("AddEnergy", m_energyGain);
    }

    public int EnergyGain
    {
        get { return m_energyGain; }
        set { m_energyGain = value; }
    }

    [SerializeField]
    private int m_energyGain;
}
