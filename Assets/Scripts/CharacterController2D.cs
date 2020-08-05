using System;
using UnityEngine;

public class CharacterController2D : MonoBehaviour, ICharacterEvents
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private Transform m_CeilingCheck; // A position marking where to check for ceilings
    [SerializeField] private Transform m_GroundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character A position marking where to check for ceilings

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private bool m_AirControl = false; // Whether or not a player can steer while jumping;
    [SerializeField] private float m_JumpIntensity = 400f; // Amount of force/velocity added when the unit jumps.
    [SerializeField] private bool m_UseVelocityForJumping = false; // Change the unit's y axis velocity instead of adding y force when jumping.
    [SerializeField] [Range(0.0f, 5f)] private float m_CoyoteTime = 0.2f; // Duration of coyote time.
    [SerializeField] private bool m_DoubleJump = false; // Whether the player can double jump.
    [SerializeField] private int m_NumberOfDoubleJumps = 1; // If Double jumps are enabled, this is the amount of times a player is allowed to double jump by default.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up

    private bool m_FacingRight = true; // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private float myCoyoteStartTime;
    private bool myCoyoteJump = false;

    public int DoubleJumpsRemaining { get; private set; }
    public bool IsGrounded { get; private set; }
    public int IsMovingHorizontally { get; private set; }
    public bool IsFalling { get; private set; }

    private Vector3 previousFeetPosition;
    private int previousMovementDirection;

    public event EventHandler OnAirbourneEvent; // When the unit first becomes airbourne.
    public event EventHandler OnLandingEvent; // When a unit initially lands.
    public event EventHandler OnFallingEvent; // When a unit starts falling. As in, your first airbourne position.y is lower than your previous position.y or your first airbourne position after reaching peak is lower than the previous one.
    public event EventHandler<int> OnHorizontalMovementChangesEvent;
    public event EventHandler OnJumpEvent;

    private void Start()
    {
        previousFeetPosition = m_GroundCheck.transform.position;
        DoubleJumpsRemaining = m_NumberOfDoubleJumps;


        if (!m_DoubleJump)
        {
            m_NumberOfDoubleJumps = 0;
            DoubleJumpsRemaining = 0;
        }
    }

    private void FixedUpdate()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////// Events

        bool previouslyGrounded = IsGrounded; // previouslyGrounded keeps the information of whether the unit was grounded the previous frame.

        IsGrounded = GroundedCheck(); // Checks whether the unit is currently grounded and assigns it to the grounded property.

        if (LandingCheck(previouslyGrounded))
            OnLanding(this, EventArgs.Empty); // If the unit has landed invokes the OnLandingEvent.

        if (AirbourneCheck(previouslyGrounded))
            OnAirbourne(this, EventArgs.Empty); // If the unit is airbourne invokes the OnAirbourneEvent.

        if (FallingCheck(AirbourneCheck(previouslyGrounded), previousFeetPosition.y))
            OnFalling(this, EventArgs.Empty); // If the unit is airbourne, falling and wasnt explicitly because of a jump run invokes the OnFallingEvent.

        // Updates the previous feet position to the current feet ground position so you can check if the unit is falling while airbourne the next frame.
        previousFeetPosition = m_GroundCheck.transform.position;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////// //Events//

        CoyoteTime();
    }


    private void CoyoteTime()
    {
        if (myCoyoteJump && Time.time >= m_CoyoteTime + myCoyoteStartTime)
        {
            myCoyoteJump = false;
        }
        if (!myCoyoteJump && IsFalling)
        {
            DoubleJumpsRemaining = 0;
        }
    }

    private bool GroundedCheck()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as to be considered as ground, checking for which layers are ground m_WhatIsGround
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        return colliders.Length > 0;
    }

    private bool LandingCheck(bool previouslyGrounded)
    {
        return IsGrounded && !previouslyGrounded;
    }

    public void OnLanding(object sender, EventArgs e)
    {
        DoubleJumpsRemaining = m_NumberOfDoubleJumps;
        IsFalling = false;

        OnLandingEvent?.Invoke(this, EventArgs.Empty);
    }

    private bool AirbourneCheck(bool previouslyGrounded)
    {
        return !IsGrounded && previouslyGrounded;
    }

    public void OnAirbourne(object sender, EventArgs e)
    {
        OnAirbourneEvent?.Invoke(this, EventArgs.Empty);
    }

    private bool FallingCheck(bool isAirbourne, float previousFeetPosition)
    {
        return isAirbourne && (m_GroundCheck.transform.position.y < previousFeetPosition);
    }

    public void OnFalling(object sender, EventArgs e)
    {
        IsFalling = true;
        myCoyoteStartTime = Time.time;
        myCoyoteJump = true;
        OnFallingEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnHorizontalMovementChanges(object sender, int movementDirection)
    {
        OnHorizontalMovementChangesEvent?.Invoke(this, movementDirection);
    }

    public void OnJump(object sender, EventArgs e)
    {
        OnJumpEvent?.Invoke(this, EventArgs.Empty);
    }

    public void Move(int movementDirection, bool jump)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(movementSpeed, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = targetVelocity;
        Debug.Log(jump);

        // If the player should jump...
        if ((IsGrounded && jump))
        {
            // Add vertical force/velocity to the unit.
            Jump();
        }
    }

    // Add vertical force/velocity to the unit.
    private void Jump()
    {
        OnJump(this, EventArgs.Empty);

        if (m_UseVelocityForJumping)
        {
            m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, m_JumpIntensity / 50, 0);
        }
        else
        {
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpIntensity));
        }

        // After the unit jumps it is no longer falling.
        IsFalling = false;
        myCoyoteJump = false;
    }
}