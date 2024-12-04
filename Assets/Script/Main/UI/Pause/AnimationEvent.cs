using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField,Tooltip("プレイヤーステータス")]
    private GameObject PlayerStatusPanel;
    [SerializeField,Tooltip("インゲーム内でのヘルプ")]
    private GameObject HelpPanel;
    [SerializeField]
    private GameObject Panel;

    private Animator m_playerStatusPanelAnimator;
    private Animator m_helpPanelAnimator;
    private Animator m_panelAnimator;

    private void Start()
    {
        m_playerStatusPanelAnimator = PlayerStatusPanel.GetComponent<Animator>();
        m_helpPanelAnimator = HelpPanel.GetComponent<Animator>();
        m_panelAnimator = Panel.GetComponent<Animator>();
    }

    /// <summary>
    /// ポーズを開始する処理。
    /// </summary>
    public void PauseStart()
    {
        m_panelAnimator.SetTrigger("Active_Main");
    }

    /// <summary>
    /// インゲームのUIの表示を決定する。
    /// </summary>
    public void NotActive_InGameUI(string trigger)
    {
        m_playerStatusPanelAnimator.SetTrigger(trigger);
        m_helpPanelAnimator.SetTrigger(trigger);
    }
}
