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
			float aimError = aimAt(m_player.Position);
			if (aimError <= m_maxAngleError){
				shoot();
			}
		}
	}
	
	void shoot(){
		// GameObject fireball = Instantiate(m_fireballClone, transform.positon, transform.rotation) as GameObject;
		// 		fireball.GetComponent<Projectile>().Direction = transform.TransformDirection(Vector3.forward);
	}
	
	// retuns true if the target is in range
	bool isInRange(Vector3 target){
		float distanceToTarget = (target - transform.position).magnitude;
		if (distanceToTarget <= m_maxRange && distanceToTarget >= m_minRange){
			return true;
		}
		return false;
	}
	
	// Points the tower object closer to the target, and returns a number indicating the ammount the tower is off by
	float aimAt(Vector3 target){
		Quaternion rotateTo = Quaternion.LookRotation(target - transform.position);
		Quaternion slerp = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime  * m_dampingCoeff);
		float aimError = (slerp.eulerAngles - rotateTo.eulerAngles).magnitude;
		if (aimError >= m_maxAngleError){
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
    private GameObject m_fireballClone;
	
	[SerializeField]
	private float m_dampingCoeff;
	[SerializeField]
	private const float m_maxAngleError = 1.0f;
	[SerializeField]
	private float m_maxRange = 500;
	[SerializeField]
	private float m_minRange = 100;

}