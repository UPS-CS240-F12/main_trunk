using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour
{
	
	// Use this for initialization
	void Start()
    {
        m_healthBar.CurrentValue = m_hitPoints;
        m_healthBar.MaxValue = m_maxHitPoints;
        m_energyBar.CurrentValue = m_energyPoints;
        m_energyBar.MaxValue = m_maxEnergyPoints;
		m_position = transform.position;
		firstJump = false;
		jumping = false;
		shield = false;
		energyMagnitude = 0.09f;
		moveBonus = 1;
		defaultSpeed = m_movementSpeed;
		
		terrainFactory = GameObject.FindGameObjectWithTag("TerrainFactory");
        StartCoroutine("EnergyLossRoutine");
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
        //TODO: Move to FixedUpdate()
		CharacterController controller = GetComponent<CharacterController>();
        // Rotate around y - axis
        /*
        transform.Rotate(0, InputControls.GetRotation() * rotateSpeed, 0);
        InputControls.ClearRotation();
        */

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.Normalize();

        if (Input.GetButton("Roll"))
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
            if (Input.GetButton("Jump"))
            {
				audio.Play();
				moveBonus = 0;
				m_movementSpeed = defaultSpeed;
                m_fallSpeed = m_jumpSpeed;
                moveDirection.y = m_jumpSpeed;
				firstJump = false;
            }
            else
			{
				if(moveBonus < m_maxBoost)
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
        else if(Input.GetButton("Jump") && firstJump && EnergyPoints > 0)
		{
			//Perform Jetpack
			audio.Play ();
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
			
			TileMessenger messenger = new TileMessenger();
			terrainFactory.SendMessage("GetRandomTile", messenger);
			
			transform.position = messenger.message + new Vector3(0, 110, 0);
			
            HitPoints -= 25;

            if (HitPoints <= 0)
                HitPoints = 0;
        }
		//update stored position
		m_position = transform.position;
	}

    void LateUpdate()
    {
        m_camera.transform.position = new Vector3(transform.position.x, transform.position.y + 400, transform.position.z - 400);
		if(jumping)
		{
			Instantiate(m_jumpParticle, transform.position, Quaternion.identity);
		}
		if(shield)
			Instantiate(m_shieldParticle, transform.position, Quaternion.identity);
	}
	
	public void AddEnergy(int energy)
	{
		//audio.Play();
		//sound added here will play when a battery is picked up
		EnergyPoints += energy;
        if (EnergyPoints > MaxEnergyPoints)
            Application.LoadLevel("GameOverScene");
	}
	
	public void RemoveEnergy(int energy)
	{
		EnergyPoints -= energy;
        if (EnergyPoints < 0)
            EnergyPoints = 0;
	}
	
	public IEnumerator AddShield(float time)
	{
		shield = true;
		yield return new WaitForSeconds(time);
		shield = false;
	}
	
	public void AddThrust()
	{
		m_maxBoost += 100;
	}

    public void Damage(int damage)
    {
		if(!shield)
		{
      		HitPoints -= damage;
        	if (HitPoints <= 0)
				Application.LoadLevel("GameOverScene");
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
        set { m_hitPoints = value; m_healthBar.CurrentValue = m_hitPoints; }
    }

    public int MaxHitPoints
    {
        get { return m_maxHitPoints; }
        set { m_maxHitPoints = value; m_healthBar.MaxValue = m_maxHitPoints; }
    }

    public int EnergyPoints
    {
        get { return m_energyPoints; }
        set { m_energyPoints = value; m_energyBar.CurrentValue = m_energyPoints; }
    }

    public int MaxEnergyPoints
    {
        get { return m_maxEnergyPoints; }
        set { m_maxEnergyPoints = value; m_energyBar.MaxValue = m_maxEnergyPoints; }
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
	bool shield;
	float energyMagnitude;
	int moveBonus;
	float defaultSpeed;
	
	[SerializeField]
	private float m_maxBoost;
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
}