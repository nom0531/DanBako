using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSwitch_GameOver : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("リトライ")]
    private SceneChange RetryToGame;
    [SerializeField, Tooltip("ステージ選択")]
    private SceneChange StageSelect;
    [SerializeField, Header("切り替えるまでの待機時間")]
    private float WaitTime = 8.0f;
    [SerializeField, Header("Animator")]
    private GameObject GameOverObject;
    [SerializeField]
    private GameObject HelpPanel;
    [SerializeField]
    private GameObject TextPanel;
    [SerializeField, Header("SE"),Tooltip("カーソル移動音")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("決定音")]
    private SE SE_Determination;

    /// <summary>
    /// 選択中のコマンド。
    /// </summary>
    public enum GameOverState
    {
        enRetry,
        enStageSelect,
    }

    private Animator m_gameOverAnimator;
    private Animator m_helpAnimator;
    private Animator m_textAnimator;
    private Cursor m_cursor;
    private Gamepad m_gamepad;
    private GameOverState m_comandState = GameOverState.enRetry;
    private float m_timer = 0.0f;
    private bool m_wait = false;            // タイマーが一定以上になったらture。
    private bool m_changeText = false;      // テキストの表示が変更されたらture。

    // Start is called before the first frame update
    void Start()
    {
        m_gameOverAnimator = GameOverObject.GetComponent<Animator>();
        m_helpAnimator = HelpPanel.GetComponent<Animator>();
        m_textAnimator = TextPanel.GetComponent<Animator>();

        m_gameOverAnimator.SetTrigger("Active");
    }

    // Update is called once per frame
    void Update()
    {
        GameOver();
        ButtonDown();
        CursorMove();
    }

    /// <summary>
    /// GameOver演出。
    /// </summary>
    private void GameOver()
    {
        if(m_wait == true)
        {
            return;
        }

        m_timer += Time.deltaTime;

        // 一定時間が経過したら自動的に切り替える。
        if (m_timer >= WaitTime)
        {
            m_wait = true;
            m_helpAnimator.SetTrigger("Active");
        }
    }

    /// <summary>
    /// カーソル移動。
    /// </summary>
    private void CursorMove()
    {
        if (m_changeText == false)
        {
            return;
        }
        if (m_cursor == null)
        {
            m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        }

        m_gamepad = Gamepad.current;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushDown();
        }

        // ゲームパッドが接続されていない場合。
        if (m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.up.wasPressedThisFrame)
        {
            PushUp();
        }
        if (m_gamepad.dpad.down.wasPressedThisFrame)
        {
            PushDown();
        }
    }

    /// <summary>
    /// ↑キーを押したときの処理。
    /// </summary>
    private void PushUp()
    {
        m_comandState--;
        // 補正。
        if (m_comandState < GameOverState.enRetry)
        {
            m_comandState = GameOverState.enStageSelect;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ↓キーを押したときの処理。
    /// </summary>
    private void PushDown()
    {
        m_comandState++;
        // 補正。
        if (m_comandState > GameOverState.enStageSelect)
        {
            m_comandState = GameOverState.enRetry;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ボタン処理。
    /// </summary>
    private void ButtonDown()
    {
        if (m_changeText == false && m_wait == true)
        {
            // Bボタンを押したとき。
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
            {
                m_changeText = true;
                SE_Determination.PlaySE();
                m_gameOverAnimator.SetTrigger("NotActive");
                m_textAnimator.SetTrigger("Active");
                return;
            }
        }
        
        if(m_changeText == true)
        {
            // Bボタンを押したとき。
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
            {
                SE_Determination.PlaySE();
                ButtonPush();
            }
        }
    }

    /// <summary>
    /// ボタンを押したときの処理。
    /// </summary>
    private void ButtonPush()
    {
        // ステートに応じて処理を変更。
        switch (m_comandState)
        {
            case GameOverState.enRetry:
                RetryToGame.CreateFadeCanvas();
                break;
            case GameOverState.enStageSelect:
                StageSelect.CreateFadeCanvas();
                break;
        }
    }
}
