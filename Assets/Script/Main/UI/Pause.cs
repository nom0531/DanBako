using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �|�[�Y�X�e�[�g�B
/// </summary>
public enum PauseState
{
    enReturnToGame,
    enRetryToGame,
    enReturnToStageSelect
}

public class Pause : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�"), Tooltip("�X�e�[�W�Z���N�g")]
    private SceneChange StageSelect;
    [SerializeField, Tooltip("�C���Q�[��")]
    private SceneChange Main;
    [SerializeField]
    private GameObject Canvas;
    [SerializeField, Header("�������")]
    private GameObject HelpPanel;
    [SerializeField, Header("SE"), Tooltip("���艹")]
    SE SE_Determination;
    [SerializeField, Tooltip("�L�����Z����")]
    SE SE_Cancel;
    [SerializeField, Tooltip("�J�[�\���ړ���")]
    SE SE_CursorMove;

    private GameManager m_gameManager;
    private Gamepad m_gamepad;
    private Cursor m_cursor;
    private Animator m_pauseAnimator;
    private Animator m_helpAnimator;
    private PauseState m_comandState = PauseState.enReturnToGame;
    private bool m_isPause = false;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_pauseAnimator = Canvas.GetComponent<Animator>();
        m_helpAnimator = HelpPanel.GetComponent<Animator>();
        m_cursor = null;
    }

    private void Update()
    {
        PauseScreen();

        if(m_isPause == false)
        {
            return;
        }
        CursorMove();
        SceneChange();
    }

    /// <summary>
    /// �|�[�Y�����B
    /// </summary>
    private void PauseScreen()
    {
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

        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�B
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
    /// �|�[�Y�����B
    /// </summary>
    private void EnterPause()
    {
        // �|�[�Y�����B
        Time.timeScale = 1.0f;
        // �X�e�[�g��ύX�B
        m_gameManager.GameMode = CurrentGameMode.enInGame;
        m_helpAnimator.SetTrigger("Active");
        m_pauseAnimator.SetTrigger("NotActive");

        SE_Cancel.PlaySE();
        m_isPause = false;
    }

    /// <summary>
    /// �|�[�Y����B
    /// </summary>
    private void ExitPause()
    {
        // �|�[�Y����B
        Time.timeScale = 0.0f;
        // �X�e�[�g��ύX�B
        m_gameManager.GameMode = CurrentGameMode.enPause;
        m_helpAnimator.SetTrigger("NotActive");
        m_pauseAnimator.SetTrigger("Active");

        SE_Determination.PlaySE();
        m_isPause = true;
    }

    /// <summary>
    /// �J�[�\���ړ��B
    /// </summary>
    private void CursorMove()
    {
        if(m_cursor == null)
        {
            m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
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

        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�B
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
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushUp()
    {
        m_comandState--;
        // �␳�B
        if (m_comandState < PauseState.enReturnToGame)
        {
            m_comandState = PauseState.enReturnToStageSelect;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushDown()
    {
        m_comandState++;
        // �␳�B
        if (m_comandState > PauseState.enReturnToStageSelect)
        {
            m_comandState = PauseState.enReturnToGame;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// �V�[����ύX����B
    /// </summary>
    private void SceneChange()
    {
        // B�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            SE_Determination.PlaySE();
        }
    }

    /// <summary>
    /// �{�^�������B
    /// </summary>
    private void ButtonPush()
    {
        // �X�e�[�g�ɉ����ď�����ύX�B
        switch (m_comandState)
        {
            case PauseState.enReturnToGame:
                EnterPause();
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
