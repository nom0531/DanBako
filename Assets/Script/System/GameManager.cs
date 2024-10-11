using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private GameObject SaveDataManagerObject, SoundManagerObject;

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManamager;

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

    private void Awake()
    {
        // 自身はシーンを跨いでも削除されないようにする。
        DontDestroyOnLoad(gameObject);
        // オブジェクトを作成。
        var saveDataManagerObject = Instantiate(SaveDataManagerObject);
        m_saveDataManager = saveDataManagerObject.GetComponent<SaveDataManager>();
        var soundManagerObject = Instantiate(SoundManagerObject);
        m_soundManamager = soundManagerObject.GetComponent<SoundManager>();
    }

#if UNITY_EDITOR
    private void Update()
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
#endif
}