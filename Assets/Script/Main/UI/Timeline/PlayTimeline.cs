using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class PlayTimeline : ChangeVCam
{
    [SerializeField, Header("イベント"), Tooltip("ステージクリア")]
    private PlayableDirector PlayableDirector_GameClear;

    private GameManager m_gameManager;
    private SaveDataManager m_saveDataManager;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_saveDataManager = GameManager.Instance.SaveDataManager;
    }

    /// <summary>
    /// ステージクリア時の処理。
    /// </summary>
    public void GameClear()
    {
        ResetPriority();
        ChangeVcam(1);
        // タイムラインを再生。
        PlayableDirector_GameClear.Play();
    }

    /// <summary>
    /// タイムライン再生を終了。
    /// </summary>
    public void Finish()
    {
        PlayableDirector_GameClear.Stop();
        m_gameManager.GameMode = CurrentGameMode.enClear;
        m_saveDataManager.SaveData.saveData.ClearStage[m_gameManager.StageID] = true;
        m_saveDataManager.Save();
    }

    /// <summary>
    /// UIを非表示にする。
    /// </summary>
    public void NotActiveUI()
    {
        GameObject.FindGameObjectWithTag("MainUI_PlayerStatus").GetComponent<Animator>().SetTrigger("NotActive");
        GameObject.FindGameObjectWithTag("MainUI_HelpPanel").GetComponent<Animator>().SetTrigger("NotActive");
    }
}
