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
    [SerializeField, Header("�t�F�[�h���x")]
    private float FadeSpeed = 1.0f;

    private string m_sceneName = "";
    private Image m_image;
    private bool m_useImage = false;        // true�Ȃ�Image�Afalse�Ȃ�}�e���A�����g�p����B
    private bool m_fadeStart = false;
    private bool m_fadeMode = false;
    private float m_alpha = 0.0f;

    /// <summary>
    /// �t�F�[�h���J�n����B
    /// </summary>
    /// <param name="number">�V�[���ԍ�</param>
    /// <param name="color">�}�e���A���̃J���[�B</param>
    /// <param name="flag">true�Ȃ�Image�Afalse�Ȃ�}�e���A�����g�p����B</param>
    public void FadeStart(SceneNumber number, Color color, bool flag)
    {
        m_fadeStart = true;
        m_sceneName = ConvertingToName(number);
        m_useImage = flag;
        m_image = transform.GetChild(0).GetComponent<Image>();

        if (m_useImage)
        {
            // �ʏ�̃t�F�[�h�B
            m_image.material = null;
            m_image.color = color;
        }
        else
        {
            // �}�e���A�����������B
            m_image.material.SetFloat("_Border", 0.0f);
            m_image.material.SetColor("_Color", color);

            // ���g��RenderCamera�Ƀ��C���J������ݒ肷��B
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        // BGM�̉��ʂ�����������B
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
            // �J������ݒ�B
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        Fade();
        SetAlpha();
    }

    /// <summary>
    /// �V�[�����ɕϊ��B
    /// </summary>
    /// <param name="number">�V�[���ԍ��B</param>
    /// <returns>�V�[�����B</returns>
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
    /// �t�F�[�h�����B
    /// </summary>
    private void Fade()
    {
        if (m_fadeMode == false)
        {
            // ��ʂ��Â�����B
            m_alpha += FadeSpeed * Time.deltaTime;

            if (m_alpha >= 1.0f)
            {
                // �V�[�������[�h�B
                SceneManager.LoadScene(m_sceneName);
                m_fadeMode = true;
            }
        }
        else
        {
            // ��ʂ𖾂邭����B
            m_alpha -= FadeSpeed * Time.deltaTime;

            if (m_alpha <= 0.0f)
            {
                // ���g���폜�B
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// �s�����x��ݒ肷��B
    /// </summary>
    private void SetAlpha()
    {
        if(m_useImage == true)
        {
            // �J���[��ݒ�B
            Color nowColor = m_image.color;
            nowColor.a = m_alpha;
            m_image.color = nowColor;
        }
        else
        {
            // �}�e���A���ɒl��ݒ�B
            m_image.material.SetFloat("_Border", m_alpha);
        }
    }
}
