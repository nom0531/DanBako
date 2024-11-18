using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField, Header("再生するBGMの番号")]
    private BGMNumber BGMNumber;
    [SerializeField, Header("フェードの速度")]
    private float FadeSpeed = 1.0f;     // フェードの速度（大きいほど速い）

    public BGMNumber SetBGM
    {
        set => BGMNumber = value;
    }

    private const float DECREMENT_VALUE = 0.1f;

    private AudioSource m_audioSource;
    private SoundManager m_soundManager;

    // BGMのフェード
    float m_volume = 0.0f;              // 現在のボリューム。
    bool m_fadeMode = false;            // フェードの種類 false=だんだん大きく true=だんだん小さく。
    bool m_isFade = false;              // フェード処理中ならtrue。
    bool m_isResetVolume = false;       // 音量を再設定するならtrue。

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
    /// フェード開始。
    /// </summary>
    /// <param name="mode">trueなら再生を開始。falseなら再生を終了する。</param>
    public void FadeStart(bool mode)
    {
        // 初期設定。
        m_fadeMode = mode;
        m_isFade = true;

        // 音量を初期化。
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
    /// 音量を再設定する。
    /// </summary>
    public void ResetVolume()
    {
        // 初期化。
        m_fadeMode = ComparisonValue(m_volume);
        m_isFade = true;

        if(m_volume > 0.0f)
        {
            m_isResetVolume = true;
        }
    }

    /// <summary>
    /// 値を比較する。
    /// </summary>
    /// <param name="value">比較する値</param>
    /// <returns>フェードのモード。</returns>
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
        // フェード中でないなら中断。
        if (m_isFade == false)
        {
            return;
        }

        if (m_fadeMode == false)
        {
            // 音量を大きくする。
            m_volume += FadeSpeed * Time.deltaTime;

            // 音量を設定。
            m_audioSource.volume = m_volume * DECREMENT_VALUE;

            if (m_volume >= m_soundManager.BGMVolume)
            {
                // 音量が最大になったら終了。
                m_isFade = false;
            }
        }
        else
        {
            // 音量を小さくする。
            m_volume -= FadeSpeed * Time.deltaTime;

            // 音量を設定。
            m_audioSource.volume = m_volume * DECREMENT_VALUE;

            if (m_isResetVolume == true)
            {
                if (m_volume <= m_soundManager.BGMVolume)
                {
                    // 音量が同じになったら終了。
                    m_volume = m_soundManager.BGMVolume;
                    m_isFade = false;
                    m_isResetVolume = false;
                }
                return;
            }

            if (m_volume <= 0.0f)
            {
                // 音量が最小になったら終了。
                m_volume = 0.0f;
                m_audioSource.volume = 0.0f;
                m_isFade = false;
            }
        }
    }
}
