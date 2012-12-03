using UnityEngine;
using System.Collections;
using System;

public class MinionObject : MonoBehaviour {

    // Use this for initialization
    void Start () {
        isChasing = true;
        speed = 1;
        maxRange = 4000.0f;
        minRange = 200.0f;
        player = GameObject.FindWithTag("Player");
    }
    
    // Update is called once per frame
    void Update () {
        Vector3 playerLocation = player.transform.position;
        aimAt(playerLocation);
        if(inRange(playerLocation)){
            runAt(playerLocation);
        }
        if(touchingPlayer(playerLocation)){
            attackPlayer();
        }
    }

    void runAt(Vector3 target){
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
    }

    void aimAt(Vector3 target){
        Quaternion rotateTo = Quaternion.LookRotation(target - transform.position);
        Quaternion slerp = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime);

        transform.rotation = slerp;
    }

    void attackPlayer(){
        player.GetComponent<PlayerCharacter>().Damage(1);
    }

    bool touchingPlayer(Vector3 playerPosition){
        if((playerPosition - transform.position).magnitude < minRange/2){
            return true;
        }
        return false;
    }

    bool inRange(Vector3 target){
        float distanceToTarget = (target - transform.position).magnitude;
        //Debug.Log(distanceToTarget);
        if (distanceToTarget <= maxRange && distanceToTarget >= minRange){
            return true;
        }
        return false;
    }

    public GameObject Player{
        get { return player; }
        set { player = value; }
    }
    
    private GameObject player;

    private bool isChasing;
    private float maxRange;
    private float minRange;
    private float speed;





}
