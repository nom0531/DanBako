using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤータグがついているか判別するクラス。
/// </summary>
public class PlayerCheck : MonoBehaviour
{
    GameTime m_gameTime;
    bool m_stopTimeFlag = false;

    private void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTime>();
    }

    private void Update()
    {
        if (m_stopTimeFlag)
        {
            m_gameTime.StopTime();
            return;
        }
        m_gameTime.DefaultTime();
    }

    // 自身に何かが衝突している間呼ばれる
    void OnTriggerStay(Collider other)
    {
        // 特定のタグが付いたオブジェクトに衝突している
        if (!other.CompareTag("Player"))
        {
            m_stopTimeFlag = false;
            Debug.Log("エリアの範囲外");
            return;
        }
        m_stopTimeFlag = true;
        Debug.Log("エリアの範囲内");
        if (Input.GetKey(KeyCode.Space))
        {
            m_gameTime.AdvanceTime();
        }
    }
}
