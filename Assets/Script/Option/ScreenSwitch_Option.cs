using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �I�𒆂̃R�}���h�B
/// </summary>
public enum OptionState
{
    enBGMSound,
    enSESound,
    enCamera,
    enReset,
    enBGMParamator,     // BGM�̃p�����[�^�B
    enSEParamator,      // SE�̃p�����[�^�B
    enCameraParamator,  // �J�����̃p�����[�^�B
}

public class ScreenSwitch_Option : MonoBehaviour
{
    [SerializeField, Header("SE"), Tooltip("���艹")]
    private SE SE_Determination;
    [SerializeField, Tooltip("�L�����Z����")]
    private SE SE_Cancel;
    [SerializeField, Tooltip("�J�[�\���ړ���")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("�G���[��")]
    private SE SE_Error;

    private SaveDataManager m_saveDataManager;
    private Cursor m_cursor;
    private SceneChange m_sceneChange;
    private SetParamator m_setParamator;
    private Gamepad m_gamepad;
    private OptionState m_comandState = OptionState.enBGMSound;
    private bool m_isPush = false;    // �{�^�����������Ȃ�ture�B

    // Start is called before the first frame update
    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        m_sceneChange = GetComponent<SceneChange>();
        m_setParamator = GetComponent<SetParamator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_isPush == true)
        {
            return;
        }
        SelectCommand();
        ButtonDown();
    }

    /// <summary>
    /// �{�^�������B
    /// </summary>
    private void ButtonDown()
    {
        // A�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            SceneChange();
            ChangeState();
            SE_Cancel.PlaySE();
            m_saveDataManager.Save();
        }
        // B�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            SE_Determination.PlaySE();
        }
    }

    /// <summary>
    /// �V�[����؂�ւ��鏈���B
    /// </summary>
    private void SceneChange()
    {
        if(m_comandState > OptionState.enReset)
        {
            return;
        }
        // �V�[����؂�ւ���B
        m_sceneChange.CreateFadeCanvas();
        m_isPush = true;
    }

    /// <summary>
    /// �X�e�[�g��ύX����B
    /// </summary>
    private void ChangeState()
    {
        if(m_comandState < OptionState.enBGMParamator)
        {
            return;
        }
        // �I�𒆂̃R�}���h��ύX����B
        m_comandState -= 4;
        m_cursor.Move((int)m_comandState);
    }

    /// <summary>
    /// �R�}���h�I�������B
    /// </summary>
    private void SelectCommand()
    {
        // �Q�[���p�b�h���擾�B
        m_gamepad = Gamepad.current;

        CursorMove();
        m_setParamator.Set(m_comandState, m_gamepad);
    }

    /// <summary>
    /// �J�[�\���ړ��B
    /// </summary>
    private void CursorMove()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushDown();
        }

        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�B
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
        if (m_comandState > OptionState.enReset)
        {
            SE_Error.PlaySE();
            return;
        }
        m_comandState--;
        // �␳�B
        if (m_comandState < OptionState.enBGMSound)
        {
            m_comandState = OptionState.enReset;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushDown()
    {
        if (m_comandState > OptionState.enReset)
        {
            SE_Error.PlaySE();
            return;
        }
        m_comandState++;
        // �␳�B
        if (m_comandState > OptionState.enReset)
        {
            m_comandState = OptionState.enBGMSound;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// �{�^�����������Ƃ��̏����B
    /// </summary>
    private void ButtonPush()
    {
        // �X�e�[�g�ɉ����ď�����ύX�B
        switch (m_comandState)
        {
            case OptionState.enBGMSound:
                m_comandState = OptionState.enBGMParamator;
                break;
            case OptionState.enSESound:
                m_comandState = OptionState.enSEParamator;
                break;
            case OptionState.enCamera:
                m_comandState = OptionState.enCameraParamator;
                break;
            case OptionState.enReset:
                m_setParamator.ResetStatus();
                break;
        }
        m_cursor.Move((int)m_comandState);
    }
}
