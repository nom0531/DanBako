using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM�̔ԍ��B
/// </summary>
public enum BGMNumber
{
    enTitle,
    enStageSelect,
    enOption,
    enHelp,
    enMain_Onece,
    enMain_Second,
    enMain_Third,
    enNum,
}

/// <summary>
/// SE�̔ԍ��B
/// </summary>
public enum SENumber
{
    enCursorMove,
    enDetermination,
    enCancel,
    enError,
    enDamage,
    enGameClear,
    enGameOver,
    enNum,
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField, Header("�T�E���h")]
    private AudioClip[] BGMSounds = new AudioClip[(int)BGMNumber.enNum];
    [SerializeField]
    private AudioClip[] SESounds = new AudioClip[(int)SENumber.enNum];
    [SerializeField, Header("��������I�u�W�F�N�g")]
    private GameObject SEObject;

    private const float MAX = 1.0f;
    private const float MIN = 0.0f;

    private SaveDataManager m_saveDataManager;
    private float m_BGMVolume = 0.0f;
    private float m_SEVolume = 0.0f;

    public float BGMVolume
    {
        get => m_BGMVolume;
        set
        {
            if(value > MAX)
            {
                value = MAX;
            }
            if(value < MIN)
            {
                value = MIN;
            }
            m_BGMVolume = value;
        }
    }

    public float SEVolume
    {
        get => m_SEVolume;
        set
        {
            if (value > MAX)
            {
                value = MAX;
            }
            if (value < MIN)
            {
                value = MIN;
            }
            m_SEVolume = value;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        InitVolume();
    }

    /// <summary>
    /// ���ʂ̏�����
    /// </summary>
    private void InitVolume()
    {
        BGMVolume = m_saveDataManager.SaveData.saveData.BGMVolume;
        SEVolume = m_saveDataManager.SaveData.saveData.SEVolume;
    }

    /// <summary>
    /// BGM��Đ��B
    /// </summary>
    /// <param name="number">�ԍ��B</param>
    public void PlayBGM(BGMNumber number, GameObject gameObject)
    {
        if(number == BGMNumber.enNum)
        {
            return;
        }

        InitVolume();
        var bgm = gameObject.GetComponent<BGM>();
        var audioSouce = bgm.AudioSource;
        // ���y�̍Đ���J�n�B
        audioSouce.clip = BGMSounds[(int)number];
        audioSouce.Play();
        bgm.FadeStart(false);
    }

    /// <summary>
    /// SE��Đ��B
    /// </summary>
    /// <param name="number">�ԍ��B</param>
    public void PlaySE(SENumber number, float decrementValue=0.1f)
    {
        if(number == SENumber.enNum)
        {
            return;
        }

        InitVolume();
        var gameObject = Instantiate(SEObject);
        var audioSouse = gameObject.GetComponent<AudioSource>();
        // ���y�̍Đ���J�n�B
        gameObject.GetComponent<DestroySEObject>().PlayFlag = true;
        audioSouse.volume = SEVolume* decrementValue;
        audioSouse.PlayOneShot(SESounds[(int)number]);
    }
}
