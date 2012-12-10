using UnityEngine;
using System.Collections;

public class Fireball : Projectile
{
    protected override void OnProjectileCollided(Collider obj)
    {
        if (audio != null)
            audio.Play();
        obj.SendMessage("Damage", m_damage * (EyeballDamageBuff == true ? 2 : 1));
        if (m_phone != null)
            NetworkInterface.AddPhoneScore(m_phone, m_damage * (EyeballDamageBuff == true ? 2 : 1));
    }

    void OnDestroy()
    {
        if (m_ID != null)
            NetworkInterface.ClearFireball(m_ID);
    }

    public int Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    public string ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    public string PhoneSpawnerID
    {
        get { return m_phone; }
        set { m_phone = value; }
    }

    private string m_ID = null;
    private string m_phone = null;

    [SerializeField]
    private int m_damage;
}
