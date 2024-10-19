using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneNumber
{
    enTitle,
    enOption,
    enStageSelect,
    enMain
}

public class FadeScene : SingletonMonoBehaviour<FadeScene>
{
    [SerializeField, Header("フェード速度")]
    private float FadeSpeed = 1.0f;

    private string m_sceneName = "";
    private Image m_image;
    private bool m_useImage = false;        // trueならImage、falseならマテリアルを使用する。
    private bool m_fadeStart = false;
    private bool m_fadeMode = false;
    private float m_alpha = 0.0f;

    /// <summary>
    /// フェードを開始する。
    /// </summary>
    /// <param name="number">シーン番号</param>
    /// <param name="color">マテリアルのカラー。</param>
    /// <param name="flag">trueならImage、falseならマテリアルを使用する。</param>
    public void FadeStart(SceneNumber number, Color color, bool flag)
    {
        m_fadeStart = true;
        m_sceneName = ConvertingToName(number);
        m_useImage = flag;
        m_image = transform.GetChild(0).GetComponent<Image>();

        if (m_useImage)
        {
            // 通常のフェード。
            m_image.material = null;
            m_image.color = color;
        }
        else
        {
            // マテリアルを初期化。
            m_image.material.SetFloat("_Border", 0.0f);
            m_image.material.SetColor("_Color", color);

            // 自身のRenderCameraにメインカメラを設定する。
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        // BGMの音量を小さくする。
        BGM bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        bgm.FadeStart(true);

        DontDestroyOnLoad(gameObject);
    }

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
    /// シーン名に変換。
    /// </summary>
    /// <param name="number">シーン番号。</param>
    /// <returns>シーン名。</returns>
    private string ConvertingToName(SceneNumber number)
    {
        var sceneName = "";
        switch (number)
        {
            case SceneNumber.enTitle:
                sceneName =  "Title";
                break;
            case SceneNumber.enOption:
                sceneName = "Option";
                break;
            case SceneNumber.enStageSelect:
                sceneName = "StageSelect";
                break;
            case SceneNumber.enMain:
                sceneName = "Main";
                break;
        }
        return sceneName;
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
                m_fadeMode = true;
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
