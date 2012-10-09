using UnityEngine;
using System.Collections;

public class TowerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		m_dampingCoeff = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//Calculate the difference between the turrent's orientation and the players position
		Quaternion deltaRotate = Quaternion.LookRotation(m_player.transform.position - transform.position);
		//match the turrets orientation to face the player, but do it slowly, according to the DampingCoefficient
		transform.rotation = Quaternion.Slerp(transform.rotation, deltaRotate, Time.deltaTime  * m_dampingCoeff);
	}

	public PlayerCharacter Player
	{
		get { return m_player; }
	}
	
	public float DampingCoefficient
	{
		get { return m_dampingCoeff; }
		set { m_dampingCoeff = value; }
	}
	
	
	[SerializeField]
	private PlayerCharacter m_player;
	
	[SerializeField]
	private float m_dampingCoeff;

}
