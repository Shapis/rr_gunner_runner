using UnityEngine;
using System;

public class MobileJoystick : MonoBehaviour, IMobileJoystickEvents
{
    [Header("Dependencies")]
    private InputHandler m_InputHandler;
    [SerializeField] private Transform joystickBackgroundCenter;
    [SerializeField] private Transform joystickBackgroundOutline;
    [SerializeField] private Transform joystickCenterBall;
    private bool touchStarted = false;
    private Vector2 mousePosition = new Vector2();
    [SerializeField] private float m_SensitivityTreshold = 10f;
    public event EventHandler OnJoystickHorizontalLeftPressedEvent;
    public event EventHandler OnJoystickHorizontalLeftUnpressedEvent;
    public event EventHandler OnJoystickHorizontalRightPressedEvent;
    public event EventHandler OnJoystickHorizontalRightUnpressedEvent;
    public event EventHandler OnJoystickVerticalUpPressedEvent;
    public event EventHandler OnJoystickVerticalUpUnpressedEvent;
    public event EventHandler OnJoystickVerticalDownPressedEvent;
    public event EventHandler OnJoystickVerticalDownUnpressedEvent;
    private Vector2Int direction = new Vector2Int(0, 0);

    private void Awake()
    {
        m_InputHandler = GameObject.FindObjectOfType<InputHandler>();
    }

    private void Start()
    {
        m_InputHandler.OnMouseButtonLeftPressedEvent += OnMouseButtonLeftPressed;
        m_InputHandler.OnMouseButtonLeftUnpressedEvent += OnMouseButtonLeftUnpressed;
        m_InputHandler.OnMouseHoverEvent += OnMouseHover;
    }

    private void OnMouseHover(object sender, Vector2 e)
    {
        mousePosition = e;
    }

    private void OnMouseButtonLeftUnpressed(object sender, Vector2 e)
    {
        touchStarted = false;
        joystickCenterBall.position = (Vector2)joystickBackgroundCenter.position;


        if (direction.x == 1)
        {
            OnJoystickHorizontalRightUnpressed(this, EventArgs.Empty);
        }
        else if (direction.x == -1)
        {
            OnJoystickHorizontalLeftUnpressed(this, EventArgs.Empty);
        }

        if (direction.y == 1)
        {
            OnJoystickVerticalUpUnpressed(this, EventArgs.Empty);
        }
        else if (direction.y == -1)
        {
            OnJoystickVerticalDownUnpressed(this, EventArgs.Empty);
        }


        direction.x = 0;
        direction.y = 0;
    }

    private void OnMouseButtonLeftPressed(object sender, Vector2 e)
    {
        if ((e - (Vector2)joystickBackgroundCenter.position).magnitude < 60f)
        {
            touchStarted = true;
        }
    }

    // I'm not 100% positive if this should be Update() or FixedUpdate(), I'm doing update because this is dealing with inputs and rendering, not physics directly.
    private void Update()
    {
        if (touchStarted)
        {
            Vector2 offset = mousePosition - (Vector2)joystickBackgroundCenter.position;
            joystickCenterBall.position = (Vector2)joystickBackgroundCenter.position + Vector2.ClampMagnitude(offset, 45f);

            // Horizontal movement
            if (offset.x >= m_SensitivityTreshold && direction.x != 1)
            {
                direction.x = 1;
                OnJoystickHorizontalRightPressed(this, EventArgs.Empty);
            }
            else if (offset.x < m_SensitivityTreshold && direction.x == 1)
            {
                direction.x = 0;
                OnJoystickHorizontalRightUnpressed(this, EventArgs.Empty);
            }

            if (offset.x <= -m_SensitivityTreshold && direction.x != -1)
            {
                direction.x = -1;
                OnJoystickHorizontalLeftPressed(this, EventArgs.Empty);
            }
            else if (offset.x > -m_SensitivityTreshold && direction.x == -1)
            {
                direction.x = 0;
                OnJoystickHorizontalLeftUnpressed(this, EventArgs.Empty);
            }

            // Vertical movement
            if (offset.y >= m_SensitivityTreshold * 2 && direction.y != 1)
            {
                direction.y = 1;
                OnJoystickVerticalUpPressed(this, EventArgs.Empty);
            }
            else if (offset.y < m_SensitivityTreshold * 2 && direction.y == 1)
            {
                direction.y = 0;
                OnJoystickVerticalUpUnpressed(this, EventArgs.Empty);
            }

            if (offset.y <= -m_SensitivityTreshold && direction.y != -1)
            {
                direction.y = -1;
                OnJoystickVerticalDownPressed(this, EventArgs.Empty);
            }
            else if (offset.y > -m_SensitivityTreshold && direction.y == -1)
            {
                direction.y = 0;
                OnJoystickVerticalDownUnpressed(this, EventArgs.Empty);
            }
        }
    }

    public void OnJoystickHorizontalLeftPressed(object sender, EventArgs e)
    {
        OnJoystickHorizontalLeftPressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickHorizontalLeftUnpressed(object sender, EventArgs e)
    {
        OnJoystickHorizontalLeftUnpressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickHorizontalRightPressed(object sender, EventArgs e)
    {
        OnJoystickHorizontalRightPressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickHorizontalRightUnpressed(object sender, EventArgs e)
    {
        OnJoystickHorizontalRightUnpressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickVerticalUpPressed(object sender, EventArgs e)
    {
        OnJoystickVerticalUpPressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickVerticalUpUnpressed(object sender, EventArgs e)
    {
        OnJoystickVerticalUpUnpressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickVerticalDownPressed(object sender, EventArgs e)
    {
        OnJoystickVerticalDownPressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnJoystickVerticalDownUnpressed(object sender, EventArgs e)
    {
        OnJoystickVerticalDownUnpressedEvent?.Invoke(this, EventArgs.Empty);
    }
}
