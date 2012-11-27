using UnityEngine;
using System.Collections;

public class MobileCamera : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("RandomFireballShooter");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    private IEnumerator RandomFireballShooter()
    {
        System.Random rand = new System.Random();
        while (true)
        {
            int waitTimeMs = rand.Next(5000, 10000);
            yield return new WaitForSeconds(waitTimeMs / 1000.0f);
            ShootFireball();
        }
    }

    public void ShootFireball()
    {
        GameObject fireball = Instantiate(m_fireballClone, transform.position, transform.rotation) as GameObject;
        fireball.GetComponent<Projectile>().Direction = transform.TransformDirection(Vector3.forward);
    }

    public string DeviceID
    {
        get { return m_deviceID; }
        set { m_deviceID = value; }
    }

    private string m_deviceID = null;

    [SerializeField]
    private GameObject m_fireballClone;
}
