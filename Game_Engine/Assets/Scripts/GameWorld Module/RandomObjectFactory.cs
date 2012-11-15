using UnityEngine;
using System.Collections;

public class RandomObjectFactory : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Resume();
    }
	
	void Awake()
	{
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
	}

    private IEnumerator CreateRandomObjects()
    {
        System.Random rand = new System.Random();
        float x, z;
        int waitTime;

        while (m_continue == true)
        {
            waitTime = rand.Next(m_minimumTimeMillis, m_maximumTimeMillis);
            yield return new WaitForSeconds(waitTime / 1000.0f);

            //x = m_boundaries.x + ((float)rand.NextDouble() * m_boundaries.width);
            //z = m_boundaries.y + ((float)rand.NextDouble() * m_boundaries.height);
			TileMessenger messenger = new TileMessenger();
			terrainFactory.SendMessage("GetRandomTile", messenger);
			Instantiate(m_objectClone, messenger.message + new Vector3(0,45,0), Quaternion.identity);
            //Instantiate(m_objectClone, new Vector3(x, m_yOffset, z), Quaternion.identity);
        }

        lock (m_lock)
        {
            m_running = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        lock (m_lock)
        {
            if (m_running == true)
                return;

            m_running = true;
        }

        m_continue = true;
        StartCoroutine("CreateRandomObjects");
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
	
	GameObject terrainFactory;
	
    [SerializeField]
    protected int m_minimumTimeMillis;
    [SerializeField]
    protected int m_maximumTimeMillis;
   // [SerializeField]
   // protected Rect m_boundaries;
    [SerializeField]
    protected int m_yOffset;

    private bool m_continue = true;
    private bool m_running = false;
    private Object m_lock = new Object();

    [SerializeField]
    protected GameObject m_objectClone;
}
