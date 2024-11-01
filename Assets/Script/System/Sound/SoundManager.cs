using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGMの番号。
/// </summary>
public enum BGMNumber
{
    enTitle,
    enStageSelect,
    enOption,
    enHelp,
    enClear,
    enGameOver,
    enMain_Onece,
    enMain_Second,
    enMain_Third,
    enNum,
}

/// <summary>
/// SEの番号。
/// </summary>
public enum SENumber
{
    enCursorMove,
    enDetermination,
    enCancel,
    enError,
    enNum,
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField, Header("サウンド")]
    private AudioClip[] BGMSounds = new AudioClip[(int)BGMNumber.enNum];
    [SerializeField]
    private AudioClip[] SESounds = new AudioClip[(int)SENumber.enNum];
    [SerializeField, Header("生成するオブジェクト")]
    private GameObject SEObject;

    private const float MAX = 1.0f;
    private const float MIN = 0.0f;
    private const float VOLUME = 0.5f;  // デフォルト。

    private SaveDataManager m_saveDataManager;
    private float m_BGMVolume = 0.5f;
    private float m_SEVolume = 0.5f;

    public float DefaultVolume
    {
        get => VOLUME;
    }

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
    /// 音量の初期化
    /// </summary>
    private void InitVolume()
    {
        BGMVolume = m_saveDataManager.SaveData.saveData.BGMVolume;
        SEVolume = m_saveDataManager.SaveData.saveData.SEVolume;
    }

    /// <summary>
    /// BGMを再生。
    /// </summary>
    /// <param name="number">番号。</param>
    public void PlayBGM(BGMNumber number, GameObject gameObject)
    {
        if(number == BGMNumber.enNum)
        {
            return;
        }

        InitVolume();
        var bgm = gameObject.GetComponent<BGM>();
        var audioSouce = bgm.AudioSource;
        // 音楽の再生を開始。
        audioSouce.clip = BGMSounds[(int)number];
        audioSouce.Play();
        bgm.FadeStart(false);
    }

    /// <summary>
    /// SEを再生。
    /// </summary>
    /// <param name="number">番号。</param>
    public void PlaySE(SENumber number, float decrementValue=0.1f)
    {
        if(number == SENumber.enNum)
        {
            return;
        }

        InitVolume();
        var gameObject = Instantiate(SEObject);
        var audioSouse = gameObject.GetComponent<AudioSource>();
        // 音楽の再生を開始。
        gameObject.GetComponent<DestroySEObject>().PlayFlag = true;
        audioSouse.volume = SEVolume* decrementValue;
        audioSouse.PlayOneShot(SESounds[(int)number]);
    }
}
