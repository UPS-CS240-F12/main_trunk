using UnityEngine;
using System.Collections;

public abstract class Projectile : AutoDestroyable
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_direction.Normalize();
        GetRigidbody().velocity = (m_direction * m_speed);
    }

    // Called if we collide with something else
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
            OnProjectileCollided(other);

        Destroy(gameObject);
    }

    protected abstract void OnProjectileCollided(Collider obj);

    public void SetVelocity(Vector3 direction, float speed)
    {
        m_direction = direction.normalized;
        m_speed = speed;
        GetRigidbody().velocity = (m_direction * m_speed);
    }

    public float Speed
    {
        get { return m_speed; }
        set
        {
            m_speed = value;
            GetRigidbody().velocity = (m_direction * m_speed);
        }
    }

    public Vector3 Direction
    {
        get { return m_direction; }
        set
        {
            m_direction = value;
            m_direction.Normalize();

            GetRigidbody().velocity = (m_direction * m_speed);
        }
    }

    private Rigidbody GetRigidbody()
    {
        return GetComponent<Rigidbody>();
    }

    [SerializeField]
    private float m_speed;
    [SerializeField]
    private Vector3 m_direction;

    public static bool EyeballDamageBuff = false;
}
