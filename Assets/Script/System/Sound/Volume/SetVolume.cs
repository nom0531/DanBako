using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �T�E���h�̃X�e�[�g
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
    /// �������B
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
    /// ���ʐݒ�B
    /// </summary>
    private void SetRate()
    {
        if (gameObject.activeSelf == false)
        {
            return;         // �I�u�W�F�N�g����\���Ȃ���s���Ȃ��B
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