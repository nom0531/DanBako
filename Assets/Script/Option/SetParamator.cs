using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SetParamator : MonoBehaviour
{
    [SerializeField, Header("�J����"),Tooltip("�e�L�X�g")]
    private GameObject CameraText;
    [SerializeField, Tooltip("�J�[�\��")]
    private GameObject[] CameraCursor;
    [SerializeField, Header("�T�E���h"), Tooltip("�J�[�\��")]
    private GameObject[] SoundCursor;
    [SerializeField, Tooltip("�e�L�X�g")]
    private GameObject[] SoundText;
    [SerializeField, Tooltip("���x�l")]
    private float PositionX_Min, PositionX_Max;
    [SerializeField, Tooltip("�i�K")]
    private int SoundStage = 10;
    [SerializeField, Header("SE"), Tooltip("�J�[�\���ړ���")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("�G���[��")]
    SE SE_Error;

    /// <summary>
    /// �T�E���h�f�[�^�B
    /// </summary>
    struct SoundData
    {
        public RectTransform rectTransform; // ���W�B
        public int soundStage;              // ���݂̉��ʁB
    }

    /// <summary>
    /// �T�E���h�X�e�[�g�B
    /// </summary>
    private enum SoundState
    {
        enBGM,
        enSE,
        enNum
    }

    private SaveDataManager m_saveDataManager;
    private Gamepad m_gamepad;
    private BGM m_bgm;
    private SoundData[] m_soundDatas = new SoundData[(int)SoundState.enNum];
    private OptionState m_comandState = OptionState.enBGMSound;
    private float m_moveLength = 0.0f;                          // ��x�̃J�[�\���̈ړ��ʁB


    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        m_soundDatas[(int)SoundState.enBGM].rectTransform = SoundCursor[(int)SoundState.enBGM].GetComponent<RectTransform>();
        m_soundDatas[(int)SoundState.enSE].rectTransform = SoundCursor[(int)SoundState.enSE].GetComponent<RectTransform>();

        // �ړ��ʂ��v�Z�B
        m_moveLength = (PositionX_Min - PositionX_Max) / SoundStage;
        Init();
    }

    /// <summary>
    /// �����������B
    /// </summary>
    private void Init()
    {
        ChangeCameraRotation(m_saveDataManager.SaveData.saveData.CameraStete);
        InitSoundVolume();
    }

    /// <summary>
    /// ���ʂ̏������B
    /// </summary>
    private void InitSoundVolume()
    {
        // �{�����[�����v�Z�B
        var bgm = m_saveDataManager.SaveData.saveData.BGMVolume * SoundStage;
        var se = m_saveDataManager.SaveData.saveData.SEVolume * SoundStage;
        // �ԍ��B
        int bgmNumber = (int)SoundState.enBGM;
        int seNumber = (int)SoundState.enSE;

        // �J�[�\���ʒu���X�V�B
        CursorMove(bgm, bgmNumber);
        CursorMove(se, seNumber);
        // ���݂̒i�K�B�i0�`SoundStage�܂Łj
        m_soundDatas[bgmNumber].soundStage = (int)bgm;
        m_soundDatas[seNumber].soundStage = (int)se;
        // �e�L�X�g���X�V�B
        SoundText[bgmNumber].GetComponent<TextMeshProUGUI>().text = m_soundDatas[bgmNumber].soundStage.ToString();
        SoundText[seNumber].GetComponent<TextMeshProUGUI>().text = m_soundDatas[seNumber].soundStage.ToString();
        // ���ʂ�K�p����B
        m_bgm.ResetVolume();
    }

    /// <summary>
    /// �J�[�\���̈ʒu�����肷��B
    /// </summary>
    /// <param name="soundStage">���ʂ̒i�K�B</param>
    private void CursorMove(float soundStage, int number)
    {
        // ���݂̉��ʂƍX�V��̉��ʂ��r�B
        var diff = (int)soundStage - m_soundDatas[number].soundStage;
        // �J�[�\���̈ړ��ʂ��v�Z�B
        MoveX(m_soundDatas[number].rectTransform, diff * -m_moveLength);
    }

    /// <summary>
    /// �p�����[�^��ݒ肷�鏈���B
    /// </summary>
    public void Set(OptionState comandState, Gamepad gamepad)
    {
        m_comandState = comandState;
        m_gamepad = gamepad;

        SetBGMParamator();
        SetSEParamator();
        SetCameraParamator();
    }

    /// <summary>
    /// BGM�̃p�����[�^��ݒ肷�鏈���B
    /// </summary>
    private void SetBGMParamator()
    {
        if (m_comandState != OptionState.enBGMParamator)
        {
            return;
        }
        SetVolume(SoundState.enBGM);
    }

    /// <summary>
    /// SE�̃p�����[�^��ݒ肷�鏈���B
    /// </summary>
    private void SetSEParamator()
    {
        if (m_comandState != OptionState.enSEParamator)
        {
            return;
        }
        SetVolume(SoundState.enSE);
    }

    /// <summary>
    /// ���ʂ�ݒ�B
    /// </summary>
    /// <param name="rectTransform">�ΏۃI�u�W�F�N�g�̍��W�B</param>
    private void SetVolume(SoundState soundState)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            VolumeUp((int)soundState);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            VolumeDown((int)soundState);
        }

        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�B
        if(m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            VolumeUp((int)soundState);
        }
        if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            VolumeDown((int)soundState);
        }
    }

    /// <summary>
    /// ���ʂ��グ�鏈���B
    /// </summary>
    private void VolumeUp(int number)
    {
        m_soundDatas[number].soundStage++;
        if (m_soundDatas[number].soundStage > SoundStage)
        {
            m_soundDatas[number].soundStage = SoundStage;
            SE_Error.PlaySE();
            return;
        }        

        MoveX(m_soundDatas[number].rectTransform, -m_moveLength);
        SoundText[number].GetComponent<TextMeshProUGUI>().text = m_soundDatas[number].soundStage.ToString();

        if (m_comandState == OptionState.enSEParamator)
        {
            m_saveDataManager.SaveData.saveData.SEVolume = (float)m_soundDatas[number].soundStage / SoundStage;
        }
        if (m_comandState == OptionState.enBGMParamator)
        {
            m_saveDataManager.SaveData.saveData.BGMVolume = (float)m_soundDatas[number].soundStage / SoundStage;
            m_bgm.ResetVolume();
        }
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// ���ʂ������鏈���B
    /// </summary>
    private void VolumeDown(int number)
    {
        m_soundDatas[number].soundStage--;
        if (m_soundDatas[number].soundStage < 0)
        {
            m_soundDatas[number].soundStage = 0;
            SE_Error.PlaySE();
            return;
        }

        MoveX(m_soundDatas[number].rectTransform, m_moveLength);
        SoundText[number].GetComponent<TextMeshProUGUI>().text = m_soundDatas[number].soundStage.ToString();

        if (m_comandState == OptionState.enSEParamator)
        {
            m_saveDataManager.SaveData.saveData.SEVolume = (float)m_soundDatas[number].soundStage / SoundStage;
        }
        if (m_comandState == OptionState.enBGMParamator)
        {
            m_saveDataManager.SaveData.saveData.BGMVolume = (float)m_soundDatas[number].soundStage / SoundStage;
            m_bgm.ResetVolume();
        }
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// �ړ������B
    /// </summary>
    /// <param name="MoveLength">�ړ��ʁB</param>
    /// <param name="rectTransform">�ΏۃI�u�W�F�N�g�̍��W�B</param>
    private void MoveX(RectTransform rectTransform, float MoveLength)
    {
        var length = rectTransform.anchoredPosition.x + MoveLength;
        var x = Mathf.Clamp(length, PositionX_Min, PositionX_Max);
        rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
    }


    /// <summary>
    /// �J�����̃p�����[�^��ݒ肷�鏈���B
    /// </summary>
    private void SetCameraParamator()
    {
        if (m_comandState != OptionState.enCameraParamator)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PushRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PushLeft();
        }

        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�B
        if(m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            PushRight();
        }
        if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            PushLeft();
        }
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushRight()
    {
        if (m_saveDataManager.SaveData.saveData.CameraStete == false)
        {
            SE_Error.PlaySE();
            return;
        }
        ChangeCameraRotation(false);
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushLeft()
    {
        if (m_saveDataManager.SaveData.saveData.CameraStete == true)
        {
            SE_Error.PlaySE();
            return;
        }
        ChangeCameraRotation(true);
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// �J������]��ύX����B
    /// </summary>
    /// <param name="flag">false�Ȃ�m�[�}����]�Bture�Ȃ烊�o�[�X��]�B</param>
    private void ChangeCameraRotation(bool flag)
    {
        var text = "�m�[�}��";
        int notActivenumber = 0;
        int activeNumber = 1;
        if(flag == true)
        {
            text = "���o�[�X";
            notActivenumber = 1;
            activeNumber = 0;
        }

        CameraCursor[notActivenumber].SetActive(false);
        CameraCursor[activeNumber].SetActive(true);
        CameraText.GetComponent<TextMeshProUGUI>().text = $"{text}";
        m_saveDataManager.SaveData.saveData.CameraStete = flag;
    }

    /// <summary>
    /// �f�[�^������������B
    /// </summary>
    public void ResetStatus()
    {
        m_saveDataManager.InitOption();
        Init();
        m_saveDataManager.Save();
    }
}
