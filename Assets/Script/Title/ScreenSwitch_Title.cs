using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSwitch_Title : MonoBehaviour
{
    [SerializeField, Header("�e�J�ڐ�"),Tooltip("�͂��߂���/�Â�����")]
    SceneChange Scene_StageSelect;
    [SerializeField, Tooltip("�I�v�V����")]
    SceneChange Scene_Option;
    [SerializeField, Header("SE"), Tooltip("���艹")]
    SE SE_Determination;
    [SerializeField, Tooltip("�Z���N�g��")]
    SE SE_Select;

    /// <summary>
    /// �I�𒆂̃R�}���h�B
    /// </summary>
    private enum ComandState
    {
        enFromBeginning,    // �͂��߂���B
        enFromContinuation, // �Â�����B
        enQuitGame,         // �Q�[���I���B
        enOption            // �I�v�V�����B
    }

    private SaveDataManager m_saveDataManager;
    private Gamepad m_gamepad;
    private Cursor m_cursor;
    private ComandState m_comandState = ComandState.enFromBeginning;
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
        SceneChange();
    }

    /// <summary>
    /// �R�}���h�I�������B
    /// </summary>
    private void SelectCommand()
    {
        // �Q�[���p�b�h���擾�B
        m_gamepad = Gamepad.current;

        // ���L�[���������Ƃ��B
        if (m_gamepad.dpad.up.wasPressedThisFrame || Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_comandState--;
            // �␳�B
            if(m_comandState < ComandState.enFromBeginning)
            {
                m_comandState = ComandState.enOption;
            }
            m_cursor.Move((int)m_comandState);
            SE_Select.PlaySE();
        }
        // ���L�[���������Ƃ��B
        if (m_gamepad.dpad.down.wasPressedThisFrame || Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_comandState++;
            // �␳�B
            if (m_comandState > ComandState.enOption)
            {
                m_comandState = ComandState.enFromBeginning;
            }
            m_cursor.Move((int)m_comandState);
            SE_Select.PlaySE();
        }
        Debug.Log(m_comandState);
    }

    /// <summary>
    /// �J�ڏ����B
    /// </summary>
    private void SceneChange()
    {
        // A�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
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
            case ComandState.enFromBeginning:
                FromBiginning();
                break;
            case ComandState.enFromContinuation:
                FromContinuation();
                break;
            case ComandState.enQuitGame:
                Quit();
                break;
            case ComandState.enOption:
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
