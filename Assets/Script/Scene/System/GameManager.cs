using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームモード。
/// </summary>
public enum CurrentGameMode
{
    enInGame,
    enPause,
    enClear,
    enOutGame
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private GameObject SaveDataManagerObject, SoundManagerObject;

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManager;
    private CurrentGameMode m_gameMode = CurrentGameMode.enOutGame;
    private int m_stageID = 0;

    public SaveDataManager SaveDataManager
    {
        get
        {
            if (m_saveDataManager == null)
            {
                m_saveDataManager = FindObjectOfType<SaveDataManager>();
            }
            return m_saveDataManager;
        }
    }

    public SoundManager SoundManager
    {
        get
        {
            if (m_soundManager == null)
            {
                m_soundManager = FindObjectOfType<SoundManager>();
            }
            return m_soundManager;
        }
    }

    public CurrentGameMode GameMode
    {
        get => m_gameMode;
        set => m_gameMode = value;
    }

    public int StageID
    {
        set => m_stageID = value;
        get => m_stageID;
    }

    protected override void Awake()
    {
        // どの場面においてもBGMを再生する為にAwakeを使用する。
        base.Awake();
        // オブジェクトを作成。
        var saveDataManagerObject = Instantiate(SaveDataManagerObject);
        m_saveDataManager = saveDataManagerObject.GetComponent<SaveDataManager>();
        var soundManagerObject = Instantiate(SoundManagerObject);
        m_soundManager = soundManagerObject.GetComponent<SoundManager>();
    }

#if UNITY_EDITOR
    private void Update()
    {
        DebugCommand();
    }

    /// <summary>
    /// デバッグコマンド。
    /// </summary>
    private void DebugCommand()
    {
        // デバッグ用コマンドはここに記載してください。
        // 処理内容に関してはコマンド一覧から確認してください。
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveDataManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SaveDataManager.Delete();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveDataManager.InitOption();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
#endif
}