using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[�����[�h�B
/// </summary>
public enum GameMode
{
    enInGame,
    enPause,
    enOutGame
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private GameObject SaveDataManagerObject, SoundManagerObject;

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManamager;
    private GameMode m_gameMode = GameMode.enOutGame;

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
            if (m_soundManamager == null)
            {
                m_soundManamager = FindObjectOfType<SoundManager>();
            }
            return m_soundManamager;
        }
    }

    public GameMode GameMode
    {
        get => m_gameMode;
        set => m_gameMode = value;
    }

#if UNITY_EDITOR
    override protected void Awake()
    {
        // �ǂ̏�ʂɂ����Ă�BGM���Đ�����ׂ�Awake���g�p����B
        CheckInstance();
        // ���g�̓V�[�����ׂ��ł��폜����Ȃ��悤�ɂ���B
        DontDestroyOnLoad(gameObject);
        // �I�u�W�F�N�g���쐬�B
        var saveDataManagerObject = Instantiate(SaveDataManagerObject);
        m_saveDataManager = saveDataManagerObject.GetComponent<SaveDataManager>();
        var soundManagerObject = Instantiate(SoundManagerObject);
        m_soundManamager = soundManagerObject.GetComponent<SoundManager>();
    }

    /// <summary>
    /// �f�o�b�O�R�}���h�B
    /// </summary>
    private void DebugCommand()
    {
        // �f�o�b�O�p�R�}���h�͂����ɋL�ڂ��Ă��������B
        // �������e�Ɋւ��Ă̓R�}���h�ꗗ����m�F���Ă��������B
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveDataManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SaveDataManager.Delete();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
#else
    private void Start()
    {
        // ���g�̓V�[�����ׂ��ł��폜����Ȃ��悤�ɂ���B
        DontDestroyOnLoad(gameObject);
        // �I�u�W�F�N�g���쐬�B
        var saveDataManagerObject = Instantiate(SaveDataManagerObject);
        m_saveDataManager = saveDataManagerObject.GetComponent<SaveDataManager>();
        var soundManagerObject = Instantiate(SoundManagerObject);
        m_soundManamager = soundManagerObject.GetComponent<SoundManager>();
    }
#endif

    private void Update()
    {
#if UNITY_EDITOR
        DebugCommand();
#endif
    }
}