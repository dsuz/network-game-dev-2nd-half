using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 爆発・ブロック破壊機能を追加するコンポーネント
/// </summary>
public class ExplosionController : MonoBehaviour
{
    /// <summary>ネットワークオブジェクトを破棄するための PhotonView の参照</summary>
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();

        // ParticleSystem の再生が終わったら破棄する
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, ps.main.duration);
    }

    /// <summary>
    /// ブロックに Trigger が当たったら破棄する
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 自分のクライアントから、相手のオブジェクトをネットワークオブジェクトとして破棄する
        if (view && view.IsMine)
        {
            if (other.gameObject.tag == "DestructableBlock")
            {
                // ブロックの PhotonView を取得し、所有権を取ってネットワークオブジェクトとして破棄する
                PhotonView view = other.gameObject.GetComponent<PhotonView>();
                view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                PhotonNetwork.Destroy(view);
            }
        }
    }
}
