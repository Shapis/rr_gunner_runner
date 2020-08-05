using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFirer : MonoBehaviour
{
    [SerializeField] private Transform m_Bullet;
    [SerializeField] private float m_BulletVelocity = 10f;
    private InputHandler m_InputHandler;
    private GameObject m_BulletContainer;


    private void Awake()
    {
        m_InputHandler = GameObject.FindObjectOfType<InputHandler>();
        m_BulletContainer = GameObject.Find("Bullet_Container");
    }
    private void Start()
    {
        m_InputHandler.OnMouseButtonLeftPressedEvent += OnMouseButtonLeftPressed;
    }

    private void OnMouseButtonLeftPressed(object sender, Vector2 e)
    {
        Transform myBullet;
        myBullet = Instantiate(m_Bullet, gameObject.transform.position, new Quaternion());
        myBullet.GetComponent<Rigidbody2D>().velocity = new Vector3(gameObject.GetComponentInParent<Rigidbody2D>().velocity.x + m_BulletVelocity, 0, 0);
        myBullet.SetParent(m_BulletContainer.transform);
    }
}
