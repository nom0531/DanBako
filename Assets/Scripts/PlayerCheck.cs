using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤータグがついているか判別するクラス。
/// </summary>
public class PlayerCheck : MonoBehaviour
{
    [SerializeField]
    private float StopRadius = 5.0f;

    GameTime m_gameTime;
    Vector3 m_targetPosition = Vector3.zero;
    bool m_stopTimeFlag = false;

    private void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTime>();
        m_targetPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
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
        float distance = Vector3.Distance(transform.position, m_targetPosition);

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
