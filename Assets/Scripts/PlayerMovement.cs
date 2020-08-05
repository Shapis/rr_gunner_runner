using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController2D m_CharacterController2D;
    private InputHandler m_InputHandler;

    [Header("Settings")]
    [SerializeField] private float m_FellThroughWorldReset = 200f;
    private int horizontalMovementDirection = 0;
    private bool jump = false;
    private bool collisionJump = false;

    private void Awake()
    {
        m_InputHandler = FindObjectOfType<InputHandler>();
    }

    private void Start()
    {
        m_InputHandler.OnVerticalUpPressedEvent += OnVerticalUpPressed;

        m_InputHandler.OnJumpPressedEvent += OnJumpPressed;
    }

    private void OnVerticalUpPressed(object sender, EventArgs e)
    {
        jump = true;
    }

    private void OnJumpPressed(object sender, EventArgs e)
    {
        jump = true;
    }



    private void FixedUpdate()
    {
        m_CharacterController2D.Move(horizontalMovementDirection, jump);
        jump = false;
    }
}
