using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan : MonoBehaviour
{
    [SerializeField, Header("パラメータ"), Tooltip("スケールを行う速度")]
    private float ScaleSpeed = 8.0f;
    [SerializeField, Tooltip("透明化するまでの時間")]
    private float FadeDelay = 0.1f;

    private Material m_material;
    private float m_elapsedTime = 0.0f;     // タイマー。

    // Start is called before the first frame update
    private void Start()
    {
        m_material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        Staging();
    }

    /// <summary>
    /// 演出を行う。
    /// </summary>
    private void Staging()
    {
        // スケールを増加させる。
        transform.localScale += Vector3.one * ScaleSpeed * Time.unscaledDeltaTime;

        // 経過時間を更新。
        m_elapsedTime += Time.time;

        // 一定時間後に透明化。
        if (m_elapsedTime >= FadeDelay)
        {
            Color newColor = m_material.color;
            newColor.a = Mathf.Lerp(m_material.color.a, 0.0f, Time.time * 2.0f);
            m_material.SetColor("_Color", newColor);
        }
    }
}
