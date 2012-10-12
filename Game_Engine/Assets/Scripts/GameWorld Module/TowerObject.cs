using UnityEngine;
using System.Collections;

public class TowerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		m_dampingCoeff = 5.0f;
		m_maxRange = 1000.0f;
		m_minRange = 100.0f;
		m_maxAngleError = 1.0f;
		isShooting = false;
		StartCoroutine("FireballShooter");
	}
	
	// Update is called once per frame
	void Update () {
		if (isInRange(m_player.Position)){
			float aimError = aimAt(m_player.Position);
			if (aimError <= m_maxAngleError){
				isShooting = true;
			} else {
				isShooting = false;
			}
		}
	}
	private IEnumerator FireballShooter()
	{
		while (true)
		{
			yield return new WaitForSeconds(2);
			if(isShooting){
				ShootFireball();
			}
		}
	}

	
	void ShootFireball(){
		GameObject fireball = Instantiate(m_bulletClone, transform.position, transform.rotation * Quaternion.Euler(-90, 0, 0)) as GameObject;
		fireball.GetComponent<Projectile>().Direction = transform.TransformDirection(Vector3.forward);
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

	public GameObject BulletClone
	{
		get { return m_bulletClone; }
		set { m_bulletClone = value; }
	}

	public PlayerCharacter Player
	{
		get { return m_player; }
		set { m_player = value; }
	}

	// public float DampingCoefficient
	// {
	// 	get { return m_dampingCoeff; }
	// 	set { m_dampingCoeff = value; }
	// }
	// 
	// public float MaxRange
	// {
	// 	get { return m_maxRange; }
	// 	set { m_maxRange = value; }
	// }
	// 
	// public float MinRange
	// {
	// 	get { return m_minRange; }
	// 	set { m_minRange = value; }
	// }
	private bool isShooting;
	
	
	[SerializeField]
	private PlayerCharacter m_player;

	[SerializeField]
	private GameObject m_bulletClone;
	
	// [SerializeField]
	private float m_dampingCoeff;
	// [SerializeField]
	private float m_maxAngleError;
	// [SerializeField]
	private float m_maxRange;
	// [SerializeField]
	private float m_minRange;

}