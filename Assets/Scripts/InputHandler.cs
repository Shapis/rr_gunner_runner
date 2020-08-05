using System;
using UnityEngine;

public class InputHandler : MonoBehaviour, IMobileJoystickEvents
{
    [SerializeField] private bool m_DebugLoggingEnabled = false;

    #region Cancel Events
    public event EventHandler OnCancelPressedEvent; // Escape on PC, Back button on Android.
    public event EventHandler OnCancelUnpressedEvent;
    #endregion

    #region Horizontal Events
    public event EventHandler OnHorizontalLeftPressedEvent;
    public event EventHandler OnHorizontalLeftUnpressedEvent;
    public event EventHandler OnHorizontalRightPressedEvent;
    public event EventHandler OnHorizontalRightUnpressedEvent;
    private int horizontalDirection;
    private bool horizontalBusy;
    #endregion

    #region Vertical Events
    public event EventHandler OnVerticalUpPressedEvent;
    public event EventHandler OnVerticalUpUnpressedEvent;
    public event EventHandler OnVerticalDownPressedEvent;
    public event EventHandler OnVerticalDownUnpressedEvent;
    private int verticalDirection;
    private bool verticalBusy;
    #endregion

    #region Jump Events
    public event EventHandler OnJumpPressedEvent;
    public event EventHandler OnJumpUnpressedEvent;
    #endregion

    #region Mouse Events
    public event EventHandler<Vector2> OnMouseHoverEvent;
    public event EventHandler<Vector2> OnMouseButtonLeftPressedEvent;
    public event EventHandler<Vector2> OnMouseButtonLeftUnpressedEvent;
    #endregion

    #region Mobile Joystick Dependencies
    private MobileJoystick m_MobileJoystick;
    #endregion

    private void Awake()
    {
        m_MobileJoystick = GameObject.FindObjectOfType<MobileJoystick>();
    }

