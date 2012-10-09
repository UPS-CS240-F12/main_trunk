using UnityEngine;
using System.Collections;

public class TowerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// m_dampingCoeff = 5.0f;
		// m_maxRange = 100.0f;
		// m_minRange = 50.0f;
	}
	
	// Update is called once per frame
	void Update () {
		float distanceToPlayer = getDistanceTo(m_player.Position);
		if (distanceToPlayer <= m_maxRange && distanceToPlayer >= m_minRange){
			aimAt(m_player.Position);
		}
	}
	
	/* retuns the distance to the traget*/
	float getDistanceTo(Vector3 target){
		target = target - transform.position;
		return target.magnitude;
	}
	
	/* Points the tower object at the target
	*/
	void aimAt(Vector3 target){
		//Calculate the difference between the turrent's orientation and the players position
		Quaternion deltaRotate = Quaternion.LookRotation(target - transform.position);
		//match the turrets orientation to face the player, but do it slowly, according to the DampingCoefficient
		transform.rotation = Quaternion.Slerp(transform.rotation, deltaRotate, Time.deltaTime  * m_dampingCoeff);
	}

	public PlayerCharacter Player
	{
		get { return m_player; }
		set { m_player = value; }
	}
	
	public float DampingCoefficient
	{
		get { return m_dampingCoeff; }
		set { m_dampingCoeff = value; }
	}
	
	public float MaximumRange
	{
		get { return m_maxRange; }
		set { m_maxRange = value; }
	}
	
	public float MinimumRange
	{
		get { return m_minRange; }
		set { m_minRange = value; }
	}
	
	
	[SerializeField]
	private PlayerCharacter m_player;
	
	[SerializeField]
	private float m_dampingCoeff;
	[SerializeField]
	private float m_maxRange = 0;
	[SerializeField]
	private float m_minRange = 0;

}
