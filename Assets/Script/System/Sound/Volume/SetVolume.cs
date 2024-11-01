using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// サウンドのステート
/// </summary>
public enum SoundState
{
    enBGM,
    enSE
}

public class SetVolume : MonoBehaviour
{
    [SerializeField]
    private SoundState SoundState;

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManager;
    private Slider m_slider;

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_soundManager = GameManager.Instance.SoundManager;
        m_slider = GetComponent<Slider>();
        InitRate();
    }

    private void Update()
    {
        SetRate();
    }

    /// <summary>
    /// 初期化。
    /// </summary>
    private void InitRate()
    {
        var rate = 0.0f;
        switch (SoundState)
        {
            case SoundState.enBGM:
                rate = m_saveDataManager.SaveData.saveData.BGMVolume;
                break;
            case SoundState.enSE:
                rate = m_saveDataManager.SaveData.saveData.SEVolume;
                m_soundManager.SEVolume = rate;
                break;
        }
        m_slider.value = rate;
    }

    /// <summary>
    /// 音量設定。
    /// </summary>
    private void SetRate()
    {
        if (gameObject.activeSelf == false)
        {
            return;         // オブジェクトが非表示なら実行しない。
        }
        var volume = m_slider.value;
        switch (SoundState)
        {
            case SoundState.enBGM:
                m_soundManager.BGMVolume = volume;
                break;
            case SoundState.enSE:
                m_soundManager.SEVolume = volume;
                break;
        }
    }
}