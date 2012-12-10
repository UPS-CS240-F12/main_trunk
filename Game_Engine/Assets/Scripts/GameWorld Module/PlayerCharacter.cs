using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        if (m_energyAndShieldOnly == true)
        {
            m_healthBar.CurrentValue = m_energyPoints;
            m_healthBar.MaxValue = m_maxEnergyPoints;

            Shield shieldScript = m_shieldPowerup.GetComponent<Shield>();
            m_energyBar.DrawText = false;
            m_energyBar.CurrentValue = 0;
            m_energyBar.MaxValue = (int) (shieldScript.ActiveTime * 100);
        }
        else
        {
            m_healthBar.CurrentValue = m_hitPoints;
            m_healthBar.MaxValue = m_maxHitPoints;
            m_energyBar.CurrentValue = m_energyPoints;
            m_energyBar.MaxValue = m_maxEnergyPoints;
        }
        //m_shield.renderer.enabled = false;
		m_position = transform.position;
		firstJump = false;
		jumping = false;
		energyMagnitude = 0.09f;
		moveBonus = 1;
		defaultSpeed = m_movementSpeed;
        characterUp = transform.eulerAngles;//* Vector3.zero;
		
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");

        if (m_energyLossRate > 0){
            StartCoroutine("EnergyLossRoutine");
        }
        //StartCoroutine("AttackAnimationTimer");
        minions =  GameObject.FindGameObjectsWithTag("Minions");
        minion = GameObject.FindGameObjectWithTag("Minions");
	}

    private IEnumerator EnergyLossRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f / m_energyLossRate);
            EnergyPoints--;
        }
    }
	
	// Update is called once per frame
	void Update()
    {   
        minions = GameObject.FindGameObjectsWithTag("Minions");
        GameObject closestMinion = minions[0];
        float closestMinionDistance = DistanceTo(closestMinion.transform.position);
        foreach (GameObject aMinion in minions){
            if (closestMinionDistance > DistanceTo(aMinion.transform.position)){
                closestMinion = aMinion;
                closestMinionDistance = DistanceTo(aMinion.transform.position);
            }

        }
        minion = closestMinion;

        //TODO: Move to FixedUpdate()
		CharacterController controller = GetComponent<CharacterController>();
        InputControls userControls = m_inputControls.GetComponent<InputControls>();
        if (userControls == null)
            return;

        // Rotate around y - axis
        /*
        InputControls.ClearRotation();
        */

        transform.Rotate(0, userControls.Rotation() * m_rotateSpeed, 0);
        characterUp.Set(characterUp.x, transform.eulerAngles.y, characterUp.z);

        Vector3 moveDirection = new Vector3(0, 0, userControls.Movement());
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.Normalize();

        if (userControls.Roll())
            moveDirection *= m_rollSpeed;
        else
		{
            moveDirection *= m_movementSpeed + moveBonus;
		}

        /*
        float curSpeed = movementSpeed * InputControls.GetMovement();
        InputControls.ClearMovement();
        */

        if (controller.isGrounded)
        {
			
			energyMagnitude = 0;
            if (userControls.Jump())
            {
				m_movementSpeed = defaultSpeed;
                m_fallSpeed = m_jumpSpeed;
                moveDirection.y = m_jumpSpeed;
				firstJump = false;
            }
            else
			{
				if(moveBonus < 100)
					moveBonus++;
                m_fallSpeed = 0;
				firstJump = false;
			}
            /*
            if (InputControls.IsJumping() == true)
            {
                moveDirection.y = jumpSpeed;
                InputControls.ClearJump();
            }
            */
        }
        else if (userControls.Jump() && firstJump && EnergyPoints > 0)
		{
			//Perform Jetpack
			moveDirection.y = moveDirection.y + m_jumpSpeed;
			m_fallSpeed = m_jumpSpeed;
			RemoveEnergy((int)energyMagnitude);
			energyMagnitude += 0.25f;
			jumping = true;
		}
		else
        {
            // Apply gravity
            m_fallSpeed -= m_gravity * Time.deltaTime;
            moveDirection.y += m_fallSpeed;
			firstJump = true;
			jumping = false;
        }

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        if (transform.position.y < -1000)
        {
            //transform.position = new Vector3(980, 100.0f, 980);
			
			if(m_shieldActive)
			{
				TileMessenger messenger = new TileMessenger();
				terrainFactory.SendMessage("GetRandomTile", messenger);
				transform.position = messenger.message + new Vector3(0, 110, 0);
			}
			else
			{
				m_pointKeeper.SendMessage ("EndGame",false); // State character lost.
			}
        }
		//update stored position
		m_position = transform.position;

        if (userControls.Shield())
        {
            if (m_shieldActive == false && EnergyPoints > m_shieldActivationCost)
            {
                AddShield();
                RemoveEnergy(m_shieldActivationCost);
            }
        }

        if (userControls.Attack())
        {
            AttackAnimation();
            //Debug.Log("Minion is in range is "+InRange(minion.transform.position));
            if (InRange(minion.transform.position)){
                (minion.GetComponent<MinionObject>()).takeDamage(m_attackDamage);
            }

        }else{
            transform.eulerAngles = characterUp;
        }
	}

