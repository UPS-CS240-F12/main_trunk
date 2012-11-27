using UnityEngine;
using System.Collections;

public class Fireball : Projectile
{
    protected override void OnProjectileCollided(Collider obj)
    {
        obj.SendMessage("Damage", m_damage);
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

    private string m_ID = null;

    [SerializeField]
    private int m_damage;
}
