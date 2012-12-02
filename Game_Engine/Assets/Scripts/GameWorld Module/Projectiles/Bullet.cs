using UnityEngine;
using System.Collections;

public class Bullet : Projectile
{
    protected override void OnProjectileCollided(Collider obj)
    {
        audio.Play ();
		obj.SendMessage("Damage", m_damage);
    }

    public int Damage
    {
        get { return m_damage;}
        set { m_damage = value; }
    }

    [SerializeField]
    private int m_damage;
}
