﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;   // PhotonNetwork を使うため
using Photon.Realtime;  // RaiseEventOptions/ReceiverGroup を使うため
using ExitGames.Client.Photon;  // SendOptions を使うため

/// <summary>
/// イベントを raise/fire するためのコンポーネント
/// イベントを起こすには PhotonNetwork.RaiseEvent メソッドを呼び出す。
/// イベントを起こすコンポーネントはネットワークコンポーネントやオブジェクトである必要はない。
/// （MonoBehaviourPunCallbacks を継承したり、Photon View をアタッチする必要はない）
/// </summary>
public class RaiseEvent : MonoBehaviour
{
    /// <summary>イベントで送るメッセージ</summary>
    [SerializeField] string m_message;

    /// <summary>
    /// イベントを起こす
    /// </summary>
    public void Raise()
    {
        //イベントとして送るものを作る
        byte eventCode = 0; // イベントコード 0~199 まで指定できる。200 以上はシステムで使われているので使えない。
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,  // 全体に送る 他に MasterClient, Others が指定できる
        };  // イベントの起こし方
        SendOptions sendOptions= new SendOptions(); // オプションだが、特に何も指定しない

        // イベントを起こす
        PhotonNetwork.RaiseEvent(eventCode, m_message, raiseEventOptions, sendOptions);
    }
}