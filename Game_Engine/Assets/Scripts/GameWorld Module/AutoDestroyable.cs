using UnityEngine;
using System.Collections;

public class AutoDestroyable : MonoBehaviour
{
    // Use this for initialization
    protected virtual void Start()
    {
        if (m_keepAlive == false)
            Destroy(gameObject, m_lifetimeMillis / 1000.0f);
    }

    public bool KeepAlive
    {
        get { return m_keepAlive; }
    }

    public int LifetimeMillis
    {
        get { return m_lifetimeMillis; }
    }

    [SerializeField]
    private bool m_keepAlive = false;
    [SerializeField]
    private int m_lifetimeMillis = 0;
}
