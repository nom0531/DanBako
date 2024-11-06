using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private SE moveCursorSE;
    [SerializeField]
    private BGM bgm;
    [SerializeField]
    private GameObject Option;       //�I�v�V������ʂ̃I�u�W�F�N�g
    [SerializeField]
    private GameObject Stage;        //�X�e�[�W�Z���N�g��ʂ̃I�u�W�F�N�g

    [SerializeField]
    private Slider bgmSlider;        //bgm�X���C�_�[
    [SerializeField]
    private Slider seSlider;         //se�X���C�_�[
    [SerializeField]
    private TextMeshProUGUI cameraOption;      //�J�����ݒ�
    [SerializeField]
    private Image backText;          //�߂�{�^��
    [SerializeField]
    private Image selectCursor;      //�J�[�\��

    [SerializeField]
    private TextMeshProUGUI bgmVolumeText;  //BGM�̃{�����[����\������e�L�X�g
    [SerializeField]
    private TextMeshProUGUI seVolumeText;   //SE�̃{�����[����\������e�L�X�g

    private int m_selectedIndex = 0;  //�I�𒆂̃C���f�b�N�X
    private GameObject[] menuItems;

    private int m_cameraIndex = 0;    //�J�����ݒ�̃C���f�b�N�X
    private string[] cameraOptionName = { "�m�[�}��", "�����̂�  " };

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManager;

    [SerializeField,Header("�X���C�_�[�I�����̃|�W�V��������")]
    private Vector3 SliderAdjustmentPosition;   //�X���C�_�[�I�����̃J�[�\���̃|�W�V�����̒����Ɏg��

    [SerializeField, Header("�J�����ݒ�I�����̃|�W�V��������")]
    private Vector3 CameraAdjustmentPosition;   //�J�����I�����̃J�[�\���̃|�W�V�����̒����Ɏg��

    [SerializeField,Header("�߂�{�^���I�����̃|�W�V��������")]
    private Vector3 ImageAdjustmentPosition;    //�߂�{�^���I�����̃J�[�\���̃|�W�V�����̒����Ɏg��

    void Start() 
    {
        // ���j���[���ڂ̃��X�g���쐬
        menuItems = new GameObject[] { bgmSlider.gameObject, seSlider.gameObject, cameraOption.gameObject ,backText.gameObject };

        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_soundManager = GameManager.Instance.SoundManager;

        //bgm��se�̃X���C�_�[�̒l��ύX
        bgmSlider.value = m_saveDataManager.SaveData.saveData.BGMVolume;
        seSlider.value = m_saveDataManager.SaveData.saveData.SEVolume;

        //selectedIndex�̏�����
        m_selectedIndex = 0;
    }

    void Update()
    {
        //�\���L�[�㉺�ňړ�
        MoveSelect();
        //�J�[�\���̈ړ�
        MoveCursor();
        //�\���L�[���E�ŃX���C�_�[�̒l��ύX
        ChangeSliderValue();
        //BGM��SE�̃{�����[���̕\����ύX
        ChangeVolumeText();
        //�\���L�[���E�ŃJ���������ύX
        ChangeCameraOption();
        //�߂�{�^���������ƃX�e�[�W�Z���N�g��ʂɑJ��
        BackToStageSelect();
       
    }

    /// <summary>
    /// �\���L�[�㉺�ňړ�
    /// </summary>
    void MoveSelect()
    {
        // �\���L�[�ŏ㉺�̓��͂��擾
        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            // �C���f�b�N�X��ύX�i�㉺�ړ��j
            int direction = -1;
            m_selectedIndex = Mathf.Clamp(m_selectedIndex + direction, 0, menuItems.Length - 1);
            //se���Đ�
            moveCursorSE.PlaySE();
        }
        else if(Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            int direction = 1;
            m_selectedIndex = Mathf.Clamp(m_selectedIndex + direction, 0, menuItems.Length - 1);
            //se���Đ�
            moveCursorSE.PlaySE();
        }
    }

    /// <summary>
    /// �J�[�\���̈ړ�
    /// </summary>
    void MoveCursor()
    {
        for(int i = 0;i < menuItems.Length;i++)
        {
            //�X���C�_�[���I������Ă���Ƃ�
            if(i == m_selectedIndex && menuItems[m_selectedIndex].TryGetComponent(out Slider selectedSlider))
            {
                selectCursor.transform.position = menuItems[i].transform.position + SliderAdjustmentPosition;
            }
            //�J�����ݒ肪�I������Ă���Ƃ�
            if (i == m_selectedIndex && menuItems[m_selectedIndex].TryGetComponent(out TextMeshProUGUI selectedText))
            {
                selectCursor.transform.position = menuItems[i].transform.position + CameraAdjustmentPosition;
            }
            //�߂�{�^�����I������Ă���Ƃ�
            if (i == m_selectedIndex && menuItems[m_selectedIndex].TryGetComponent(out Image selectedImage))
            {
                selectCursor.transform.position = menuItems[i].transform.position + ImageAdjustmentPosition;
            }
        }
    }

    /// <summary>
    /// �\���L�[���E�ŃX���C�_�[�̒l��ύX
    /// </summary>
    void ChangeSliderValue()
    {
        // �I�����ڂ��X���C�_�[�Ȃ獶�E���͂Œl�𒲐�
        if (menuItems[m_selectedIndex].TryGetComponent(out Slider selectedSlider))
        {
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                selectedSlider.value += 0.1f;
                //���ʂ�ύX
                SetBGMVolume();
                //se���Đ�
                moveCursorSE.PlaySE();
            }
            else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                selectedSlider.value -= 0.1f;
                //���ʂ�ύX
                SetBGMVolume();
                //se���Đ�
                moveCursorSE.PlaySE();
            }
        }
    }

    /// <summary>
    /// ���ʂ̕ύX
    /// </summary>
    void SetBGMVolume()
    {
        //�Z�[�u�f�[�^��bgm�̃{�����[������
        m_saveDataManager.SaveData.saveData.BGMVolume = bgmSlider.value;
        m_saveDataManager.Save();
        bgm.ResetVolume();
    }

    /// <summary>
    /// BGM��SE�̃{�����[���̕\����ύX
    /// </summary>
    void ChangeVolumeText()
    {
        //bgm��se�̒l���e�L�X�g�ɑ��
        bgmVolumeText.text = bgmSlider.value.ToString();
        seVolumeText.text = seSlider.value.ToString();
    }

    /// <summary>
    /// �J��������̕ύX
    /// </summary>
    void ChangeCameraOption()
    {
        //�I�����ڂ��J�����ݒ肾�����獶�E���͂Őݒ�ύX
        if(menuItems[m_selectedIndex] == cameraOption.gameObject)
        {
            if(Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                m_cameraIndex = (m_cameraIndex + 1 + cameraOptionName.Length) % cameraOptionName.Length;
            }
            else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                m_cameraIndex = (m_cameraIndex - 1 + cameraOptionName.Length) % cameraOptionName.Length;
            }
        }
        cameraOption.text = cameraOptionName[m_cameraIndex];
    }

    /// <summary>
    /// �߂�{�^���������ƃX�e�[�W�Z���N�g��ʂɑJ��
    /// </summary>
    void BackToStageSelect()
    {
        // �u�߂�v�{�^�����I������Ă���B�{�^����������Start�{�^���������Ɩ߂鏈�������s
        if (menuItems[m_selectedIndex].TryGetComponent(out Image selectedImage) && Gamepad.current.bButton.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame)
        {
            Stage.SetActive(true);
            Option.SetActive(false);
        }
    }
}
