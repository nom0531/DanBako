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
    [SerializeField, Header("�t�F�[�h���x")]
    private float FadeSpeed = 1.0f;

    private string m_sceneName = "";
    private Image m_image = null;
    private bool m_useImage = false;        // true�Ȃ�Image�Afalse�Ȃ�}�e���A�����g�p����B

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
            // �J������ݒ�B
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        Fade();
        SetAlpha();
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
                m_fadeMode = false;
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
