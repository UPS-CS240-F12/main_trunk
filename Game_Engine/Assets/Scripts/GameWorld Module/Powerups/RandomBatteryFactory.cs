using UnityEngine;
using System.Collections;

public class RandomBatteryFactory : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		StartCoroutine("CreateRandomBatteries");
	}
			
	private IEnumerator CreateRandomBatteries()
	{
		System.Random rand = new System.Random();
		while (m_continue == true)
		{
			int waitTime = rand.Next(m_minimumTimeMillis, m_maximumTimeMillis);
			yield return new WaitForSeconds(waitTime / 1000.0f);
			
			float x = m_boundaries.x + ((float) rand.NextDouble() * m_boundaries.width);
			float z = m_boundaries.y + ((float) rand.NextDouble() * m_boundaries.height);
            Instantiate(m_batteryClone, new Vector3(x, 30, z), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update()
	{

	}
	
	public void Stop()
	{
		m_continue = false;
	}
	
	public int MinimumTimeMillis
	{
		get { return m_minimumTimeMillis; }
	}
	
	public int MaximumTimeMillis
	{
		get { return m_maximumTimeMillis; }
	}
	
	[SerializeField]
	private int m_minimumTimeMillis;
	[SerializeField]
	private int m_maximumTimeMillis;
	[SerializeField]
	private Rect m_boundaries;
		
	private bool m_continue = true;
	[SerializeField]
	private GameObject m_batteryClone;
}
