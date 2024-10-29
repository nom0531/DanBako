using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSwitch_Title : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�"),Tooltip("�͂��߂���/�Â�����")]
    SceneChange Scene_StageSelect;
    [SerializeField, Tooltip("�I�v�V����")]
    SceneChange Scene_Option;
    [SerializeField, Header("SE"), Tooltip("���艹")]
    SE SE_Determination;
    [SerializeField, Tooltip("�J�[�\���ړ���")]
    SE SE_CursorMove;

    /// <summary>
    /// �I�𒆂̃R�}���h�B
    /// </summary>
    private enum TitleState
    {
        enFromBeginning,    // �͂��߂���B
        enFromContinuation, // �Â�����B
        enQuitGame,         // �Q�[���I���B
        enOption            // �I�v�V�����B
    }

    private SaveDataManager m_saveDataManager;
    private Gamepad m_gamepad;
    private Cursor m_cursor;
    private TitleState m_comandState = TitleState.enFromBeginning;
    private bool m_isPush = false;      // �{�^�����������Ȃ�ture�B

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
    }

    // Update is called once per frame
    private void Update()
    {
        // �{�^�����������Ȃ���s���Ȃ��B
        if(m_isPush == true)
        {
            return;
        }
        SelectCommand();
        ButtonDown();
    }

    /// <summary>
    /// �R�}���h�I�������B
    /// </summary>
    private void SelectCommand()
    {
        // �Q�[���p�b�h���擾�B
        m_gamepad = Gamepad.current;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushDown();
        }

        if(m_gamepad == null)
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
        if (m_comandState < TitleState.enFromBeginning)
        {
            m_comandState = TitleState.enOption;
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
        if (m_comandState > TitleState.enOption)
        {
            m_comandState = TitleState.enFromBeginning;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// �{�^�������B
    /// </summary>
    private void ButtonDown()
    {
        // B�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            m_isPush = true;
        }
    }

    /// <summary>
    /// �{�^�����������Ƃ��̏����B
    /// </summary>
    private void ButtonPush()
    {
        // �X�e�[�g�ɉ����ď�����ύX�B
        switch (m_comandState){
            case TitleState.enFromBeginning:
                FromBiginning();
                break;
            case TitleState.enFromContinuation:
                FromContinuation();
                break;
            case TitleState.enQuitGame:
                Quit();
                break;
            case TitleState.enOption:
                Option();
                break;
        }
        SE_Determination.PlaySE();
    }

    /// <summary>
    /// �͂��߂����I�������ꍇ�̏����B
    /// </summary>
    private void FromBiginning()
    {
        m_saveDataManager.Delete(); // �ȑO�܂ł̃f�[�^���폜�B
        Scene_StageSelect.CreateFadeCanvas();
    }

    /// <summary>
    /// �Â������I�������ꍇ�̏����B
    /// </summary>
    private void FromContinuation()
    {
        Scene_StageSelect.CreateFadeCanvas();
    }

    /// <summary>
    /// �Q�[�����I�����鏈���B
    /// </summary>
    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// �I�v�V�����I�����̏����B
    /// </summary>
    private void Option()
    {
        Scene_Option.CreateFadeCanvas();
    }
}
