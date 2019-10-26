using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon 用の名前空間を参照する
// using ExitGames.Client.Photon;
using Photon.Pun;
// using Photon.Realtime;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    [SerializeField] float m_speed = 1f;
    [SerializeField] float m_rotateSpeed = 1f;
    [SerializeField] Transform m_muzzle;
    [SerializeField] string m_cannonPrefabName = "CannonPrefab";
    Rigidbody m_rb;
    Animator m_anim;
    GameObject m_cannonObject;
    /// <summary>RPC を呼び出すための PhotonView コンポーネント</summary>
    PhotonView m_photonView;
    /// <summary>ライフの最大値（初期値）</summary>
    [SerializeField] int m_maxLife = 30;
    /// <summary>現在のライフ</summary>
    int m_life;
    /// <summary>ライフ表示</summary>
    [SerializeField] UnityEngine.UI.Text m_lifeText;
    /// <summary>同期間隔（単位：秒）</summary>
    [SerializeField] float m_syncPeriod = 3f;
    float m_timer;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        // RPC を呼び出すために PhotonView を取得しておく
        m_photonView = GetComponent<PhotonView>();
        m_life = m_maxLife;
    }

    void Update()
    {
        // 同期処理
        m_timer += Time.deltaTime;
        if (m_timer > m_syncPeriod)
        {
            if (m_photonView && m_photonView.IsMine)
            {
                int id = PhotonNetwork.LocalPlayer.ActorNumber;
                object[] parameters = new object[] { id, m_life };
                m_photonView.RPC("SyncLife", RpcTarget.All, parameters);
                m_timer = 0;
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = this.transform.forward * v * m_speed;
        dir = new Vector3(dir.x, m_rb.velocity.y, dir.z);
        m_rb.velocity = dir;
        this.transform.Rotate(0f, m_rotateSpeed * h * Time.deltaTime, 0f);

        if (Input.GetButtonDown("Fire1"))
        {
            if (m_cannonObject == null)
            {
                m_cannonObject = PhotonNetwork.Instantiate(m_cannonPrefabName, m_muzzle.position, Quaternion.identity);
                m_cannonObject.GetComponent<CannonController>().enabled = true;
                m_cannonObject.transform.SetParent(m_muzzle);
            }
        }

        if (m_cannonObject && m_cannonObject.transform.parent)
        {
            m_cannonObject.transform.position = m_muzzle.position;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (m_cannonObject)
            {
                m_cannonObject.GetComponent<CannonController>().Fire(this.transform.forward);
                m_cannonObject.transform.SetParent(null);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 砲弾が当たった時の処理
        if (collision.gameObject.tag == "Canon")
        {
            // 破棄しないと何度も当たってしまうので、破棄する
            Destroy(collision.gameObject);

            // PhotonView をコントロールしている方だったら処理する
            if (m_photonView && m_photonView.IsMine)    // IsMine というのは「PhotonView が Controlled locally か」というプロパティ 
            {
                // id を取得して
                int id = PhotonNetwork.LocalPlayer.ActorNumber;
                //object[] parameters = new object[] { id, 10 };
                // RPC で呼び出す
                //m_photonView.RPC("Damage", RpcTarget.All, parameters);  // RpcTarget.All は、ローカルもリモートも呼び出すというオプションである。
                Damage(id, 10);
            }
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="playerId">プレイヤー ID</param>
    /// <param name="damage">受けたダメージ</param>
    //[PunRPC]
    void Damage(int playerId, int damage)
    {
        Debug.Log("player " + playerId.ToString() + " took " + damage.ToString() + " damage: ");

        // TODO: tank のライフを設定し、受けたダメージ分ライフを減らす
        // ライフが 0 になったらやられたとみなし、タンクのオブジェクトを破棄する
        m_life -= damage;
        
        if (m_life < 1)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            object[] parameters = new object[] { playerId, m_life };
            // RPC で呼び出す
            m_photonView.RPC("SyncLife", RpcTarget.All, parameters);
        }
    }

    /// <summary>
    /// ライフを同期する
    /// ライフ表示を更新する
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="currentLife"></param>
    [PunRPC]
    void SyncLife(int playerId, int currentLife)
    {
        m_life = currentLife;
        Debug.Log("player " + playerId.ToString() + "'s life: " + m_life.ToString());
        if (m_lifeText)
        {
            m_lifeText.text = m_life.ToString();
        }
    }
}
