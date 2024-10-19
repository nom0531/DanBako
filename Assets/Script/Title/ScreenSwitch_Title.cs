using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Title : MonoBehaviour
{
    /// <summary>
    /// �I�𒆂̃R�}���h�B
    /// </summary>
    private enum ComandState
    {
        enFromBeginning,    // �͂��߂���B
        enFromContinuation, // �Â�����B
        enGameEnd,          // �Q�[���I���B
        enOption            // �I�v�V�����B
    }

    [SerializeField, Header("�e�J�ڐ�"),Tooltip("�͂��߂���/�Â�����")]
    SceneChange Scene_StageSelect;
    [SerializeField, Tooltip("�I�v�V����")]
    SceneChange Scene_Option;
    [SerializeField, Header("SE"), Tooltip("���艹")]
    SE SE_Determination;
    [SerializeField, Tooltip("�Z���N�g��")]
    SE SE_Select;
    [SerializeField, Header("Animation")]
    Animator[] Animators;

    private SaveDataManager m_saveDataManager;
    private ComandState m_comandState = ComandState.enFromBeginning;
    private bool m_isPush = false;      // �{�^�����������Ȃ�ture�B

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
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

        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            m_isPush = true;
        }
    }

    /// <summary>
    /// �R�}���h�I�������B
    /// </summary>
    private void SelectCommand()
    {
        Animators[(int)m_comandState].SetBool("IsSelect", false);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_comandState--;
            if(m_comandState < ComandState.enFromBeginning)
            {
                m_comandState = ComandState.enOption;
            }
            SE_Determination.PlaySE();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_comandState++;
            if (m_comandState > ComandState.enOption)
            {
                m_comandState = ComandState.enFromBeginning;
            }
            SE_Determination.PlaySE();
        }
        Animators[(int)m_comandState].SetBool("IsSelect",true);
        Animators[(int)m_comandState].SetTrigger("Select");
    }

    /// <summary>
    /// �{�^�����������Ƃ��̏���
    /// </summary>
    private void ButtonPush()
    {
        switch (m_comandState){
            case ComandState.enFromBeginning:
                FromBiginning();
                break;
            case ComandState.enFromContinuation:
                FromContinuation();
                break;
            case ComandState.enGameEnd:
                Quit();
                break;
            case ComandState.enOption:
                Option();
                break;
        }
        SE_Select.PlaySE();
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
