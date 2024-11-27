using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    private SaveDataManager m_saveDataManager;
    private Animator m_animator;
    private int m_stageID = 0;

    public int StageID
    {
        set => m_stageID = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_animator = GetComponent<Animator>();

        Draw();
    }

    /// <summary>
    /// 表示処理。
    /// </summary>
    public void Draw()
    {
        // クリアしていないなら自身は表示しない。
        if(m_saveDataManager.SaveData.saveData.ClearStage[m_stageID] == false)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        if (m_saveDataManager.SaveData.saveData.DrawStamp[m_stageID] == false)
        {
            PlayAnimation();
            m_saveDataManager.SaveData.saveData.DrawStamp[m_stageID] = true;
            m_saveDataManager.Save();
        }
    }

    /// <summary>
    /// アニメーションを再生する。
    /// </summary>
    private void PlayAnimation()
    {
        m_animator.SetTrigger("Active");
    }
}
