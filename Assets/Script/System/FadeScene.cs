using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneNumber
{
    enTitle,
    enStageSelect,
    enMain,
    enGameClear,
    enGameOver,
    enNumber
}

public class FadeScene : MonoBehaviour
{
    [SerializeField, Header("フェード速度")]
    private float FadeSpeed = 1.0f;

    private string m_sceneName = "";
    private Image m_image = null;
    private bool m_useImage = false;        // trueならImage、falseならマテリアルを使用する。

    private bool m_fadeStart = false;
    private bool m_fadeMode = false;
    private float m_alpha = 0.0f;

    private void Update()
    {
        if(m_fadeStart == false)
        {
            return;
        }

        if(GetComponent<Canvas>().worldCamera == null
            && m_useImage == false)
        {
            // カメラを設定。
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        Fade();
        SetAlpha();
    }

    /// <summary>
    /// フェード処理。
    /// </summary>
    private void Fade()
    {
        if (m_fadeMode == false)
        {
            // 画面を暗くする。
            m_alpha += FadeSpeed * Time.deltaTime;

            if (m_alpha >= 1.0f)
            {
                // シーンをロード。
                SceneManager.LoadScene(m_sceneName);
                m_fadeMode = false;
            }
        }
        else
        {
            // 画面を明るくする。
            m_alpha -= FadeSpeed * Time.deltaTime;

            if (m_alpha <= 0.0f)
            {
                // 自身を削除。
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 不透明度を設定する。
    /// </summary>
    private void SetAlpha()
    {
        if(m_useImage == true)
        {
            // カラーを設定。
            Color nowColor = m_image.color;
            nowColor.a = m_alpha;
            m_image.color = nowColor;
        }
        else
        {
            // マテリアルに値を設定。
            m_image.material.SetFloat("_Border", m_alpha);
        }
    }
}
