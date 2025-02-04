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
        m_elapsedTime += Time.unscaledDeltaTime;

        // 一定時間後に透明化。
        if (m_elapsedTime >= FadeDelay)
        {
            float newColor = m_material.GetFloat("_Alpha");

            float t = Mathf.Clamp01((m_elapsedTime - FadeDelay) / 1.0f); // 1秒で透明にする
            newColor = Mathf.Lerp(1.0f, 0.0f, t);

            m_material.SetFloat("_Alpha", newColor);

            // 透明になったら削除する。
            if (newColor <= 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }
}
