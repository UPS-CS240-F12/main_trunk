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
	}
	
	// Update is called once per frame
	void Update()
    {
        //TODO: Move to FixedUpdate()
		CharacterController controller = GetComponent<CharacterController>();
		m_position = transform.position;
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
            moveDirection *= m_movementSpeed;

        /*
        float curSpeed = movementSpeed * InputControls.GetMovement();
        InputControls.ClearMovement();
        */

        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                m_fallSpeed = m_jumpSpeed;
                moveDirection.y = m_jumpSpeed;
            }
            else
                m_fallSpeed = 0;
            /*
            if (InputControls.IsJumping() == true)
            {
                moveDirection.y = jumpSpeed;
                InputControls.ClearJump();
            }
            */
        }
        else
        {
            // Apply gravity
            m_fallSpeed -= m_gravity * Time.deltaTime;
            moveDirection.y += m_fallSpeed;
        }

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        if (transform.position.y < -1000)
        {
            transform.position = new Vector3(980, 40.08f, 980);
            HitPoints -= 25;

            if (HitPoints <= 0)
                HitPoints = MaxHitPoints;
        }
	}

    void LateUpdate()
    {
        m_camera.transform.position = new Vector3(transform.position.x, transform.position.y + 500, transform.position.z - 300);
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
	
	public Vector3 Position
	{
		get { return m_position; }
		set { ; }
	}

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

    [SerializeField]
    private int m_hitPoints;
    [SerializeField]
    private int m_maxHitPoints;
    [SerializeField]
    private int m_energyPoints;
    [SerializeField]
    private int m_maxEnergyPoints;

	[SerializeField]
	public Vector3 m_position;

    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private HUDBarObject m_healthBar;
    [SerializeField]
    private HUDBarObject m_energyBar;
}
