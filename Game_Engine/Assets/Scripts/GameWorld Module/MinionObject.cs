using UnityEngine;
using System.Collections;
using System;

public class MinionObject : MonoBehaviour {

    // Use this for initialization
    void Start () {
        frozen = false;
        //isChasing = true;
        spinRate = 400.0f;
        //speed = 1;
        maxRange = 4000.0f;
        minRange = 200.0f;
        maxHitPoints = hitPoints = 2;
        player = GameObject.FindWithTag("Player");
        StartCoroutine("unfreezeTimer");

    }
    
    // Update is called once per frame
    void Update () {
        Vector3 playerLocation = player.transform.position;
        if (!frozen){
            aimAt(playerLocation);
            if(inRange(playerLocation)){
                runAt(playerLocation);
            }
            if(touchingPlayer(playerLocation)){
                attackPlayer();
            }
        }
        else{
            doSpinAnimation();
        }
    }

    void doSpinAnimation(){
        transform.Rotate(Vector3.right,spinRate*Time.deltaTime,0);
    }

    void runAt(Vector3 target){
        CharacterController controller = GetComponent<CharacterController>();
        Vector3 update = Vector3.Lerp(transform.position, target, Time.deltaTime);
        Vector3 movement = update - transform.position;
        controller.Move(movement);
        

        
    }

    void aimAt(Vector3 target){
        Quaternion rotateTo = Quaternion.LookRotation(target - transform.position);
        Quaternion slerp = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime * spinRate);

        transform.rotation = slerp;
    }

    void attackPlayer(){
        player.GetComponent<PlayerCharacter>().Damage(1);
		if (m_owner != null)
			NetworkInterface.AddPhoneScore(m_owner, 1);
    }

    bool touchingPlayer(Vector3 playerPosition){
        if((playerPosition - transform.position).magnitude < minRange/2){
            return true;
        }
        return false;
    }   

    private IEnumerator unfreezeTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
                frozen = false;
        }
    }

    bool inRange(Vector3 target){
        float distanceToTarget = (target - transform.position).magnitude;
        //Debug.Log(distanceToTarget);
        if (distanceToTarget <= maxRange && distanceToTarget >= minRange){
            return true;
        }
        return false;
    }

    public void takeDamage(float amount){
        hitPoints -= amount;
        //Debug.Log("Took damage of: "+amount);
        if (hitPoints < 0){
            frozen = true;
            hitPoints = maxHitPoints;
        }
    }
	

	void OnDestroy()
	{	
		NetworkInterface.ClearMinion(m_id);
	}

    public GameObject Player{
        get { return player; }
        set { player = value; }
    }
	
	public string PhoneOwner
	{
		get { return m_owner; }
		set { m_owner = value; }
	}
	
	public string ID
	{
		get { return m_id; }
		set {m_id = value; }
	}

    
    private GameObject player;
	
	[SerializeField]
	private string m_id = null;
	[SerializeField]
	private string m_owner = null;

    //private bool isChasing;
    private float maxRange;
    private float minRange;
    //private float speed;
    private float hitPoints;
    private float maxHitPoints;
    private bool frozen;
    private float spinRate;
}
