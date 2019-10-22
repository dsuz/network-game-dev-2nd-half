using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody))]
public class CannonController : MonoBehaviour
{
    [SerializeField] float m_power = 1f;
    [SerializeField] float m_lifeTime = 1f;
    bool m_fired;
    float m_timer;
    Rigidbody m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!m_fired) return;

        m_timer += Time.deltaTime;
        if (m_timer > m_lifeTime)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void Fire(Vector3 dir)
    {
        if (!m_fired)
        {
            m_fired = true;
            m_rb.AddForce(dir * m_power, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
