using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [SerializeField] float m_speed = 1f;
    Rigidbody m_rb;
    Animator m_anim;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = (Vector3.forward * v + Vector3.right * h).normalized;
        m_rb.AddForce(dir * m_speed);

        if (Input.GetButtonDown("Jump"))
        {
            m_anim.SetBool("Heat", true);
        }

        if (Input.GetButtonUp("Jump"))
        {
            m_anim.SetBool("Heat", false);
        }
    }
}