    private void Start()
    {
        if (m_MobileJoystick != null)
        {
            m_MobileJoystick.OnJoystickHorizontalLeftPressedEvent += OnJoystickHorizontalLeftPressed;
            m_MobileJoystick.OnJoystickHorizontalLeftUnpressedEvent += OnJoystickHorizontalLeftUnpressed;
            m_MobileJoystick.OnJoystickHorizontalRightPressedEvent += OnJoystickHorizontalRightPressed;
            m_MobileJoystick.OnJoystickHorizontalRightUnpressedEvent += OnJoystickHorizontalRightUnpressed;
            m_MobileJoystick.OnJoystickVerticalDownPressedEvent += OnJoystickVerticalDownPressed;
            m_MobileJoystick.OnJoystickVerticalDownUnpressedEvent += OnJoystickVerticalDownUnpressed;
            m_MobileJoystick.OnJoystickVerticalUpPressedEvent += OnJoystickVerticalUpPressed;
            m_MobileJoystick.OnJoystickVerticalUpUnpressedEvent += OnJoystickVerticalUpUnpressed;
        }
    }
    void Update()
    {
        #region Cancel events
        if (Input.GetButtonDown("Cancel"))
        {
            OnCancelPressed();
            if (m_DebugLoggingEnabled)
            {
                Debug.Log("Cancel button pressed!");
            }
        }
        if (Input.GetButtonUp("Cancel"))
        {
            OnCancelUnpressed();
            if (m_DebugLoggingEnabled)
            {
                Debug.Log("Cancel button unpressed!");
            }
        }
        #endregion

        #region Horizontal events
        if (Input.GetButton("Horizontal"))
        {
            if (!horizontalBusy)
            {
                horizontalBusy = true;
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    OnHorizontalLeftPressed(this, EventArgs.Empty);
                    horizontalDirection = -1;
                    if (m_DebugLoggingEnabled)
                    {
                        Debug.Log("Left button pressed!");
                    }
                }
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    OnHorizontalRightPressed(this, EventArgs.Empty);
                    horizontalDirection = 1;
                    if (m_DebugLoggingEnabled)
                    {
                        Debug.Log("Right button pressed!");
                    }
                }
            }
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            if ((Input.GetAxisRaw("Horizontal") >= 0 && horizontalDirection < 0))
            {
                OnHorizontalLeftUnpressed(this, EventArgs.Empty);
                horizontalBusy = false;
                if (m_DebugLoggingEnabled)
                {
                    Debug.Log("Left button unpressed!");
                }
            }
            else if (Input.GetAxisRaw("Horizontal") <= 0 && horizontalDirection > 0)
            {
                OnHorizontalRightUnpressed(this, EventArgs.Empty);
                horizontalBusy = false;
                if (m_DebugLoggingEnabled)
                {
                    Debug.Log("Right button unpressed!");
                }
            }
        }
        #endregion

        #region  Vertical events
        if (Input.GetButton("Vertical"))
        {
            if (!verticalBusy)
            {
                verticalBusy = true;
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    OnVerticalDownPressed(this, EventArgs.Empty);
                    verticalDirection = -1;
                    if (m_DebugLoggingEnabled)
                    {
                        Debug.Log("Down button pressed!");
                    }
                }
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    OnVerticalUpPressed(this, EventArgs.Empty);
                    verticalDirection = 1;
                    if (m_DebugLoggingEnabled)
                    {
                        Debug.Log("Up button pressed!");
                    }
                }
            }
        }
        if (Input.GetButtonUp("Vertical"))
        {

            if ((Input.GetAxisRaw("Vertical") >= 0 && verticalDirection < 0))
            {
                OnVerticalDownUnpressed(this, EventArgs.Empty);
                verticalBusy = false;
                if (m_DebugLoggingEnabled)
                {
                    Debug.Log("Down button unpressed!");
                }
            }
            else if (Input.GetAxisRaw("Vertical") <= 0 && verticalDirection > 0)
            {
                OnVerticalUpUnpressed(this, EventArgs.Empty);
                verticalBusy = false;
                if (m_DebugLoggingEnabled)
                {
                    Debug.Log("Up button unpressed!");
                }
            }
        }
        #endregion

        #region Jump events
        if (Input.GetButtonDown("Jump"))
        {
            OnJumpPressed();
            if (m_DebugLoggingEnabled)
            {
                Debug.Log("Jump button pressed!");
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            OnJumpUnpressed();
            if (m_DebugLoggingEnabled)
            {
                Debug.Log("Jump button unpressed!");
            }
        }
        #endregion

        #region Mouse events
        OnMouseHover(Input.mousePosition); // This fires off every update for passing the current mouse position.

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonLeftPressed(Input.mousePosition);
            if (m_DebugLoggingEnabled)
            {
                Debug.Log("Mouse button left pressed! At position: " + Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonLeftUnpressed(Input.mousePosition);
            if (m_DebugLoggingEnabled)
            {
                Debug.Log("Mouse button left unpressed! At position: " + Input.mousePosition);
            }
        }
        #endregion
    }

    #region Cancel events invoker
    private void OnCancelPressed()
    {
        OnCancelPressedEvent?.Invoke(this, EventArgs.Empty);
    }
    private void OnCancelUnpressed()
    {
        OnCancelUnpressedEvent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Vertical events invoker
    private void OnVerticalUpPressed(object sender, EventArgs e)
    {
        OnVerticalUpPressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    private void OnVerticalUpUnpressed(object sender, EventArgs e)
    {
        OnVerticalUpUnpressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    private void OnVerticalDownPressed(object sender, EventArgs e)
    {
        OnVerticalDownPressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    private void OnVerticalDownUnpressed(object sender, EventArgs e)
    {
        OnVerticalDownUnpressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    #endregion

    #region Horizontal events invoker
    private void OnHorizontalLeftPressed(object sender, EventArgs e)
    {
        OnHorizontalLeftPressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    private void OnHorizontalLeftUnpressed(object sender, EventArgs e)
    {
        OnHorizontalLeftUnpressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    private void OnHorizontalRightPressed(object sender, EventArgs e)
    {
        OnHorizontalRightPressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    private void OnHorizontalRightUnpressed(object sender, EventArgs e)
    {
        OnHorizontalRightUnpressedEvent?.Invoke(sender, EventArgs.Empty);
    }
    #endregion

    #region Jump events invoker
    private void OnJumpPressed()
    {
        OnJumpPressedEvent?.Invoke(this, EventArgs.Empty);
    }
    private void OnJumpUnpressed()
    {
        OnJumpUnpressedEvent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Mouse events invoker
    // If the mouse position is negative or larger than the length/width of the screen resolution it means the mouse is out of bounds of the gamescreen.
    private void OnMouseHover(Vector2 myMousePosition)
    {
        OnMouseHoverEvent?.Invoke(this, myMousePosition);
    }

    private void OnMouseButtonLeftPressed(Vector2 myMousePosition)
    {
        OnMouseButtonLeftPressedEvent?.Invoke(this, myMousePosition);
    }
    private void OnMouseButtonLeftUnpressed(Vector2 myMousePosition)
    {
        OnMouseButtonLeftUnpressedEvent?.Invoke(this, myMousePosition);
    }
    #endregion

    #region Mobile Joystick methods to invoke the equivalent mouse events
    public void OnJoystickHorizontalLeftPressed(object sender, EventArgs e)
    {
        OnHorizontalLeftPressed(sender, e);
    }

    public void OnJoystickHorizontalLeftUnpressed(object sender, EventArgs e)
    {
        OnHorizontalLeftUnpressed(sender, e);
    }

    public void OnJoystickHorizontalRightPressed(object sender, EventArgs e)
    {
        OnHorizontalRightPressed(sender, e);
    }

    public void OnJoystickHorizontalRightUnpressed(object sender, EventArgs e)
    {
        OnHorizontalRightUnpressed(sender, e);
    }

    public void OnJoystickVerticalUpPressed(object sender, EventArgs e)
    {
        OnVerticalUpPressed(sender, e);
    }

    public void OnJoystickVerticalUpUnpressed(object sender, EventArgs e)
    {
        OnVerticalUpUnpressed(sender, e);
    }

    public void OnJoystickVerticalDownPressed(object sender, EventArgs e)
    {
        OnVerticalDownPressed(sender, e);
    }

    public void OnJoystickVerticalDownUnpressed(object sender, EventArgs e)
    {
        OnVerticalDownUnpressed(sender, e);
    }
    #endregion
}
