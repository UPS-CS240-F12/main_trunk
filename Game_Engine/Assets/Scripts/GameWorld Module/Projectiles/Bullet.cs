using UnityEngine;
using System.Collections;

public class Bullet : Projectile
{
    protected override void OnProjectileCollided(Collider obj)
    {
        obj.SendMessage("Damage", m_damage);
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Towers"))
        {
            TowerObject script = tower.GetComponent<TowerObject>();
            if (script != null)
            {
                if (script.OwnerID != null)
                    NetworkInterface.AddPhoneScore(script.OwnerID, m_damage);
            }
        }
    }

    public int Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    [SerializeField]
    private int m_damage;
}
