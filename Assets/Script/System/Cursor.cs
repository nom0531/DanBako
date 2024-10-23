using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField,Header("移動先の座標")]
    private Vector3[] Position;
    [SerializeField, Header("移動速度")]
    private float Speed;

    private RectTransform m_rectTransform;
    private Vector3 m_startPosition = Vector3.zero; // 開始点。
    private Vector3 m_endPosition = Vector3.zero;   // 終了点。
    private bool m_isLeap = false;                  // 線形補完を開始したならtrue。
    private float m_time = 0.0f;                    // タイマー。

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = Position[0];
    }

    private void Update()
    {
        Leap();
    }

    /// <summary>
    /// 線形補完の処理。
    /// </summary>
    private void Leap()
    {
        // 経過時間を更新。
        m_time += Time.deltaTime;
        // 割合を計算。
        var t = m_time / Speed;
        // 二点間を線形補完。
        m_rectTransform.anchoredPosition = Vector3.Lerp(m_startPosition, m_endPosition, t);
        // 線形補完が終了したなら。
        if (t >= 1.0f)
        {
            m_time = Speed;
            m_isLeap = false;
        }
    }

    /// <summary>
    /// 移動処理。
    /// </summary>
    /// <param name="number">移動先の番号。</param>
    public void Move(int number)
    {
        // 値を初期化。
        m_startPosition = m_rectTransform.anchoredPosition;
        m_endPosition = Position[number];
        m_time = 0.0f;
        // 線形補完を開始。
        m_isLeap = true;
    }
}
