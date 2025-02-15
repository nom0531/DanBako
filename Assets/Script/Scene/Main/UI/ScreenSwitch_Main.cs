using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Main : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("GameOver")]
    private SceneChange GameOver;
    [SerializeField, Tooltip("ステージ選択")]
    private SceneChange StageSelect;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    private SE SE_Determination;

    private GameManager m_gameManager;
    private StarCount m_starCount;
    private PlayTimeline m_playTimeline;

    // Start is called before the first frame update
    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_gameManager.GameMode = CurrentGameMode.enInGame;
        m_starCount = GameObject.FindGameObjectWithTag("MainUI_StarCount").GetComponent<StarCount>();
        m_playTimeline = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayTimeline>();
    }

    private void FixedUpdate()
    {
        if (m_starCount.MaxStarCount != m_starCount.NowStarCount)
        {
            return;
        }
        PlayGameClear();
        ButtonPush();
    }

    private void ButtonPush()
    {
        if(m_gameManager.GameMode != CurrentGameMode.enClear)
        {
            return;
        }
        if (m_playTimeline.PlayTimeLineFlag == true)
        {
            return;
        }
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            SE_Determination.PlaySE();
            StageSelect.CreateFadeCanvas();
        }
    }

    /// <summary>
    /// ゲームオーバー演出。
    /// </summary>
    public void PlayGameOver()
    {
        GameOver.CreateFadeCanvas();
    }

    /// <summary>
    /// ゲームクリア演出。
    /// </summary>
    private void PlayGameClear()
    {
        if (m_gameManager.GameMode == CurrentGameMode.enClear)
        {
            return;
        }
        m_gameManager.GameMode = CurrentGameMode.enClear;
        m_playTimeline.GameClear();
    }
}
