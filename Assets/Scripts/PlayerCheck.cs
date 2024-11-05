using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤータグがついているか判別するクラス。
/// </summary>
public class PlayerCheck : MonoBehaviour
{
    [SerializeField]
    private Transform PlayerTransform;
    [SerializeField]
    private float StopRadius = 5.0f;

    GameTime m_gameTime;
    bool m_stopTimeFlag = false;

    private void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTime>();
    }

    private void Update()
    {
        Distance();
    }

    /// <summary>
    /// プレイヤーとの距離を計算する。
    /// </summary>
    private void Distance()
    {
        float distance = Vector3.Distance(transform.position, PlayerTransform.position);

        if (distance <= StopRadius && m_stopTimeFlag == false)
        {
            m_gameTime.StopTime();
            m_stopTimeFlag = true;
        }
        else if (distance > StopRadius && m_stopTimeFlag == true)
        {
            m_gameTime.DefaultTime();
            m_stopTimeFlag = false;
        }
    }
}
