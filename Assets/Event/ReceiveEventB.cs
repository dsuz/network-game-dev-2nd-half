using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;  // EventData を使うため
using Photon.Pun;   // PhotonNetwork を使うため

/// <summary>
/// イベントを受け取るコンポーネント（パターン B）
/// やっていること：
/// 1. ExitGames.Client.Photon.EventData 型の変数をパラメータとして受け取るメソッドを定義する
/// 2. 定義したメソッドを PhotonNetwork.NetworkingClient.EventReceived イベントに登録する
/// 3. イベントが Raise されると登録したメソッドが呼ばれるので、呼ばれた時の処理を実装する
/// イベントを受け取るコンポーネントはネットワークコンポーネントやオブジェクトである必要はない。
/// （MonoBehaviourPunCallbacks を継承したり、Photon View をアタッチする必要はない）
/// </summary>
public class ReceiveEventB : MonoBehaviour
{
    /// <summary>オブジェクトが有効になった時にイベントにメソッドを登録する</summary>
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += EventReceived;
    }

    /// <summary>オブジェクトが無効になった時にイベントからメソッドを解除する</summary>
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= EventReceived;
    }

    /// <summary>
    /// イベントデータとして渡された内容をログに出力する
    /// </summary>
    /// <param name="e">イベントデータ</param>
    void EventReceived(EventData e)
    {
        if ((int)e.Code < 200)  // 200 以上はシステムで使われているので処理しない
        {
            // イベントで受け取った内容をログに出力する
            string message = "EventReceived. EventCode: " + e.Code.ToString() + ", Message: " + e.CustomData.ToString() + ", From: " + e.Sender.ToString();
            Debug.Log(message);
        }
    }
}
