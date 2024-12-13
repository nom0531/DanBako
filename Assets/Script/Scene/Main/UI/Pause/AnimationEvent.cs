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
    [SerializeField]
    private GameObject Option;

    private OptionsMenu_Main m_optionsMenu;
    private Animator m_playerStatusPanelAnimator;
    private Animator m_helpPanelAnimator;
    private Animator m_panelAnimator;

    private void Start()
    {
        m_playerStatusPanelAnimator = PlayerStatusPanel.GetComponent<Animator>();
        m_helpPanelAnimator = HelpPanel.GetComponent<Animator>();
        m_panelAnimator = Panel.GetComponent<Animator>();
        m_optionsMenu = Option.GetComponent<OptionsMenu_Main>();
    }

    /// <summary>
    /// ポーズを開始する処理。
    /// </summary>
    public void PauseStart()
    {
        m_panelAnimator.SetTrigger("Active");
    }

    /// <summary>
    /// ポーズを終了する処理。
    /// </summary>
    public void PauseFinish()
    {
        m_panelAnimator.SetTrigger("NotActive");
    }

    /// <summary>
    /// オプション画面へ遷移する処理。
    /// </summary>
    public void OptionUIStart()
    {
        m_optionsMenu.SelectOptionFlag = true;
        m_panelAnimator.SetTrigger("OptionActive");
    }

    /// <summary>
    /// オプション画面から遷移する処理。
    /// </summary>
    public void OptionFinish()
    {
        m_optionsMenu.SelectOptionFlag = false;
        m_panelAnimator.SetTrigger("OptionNotActive");
    }

    /// <summary>
    /// インゲームのUIの表示を決定する。
    /// </summary>
    public void InGameUI(string trigger)
    {
        m_playerStatusPanelAnimator.SetTrigger(trigger);
        m_helpPanelAnimator.SetTrigger(trigger);
    }
}
