using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField, Header("�Đ�����BGM�̔ԍ�")]
    private BGMNumber BGMNumber;
    [SerializeField, Header("�t�F�[�h�̑��x")]
    private float FadeSpeed = 1.0f;     // �t�F�[�h�̑��x�i�傫���قǑ����j

    public BGMNumber SetBGM
    {
        set => BGMNumber = value;
    }

    private const float DECREMENT_VALUE = 0.1f;

    private AudioSource m_audioSource;
    private SoundManager m_soundManager;

    // BGM�̃t�F�[�h
    float m_volume = 0.0f;              // ���݂̃{�����[���B
    bool m_fadeMode = false;            // �t�F�[�h�̎�� false=���񂾂�傫�� true=���񂾂񏬂����B
    bool m_isFade = false;              // �t�F�[�h�������Ȃ�true�B
    bool m_isResetVolume = false;       // ���ʂ�Đݒ肷��Ȃ�true�B

    public AudioSource AudioSource
    {
        get => m_audioSource;
    }

    private void Start()
    {
        m_soundManager = GameManager.Instance.SoundManager;
        m_audioSource = GetComponent<AudioSource>();
        m_soundManager.PlayBGM(BGMNumber, gameObject);
    }

    /// <summary>
    /// �t�F�[�h�J�n�B
    /// </summary>
    /// <param name="mode">true�Ȃ�Đ���J�n�Bfalse�Ȃ�Đ���I������B</param>
    public void FadeStart(bool mode)
    {
        // �����ݒ�B
        m_fadeMode = mode;
        m_isFade = true;

        // ���ʂ�������B
        if (mode == false)
        {
            m_volume = 0.0f;
        }
        else
        {
            m_volume = m_soundManager.BGMVolume;
        }
    }

    /// <summary>
    /// ���ʂ�Đݒ肷��B
    /// </summary>
    public void ResetVolume()
    {
        // �������B
        m_fadeMode = ComparisonValue(m_volume);
        m_isFade = true;
        m_isResetVolume = true;
    }

    /// <summary>
    /// �l���r����B
    /// </summary>
    /// <param name="value">��r����l</param>
    /// <returns>�t�F�[�h�̃��[�h�B</returns>
    private bool ComparisonValue(float value)
    {
        if (m_soundManager.BGMVolume > value)
        {
            return false;
        }
        return true;
    }

    private void Update()
    {
        // �t�F�[�h���łȂ��Ȃ璆�f�B
        if (m_isFade == false)
        {
            return;
        }

        if (m_fadeMode == false)
        {
            // ���ʂ�傫������B
            m_volume += FadeSpeed * Time.deltaTime;

            // ���ʂ�ݒ�B
            m_audioSource.volume = m_volume * DECREMENT_VALUE;

            if (m_volume >= m_soundManager.BGMVolume)
            {
                // ���ʂ��ő�ɂȂ�����I���B
                m_isFade = false;
            }
        }
        else
        {
            // ���ʂ����������B
            m_volume -= FadeSpeed * Time.deltaTime;

            // ���ʂ�ݒ�B
            m_audioSource.volume = m_volume * DECREMENT_VALUE;

            if (m_isResetVolume == true)
            {
                if (m_volume <= m_soundManager.BGMVolume)
                {
                    // ���ʂ������ɂȂ�����I���B
                    m_volume = m_soundManager.BGMVolume;
                    m_isFade = false;
                    m_isResetVolume = false;
                }
                return;
            }

            if (m_volume < 0.0f)
            {
                // ���ʂ��ŏ��ɂȂ�����I���B
                m_volume = 0.0f;
                m_audioSource.volume = 0.0f;
                m_isFade = false;
            }
        }
    }
}
