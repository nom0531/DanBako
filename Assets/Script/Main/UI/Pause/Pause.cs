using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ポーズステート。
/// </summary>
public enum PauseState
{
    enReturnToGame,
    enOption,
    enRetryToGame,
    enReturnToStageSelect
}

public class Pause : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("ステージセレクト")]
    private SceneChange StageSelect;
    [SerializeField, Tooltip("インゲーム")]
    private SceneChange Main;
    [SerializeField]
    private GameObject Cursor;
    [SerializeField]
    private GameObject PauseCanvas;
    [SerializeField]
    private GameObject Option;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    SE SE_Determination;
    [SerializeField, Tooltip("キャンセル音")]
    SE SE_Cancel;

    private GameManager m_gameManager;
    private OptionsMenu_Main m_optionsMenu;
    private Gamepad m_gamepad;
    private Cursor m_cursor;
    private Animator m_animator;
    private AnimationEvent m_animationEvent;
    private PauseState m_comandState = PauseState.enReturnToGame;
    private bool m_isPause = false;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_animator = PauseCanvas.GetComponent<Animator>();
        m_cursor = Cursor.GetComponent<Cursor>();
        m_optionsMenu = Option.GetComponent<OptionsMenu_Main>();
        m_animationEvent = PauseCanvas.GetComponent<AnimationEvent>();
    }

    private void Update()
    {
        if (m_optionsMenu.SelectOptionFlag == true)
        {
            return;
        }
        PauseScreen();

        if(m_isPause == false)
        {
            return;
        }
        CursorMove();
        SceneChange();
    }

    /// <summary>
    /// ポーズ処理。
    /// </summary>
    private void PauseScreen()
    {
        // ゲームをクリアしたなら実行しない。
        if(m_gameManager.GameMode == CurrentGameMode.enClear)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (m_isPause == true)
            {
                EnterPause();
            }
            else
            {
                ExitPause();
            }
        }

        m_gamepad = Gamepad.current;

        // ゲームパッドが接続されていない場合。
        if (m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.startButton.wasPressedThisFrame)
        {
            if (m_isPause == true)
            {
                EnterPause();
            }
            else
            {
                ExitPause();
            }
        }
    }

    /// <summary>
    /// ポーズ解除。
    /// </summary>
    private void EnterPause()
    {
        // ポーズ解除。
        Time.timeScale = 1.0f;
        // ステートを変更。
        m_gameManager.GameMode = CurrentGameMode.enInGame;
        m_animator.SetTrigger("NotActive");

        SE_Cancel.PlaySE();
        m_isPause = false;
    }

    /// <summary>
    /// ポーズする。
    /// </summary>
    private void ExitPause()
    {
        // ポーズする。
        Time.timeScale = 0.0f;
        // ステートを変更。
        m_gameManager.GameMode = CurrentGameMode.enPause;
        m_animator.SetTrigger("Active");

        m_comandState = PauseState.enReturnToGame;
        SE_Determination.PlaySE();
        m_isPause = true;
    }

    /// <summary>
    /// カーソル移動。
    /// </summary>
    private void CursorMove()
    {
        if(m_cursor == null)
        {
            m_cursor = Cursor.GetComponent<Cursor>();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushDown();
        }

        m_gamepad = Gamepad.current;

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
        if (m_comandState < PauseState.enReturnToGame)
        {
            m_comandState = PauseState.enReturnToStageSelect;
        }
        m_cursor.Move((int)m_comandState);
    }

    /// <summary>
    /// ↓キーを押したときの処理。
    /// </summary>
    private void PushDown()
    {
        m_comandState++;
        // 補正。
        if (m_comandState > PauseState.enReturnToStageSelect)
        {
            m_comandState = PauseState.enReturnToGame;
        }
        m_cursor.Move((int)m_comandState);
    }

    /// <summary>
    /// シーンを変更する。
    /// </summary>
    private void SceneChange()
    {
        // Bボタンを押したとき。
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            SE_Determination.PlaySE();
        }
    }

    /// <summary>
    /// ボタン処理。
    /// </summary>
    private void ButtonPush()
    {
        // ステートに応じて処理を変更。
        switch (m_comandState)
        {
            case PauseState.enReturnToGame:
                EnterPause();
                break;
            case PauseState.enOption:
                m_animationEvent.OptionUIStart();
                break;
            case PauseState.enRetryToGame:
                EnterPause();
                Main.CreateFadeCanvas();
                break;
            case PauseState.enReturnToStageSelect:
                Time.timeScale = 1.0f;
                m_gameManager.GameMode = CurrentGameMode.enOutGame;
                StageSelect.CreateFadeCanvas();
                break;
        }
    }
}
