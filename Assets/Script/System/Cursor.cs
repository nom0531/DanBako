using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField,Header("移動先の座標")]
    private Vector2[] Position;
    [SerializeField, Header("移動速度")]
    private float Speed;

    private RectTransform m_rectTransform;
    private Vector2 m_startPosition = Vector2.zero;     // 開始点。
    private Vector2 m_endPosition = Vector2.zero;       // 終了点。
    private float m_time = 0.0f;                        // タイマー。
    private bool m_isStart = false;                     // 線形補完を開始するならture。

    public Vector2[] SetPosition
    {
        set => Position = value;
        get => Position;
    }

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
        if(m_isStart == false)
        {
            return;
        }

        // 経過時間を更新。
        m_time += Time.unscaledDeltaTime;
        // 割合を計算。
        var t = Mathf.Clamp01(m_time / Speed);
        // 二点間を線形補完。
        m_rectTransform.anchoredPosition = Vector2.Lerp(m_startPosition, m_endPosition, t);

        // 線形補完が終了したなら。
        if (t >= 1.0f)
        {
            m_rectTransform.anchoredPosition = m_endPosition;
            m_isStart = false;
            return;
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
        // 線形補完を開始する。
        m_isStart = true;
    }
}
