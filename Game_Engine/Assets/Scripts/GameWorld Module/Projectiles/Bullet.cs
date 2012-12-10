using UnityEngine;
using System.Collections;

public class Bullet : Projectile
{
    protected override void Start()
    {
        base.Start();
        m_id = s_idTracker;
        s_idTracker++;
    }

    protected override void OnProjectileCollided(Collider obj)
    {
        if (audio != null)
            audio.Play();
        obj.SendMessage("Damage", m_damage * (EyeballDamageBuff == true ? 2 : 1));
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Towers"))
        {
            TowerObject script = tower.GetComponent<TowerObject>();
            if (script != null)
            {
                if (script.OwnerID != null)
                    NetworkInterface.AddPhoneScore(script.OwnerID, m_damage * (EyeballDamageBuff == true ? 2 : 1));
            }
        }
    }

    void OnDestroy()
    {
        NetworkInterface.ClearBullet(m_id.ToString());
    }

    public int Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    public int ID
    {
        get { return m_id; }
    }

    [SerializeField]
    private int m_damage;

    private int m_id;

    private static int s_idTracker;
}
