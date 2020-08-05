using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;

    private void Start()
    {
        m_Player = GameObject.Find("Player");
    }
    void Update()
    {
        gameObject.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y, -10f);
    }
}
