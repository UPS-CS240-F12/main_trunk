using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

	void Start () 
	{
		int xWidth = m_tileDimensions;
		int zWidth = m_tileDimensions;
		int x = 0; 
		int temp = 0;
		while(x < m_xSize)
		{
			for (int z = 0; z < m_zSize; z += zWidth)
			{    
				GameObject tile = Instantiate (m_tileClone, new Vector3(x,-10-temp,z), 
						transform.rotation) as GameObject;
			}
			x += xWidth;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	[SerializeField]
	private GameObject m_tileClone;
	[SerializeField]
	private int m_xSize;
	[SerializeField]
	private int m_zSize;
	[SerializeField]
	private int m_tileDimensions;
	

}