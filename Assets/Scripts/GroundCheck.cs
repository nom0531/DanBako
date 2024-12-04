using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地面に設置しているかどうか調べるクラス。
/// </summary>
public class GroundCheck : MonoBehaviour
{
    // 接地しているかを格納する変数
    bool m_isGround = false;

    public bool GroundCheckFlag
    {
        get => m_isGround;
        set => m_isGround = value;
    }

    // 自身に何かが衝突している間呼ばれる
    void OnTriggerStay(Collider other)
    {
        // 地面のタグが付いたオブジェクトに衝突している
        if (other.CompareTag("Ground"))
        {
            m_isGround = true;
        }
    }
}
