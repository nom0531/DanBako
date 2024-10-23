using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Option : MonoBehaviour
{
    [SerializeField, Header("SE"), Tooltip("���艹")]
    SE SE_Determination;
    [SerializeField, Tooltip("�L�����Z����")]
    SE SE_Chancel;
    [SerializeField, Tooltip("�Z���N�g��")]
    SE SE_Select;

    /// <summary>
    /// �I�𒆂̃R�}���h�B
    /// </summary>
    private enum ComandState
    {
        enBGMSound,
        enSESound,
        enCamera,
        enReset
    }

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManager;
    private SceneChange m_sceneChange;
    private ComandState m_comandState = ComandState.enBGMSound;
    private bool m_isPush = false;    // �{�^�����������Ȃ�ture�B

    // Start is called before the first frame update
    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_soundManager = GameManager.Instance.SoundManager;
        m_sceneChange = GetComponent<SceneChange>();
    }

    // Update is called once per frame
    private void Update()
    {
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
        // ���L�[���������Ƃ��B
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_comandState--;
            // �␳�B
            if (m_comandState < ComandState.enBGMSound)
            {
                m_comandState = ComandState.enReset;
            }
            SE_Select.PlaySE();
        }
        // ���L�[���������Ƃ��B
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_comandState++;
            // �␳�B
            if (m_comandState > ComandState.enReset)
            {
                m_comandState = ComandState.enBGMSound;
            }
            SE_Select.PlaySE();
        }
        Debug.Log(m_comandState);
    }

    /// <summary>
    /// �J�ڏ����B
    /// </summary>
    private void SceneChange()
    {
        // B�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            m_sceneChange.CreateFadeCanvas();
            SE_Chancel.PlaySE();
            m_isPush = true;
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
            case ComandState.enBGMSound:
                break;
            case ComandState.enSESound:
                break;
            case ComandState.enCamera:
                break;
            case ComandState.enReset:
                ResetStatus();
                break;
        }
        SE_Determination.PlaySE();
    }

    /// <summary>
    /// �f�[�^������������B
    /// </summary>
    private void ResetStatus()
    {
        // ���ʂ��f�t�H���g�ɖ߂��B

        // �J������]���f�t�H���g�ɖ߂��B
        Debug.Log("�����l�ɖ߂�");
    }
}
