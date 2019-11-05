using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブロックに追加するコンポーネント
/// プラットホームから落ちたブロックを破棄する
/// </summary>
public class DestructableBlockController : MonoBehaviour
{
    void Update()
    {
        // プラットホームから落ちたら破棄する
        if (this.transform.position.y < -1)
        {
            // ネットワークオブジェクトとして破棄せず普通に破棄してしまう
            Destroy(this.gameObject);
        }
    }
}