//    private IEnumerator AttackAnimationTimer()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(0.01f);
//                transform.Rotate(characterUp,10 * attackAnimationSpeed * Time.deltaTime,0);
//        }
//    }



    void AttackAnimation(){
        transform.Rotate(Vector3.right,attackAnimationSpeed*Time.deltaTime,0);
    }

    bool InRange(Vector3 target){
        //float distanceToTarget = (target - transform.position).magnitude;
        if (DistanceTo(target) < m_range){
            return true;
        }
        return false;
    }

    float DistanceTo(Vector3 target){
        return (target - transform.position).magnitude;
    }

    void LateUpdate()
    {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        m_camera.transform.position = new Vector3(transform.position.x - (400 * Mathf.Sin(Mathf.Deg2Rad * eulerAngles.y)), transform.position.y + 600, (transform.position.z) - (400 * Mathf.Cos(Mathf.Deg2Rad * eulerAngles.y)));
        m_camera.transform.LookAt(transform.position);

        //m_shield.transform.position = transform.position;
		if(jumping)
		{
			Instantiate(m_jumpParticle, transform.position, Quaternion.identity);
		}
        if (m_shieldActive)
        {
            Instantiate(m_shieldParticle, transform.position, Quaternion.identity);
            m_energyBar.CurrentValue -= (int)(Time.deltaTime * 100);
            if (m_energyBar.CurrentValue < 0)
            {
                m_energyBar.CurrentValue = 0;
                m_shieldActive = false;
                //m_shield.renderer.enabled = false;
            }
        }
	}

    public void AddThrust()
    {
    }
	
	public void AddEnergy(int energy)
	{
		EnergyPoints += energy;
        if (EnergyPoints > MaxEnergyPoints)
            m_pointKeeper.SendMessage ("EndGame",true); // Declare character won.
	}
	
	public void RemoveEnergy(int energy)
	{
		EnergyPoints -= energy;
        if (EnergyPoints < 0)
        {
            EnergyPoints = 0;
            if (m_energyAndShieldOnly == true)
                m_pointKeeper.SendMessage ("EndGame",false); // State character lost.
        }
	}
	
	public void AddShield()
	{
        m_shieldActive = true;
        m_energyBar.CurrentValue = m_energyBar.MaxValue;
        //m_shield.renderer.enabled = true;
	}

    public void Damage(int damage)
    {
        if (m_shieldActive == false)
		{
            if (m_energyAndShieldOnly == true)
                RemoveEnergy(damage);
            else
            {
                HitPoints -= damage;
                if (HitPoints <= 0)
                {
                    HitPoints = 0;
                    Application.LoadLevel("GameOverScene");
                }
            }
		}
    }

    public float MovementSpeed
    {
        get { return m_movementSpeed; }
        set { m_movementSpeed = value; }
    }

    public float RotateSpeed
    {
        get { return m_rotateSpeed; }
        set { m_rotateSpeed = value; }
    }

    public float JumpSpeed
    {
        get { return m_jumpSpeed; }
        set { m_jumpSpeed = value; }
    }

    public float RollSpeed
    {
        get { return m_rollSpeed; }
        set { m_rollSpeed = value; }
    }

    public float Gravity
    {
        get { return m_gravity; }
    }

    public int HitPoints
    {
        get { return m_hitPoints; }
        set
        {
            m_hitPoints = value;
            if (m_energyAndShieldOnly == false)
                m_healthBar.CurrentValue = m_hitPoints;
        }
    }

    public int MaxHitPoints
    {
        get { return m_maxHitPoints; }
        set
        {
            m_maxHitPoints = value;
            if (m_energyAndShieldOnly == false)
                m_healthBar.MaxValue = m_maxHitPoints;
        }
    }

    public int EnergyPoints
    {
        get { return m_energyPoints; }
        set
        {
            m_energyPoints = value;
            if (m_energyAndShieldOnly == true)
                m_healthBar.CurrentValue = m_energyPoints;
            else
                m_energyBar.CurrentValue = m_energyPoints;
        }
    }

    public int MaxEnergyPoints
    {
        get { return m_maxEnergyPoints; }
        set
        {
            m_maxEnergyPoints = value;
            if (m_energyAndShieldOnly == true)
                m_healthBar.MaxValue = m_maxEnergyPoints;
            else
                m_energyBar.MaxValue = m_maxEnergyPoints;
        }
    }

    public float EnergyLossRate
    {
        get { return m_energyLossRate; }
    }
	
	public Vector3 Position
	{
		get { return m_position; }
	}
	
	bool firstJump;
	bool jumping;
	float energyMagnitude;
	int moveBonus;
	float defaultSpeed;

    private Vector3 characterUp;
    private GameObject[] minions;
    private GameObject minion;
	
    [SerializeField]
    private float m_movementSpeed;
    [SerializeField]
    private float m_rotateSpeed;
    [SerializeField]
    private float m_jumpSpeed;
    [SerializeField]
    private float m_rollSpeed;

    [SerializeField]
    private float m_gravity;
    private float m_fallSpeed = 0;
	
	private GameObject terrainFactory;

    [SerializeField]
    private int m_hitPoints;
    [SerializeField]
    private int m_maxHitPoints;
    [SerializeField]
    private int m_energyPoints;
    [SerializeField]
    private int m_maxEnergyPoints;
    [SerializeField]
    private float m_energyLossRate = 1;
    [SerializeField]
    private bool m_shieldActive = false;
    [SerializeField]
    private int m_shieldActivationCost;
    [SerializeField]
    private bool m_energyAndShieldOnly = true;
    [SerializeField]
    private float m_attackDamage = 1.0f;
    [SerializeField]
    private float m_range = 250.0f;
    [SerializeField]
    private float attackAnimationSpeed;

    [SerializeField]
    private Vector3 m_position;

    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private HUDBarObject m_healthBar;
    [SerializeField]
    private HUDBarObject m_energyBar;
	
	[SerializeField]
	private GameObject m_jumpParticle;
	[SerializeField]
	private GameObject m_shieldParticle;
    [SerializeField]
    private GameObject m_shieldPowerup;

	[SerializeField]
	private GameObject m_pointKeeper;

    [SerializeField]
    private GameObject m_inputControls;
}