using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class PlayTimeline : ChangeVCam
{
    [SerializeField, Header("イベント"), Tooltip("ステージクリア")]
    private PlayableDirector PlayableDirector_GameClear;
    [SerializeField, Header("StageClear用のTransform")]
    private Transform VcamTransform;

    private GameManager m_gameManager;
    private SaveDataManager m_saveDataManager;
    private bool m_isPlay = false;              // タイムラインを再生したらtrue。

    public bool PlayTimeLineFlag
    {
        get => m_isPlay;
    }

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        SetEventPosition(VcamTransform, 1);
    }

    private void FixedUpdate()
    {
        SetEventPosition(VcamTransform, 1);
    }

    /// <summary>
    /// ステージクリア時の処理。
    /// </summary>
    public void GameClear()
    {
        if(m_isPlay == true)
        {
            return;
        }
        ResetPriority();
        ChangeVcam(1);
        // タイムラインを再生。
        PlayableDirector_GameClear.Play();
        m_isPlay = true;
    }

    /// <summary>
    /// タイムライン再生を終了。
    /// </summary>
    public void Finish()
    {
        PlayableDirector_GameClear.Stop();
        m_saveDataManager.SaveData.saveData.StageData[m_gameManager.StageID].ClearFlag = true;
        m_saveDataManager.Save();
        m_isPlay = false;
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
