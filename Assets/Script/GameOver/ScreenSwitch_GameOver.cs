using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSwitch_GameOver : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�"), Tooltip("���g���C")]
    private SceneChange RetryToGame;
    [SerializeField, Tooltip("�X�e�[�W�I��")]
    private SceneChange StageSelect;
    [SerializeField, Header("�؂�ւ���܂ł̑ҋ@����")]
    private float WaitTime = 8.0f;
    [SerializeField, Header("Animator")]
    private GameObject GameOverObject;
    [SerializeField]
    private GameObject HelpPanel;
    [SerializeField]
    private GameObject TextPanel;
    [SerializeField, Header("SE"),Tooltip("�J�[�\���ړ���")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("���艹")]
    private SE SE_Determination;

    /// <summary>
    /// �I�𒆂̃R�}���h�B
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
    private bool m_wait = false;            // �^�C�}�[�����ȏ�ɂȂ�����ture�B
    private bool m_changeText = false;      // �e�L�X�g�̕\�����ύX���ꂽ��ture�B

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
    /// GameOver���o�B
    /// </summary>
    private void GameOver()
    {
        if(m_wait == true)
        {
            return;
        }

        m_timer += Time.deltaTime;

        // ��莞�Ԃ��o�߂����玩���I�ɐ؂�ւ���B
        if (m_timer >= WaitTime)
        {
            m_wait = true;
            m_helpAnimator.SetTrigger("Active");
        }
    }

    /// <summary>
    /// �J�[�\���ړ��B
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
        if (m_comandState < GameOverState.enRetry)
        {
            m_comandState = GameOverState.enStageSelect;
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
        if (m_comandState > GameOverState.enStageSelect)
        {
            m_comandState = GameOverState.enRetry;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// �{�^�������B
    /// </summary>
    private void ButtonDown()
    {
        if (m_changeText == false && m_wait == true)
        {
            // B�{�^�����������Ƃ��B
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
            // B�{�^�����������Ƃ��B
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
            {
                SE_Determination.PlaySE();
                ButtonPush();
            }
        }
    }

    /// <summary>
    /// �{�^�����������Ƃ��̏����B
    /// </summary>
    private void ButtonPush()
    {
        // �X�e�[�g�ɉ����ď�����ύX�B
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
