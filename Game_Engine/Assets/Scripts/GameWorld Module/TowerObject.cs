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
		if (isInRange(m_player.Position)){
			aimAt(m_player.Position);
			//check if aimed correctly
			//shoot missile
		}
		
	}
	
	/* retuns true if the target is in range*/
	bool isInRange(Vector3 target){
		target = target - transform.position;
		float distanceToTarget = target.magnitude;
		if (distanceToTarget <= m_maxRange && distanceToTarget >= m_minRange){
			return true;
		}
		return false;
	}
	
	/* Points the tower object at the target, and returns a number indicating the precision of the targeting
	*/
	float aimAt(Vector3 target){
		Quaternion rotateTo = Quaternion.LookRotation(target - transform.position);
		Quaternion slerp = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime  * m_dampingCoeff);
		float aimError = (slerp.eulerAngles - rotateTo.eulerAngles).magnitude;
		if (aimError >= m_angleError*0){
			transform.rotation = slerp;
		}
		return aimError;
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
	private const float m_angleError = 1.0f;
	[SerializeField]
	private float m_maxRange = 0;
	[SerializeField]
	private float m_minRange = 0;

}
