using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class StageSelector : MonoBehaviour
{
    /// <summary>
    /// �X�e�[�W�̃X�e�[�g�B
    /// </summary>
    public enum StageState
    {
        enStop,
        enRight,
        enLeft = -1
    }

    public GameObject Option;   //�I�v�V������ʂ̃I�u�W�F�N�g
    public GameObject Stage;    //�X�e�[�W�Z���N�g��ʂ̃I�u�W�F�N�g
    [SerializeField]
    public TextMeshProUGUI stageNameText;   //�X�e�[�W����\������I�u�W�F�N�g

    //�X�e�[�W����ۑ�����z��
    private string[] stageNames = { "�X�e�[�W1", "�X�e�[�W2", "�X�e�[�W3", "�X�e�[�W4", "�X�e�[�W5" };

    [SerializeField]
    private GameObject[] StageObjects;                  // �X�e�[�W�I�u�W�F�N�g�̔z��
    [SerializeField, Header("�ړ���̍��W")]
    private Vector3[] MovePositions;                    // �ړ���̃|�W�V����
    [SerializeField, Header("�V�t�g���x")]
    private float ShiftMoveSpeed = 5.0f;                // �X�e�[�W�ړ��̑��x

    private const float SELECTED_SCALE = 40.0f;         // �I�����ꂽ�X�e�[�W�̊g�嗦
    private const float DEFAULT_SCALE = 20.0f;          // ��I���X�e�[�W�̃f�t�H���g�X�P�[��

    private StageState m_nextStage = StageState.enStop; // ���ɑI������X�e�[�W�̃X�e�[�g
    private int m_forwardStageScale = 0;                // �g�傷��X�e�[�W�̃C���f�b�N�X
    private int m_currentIndex = 0;                     // ���ݑI������Ă���X�e�[�W�̃C���f�b�N�X
    [SerializeField]
    private bool m_isMoving = false;                    // �X���C�h���Ă��邩�ǂ���
    [SerializeField]
    private bool allMoved = true;                       //�S�Ă̓����I��������ǂ���


    private void Start()
    {
        if (StageObjects.Length == 0)
        {
            Debug.LogError("�X�e�[�W���ݒ肳��Ă��܂���I");
            return;
        }

        // ���W��ݒ肷��B
        for (int i = 0; i < StageObjects.Length; i++)
        {
            StageObjects[i].transform.position = MovePositions[i];
        }

        // �ŏ��̑I���X�e�[�W������
        UpdateStageScale();
    }

    private void Update()
    {
        //�X�e�[�W�̖��O��\��
        UpdateStageName();

        //���E�̏\���L�[�ŃX�e�[�W��ύX
        //Start�{�^���ŃI�v�V������\��
        SelectStageAndOption();
        
        //�X�e�[�W�̈ʒu���X�V
        MoveStage();
        
        // �X�P�[�������炩�ɍX�V
        UpdateStageScale();
    }

    private void SelectStageAndOption()
    {
        if (m_isMoving == false)
        {
            // ���E�̖��L�[���͂��`�F�b�N
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                // �E�ɃV�t�g�B
                ShiftObjects(StageState.enRight);
            }
            else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                // ���ɃV�t�g�B
                ShiftObjects(StageState.enLeft);
            }
            else if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                //�I�v�V������ʂ��J��
                OpenOption();
            }
            else
            {
                //��L�ȊO�Ȃ�
                m_isMoving = true;
            }
        }
    }


    /// <summary>
    /// �I�𒆂̃X�e�[�W����\��
    /// </summary>
    private void UpdateStageName()
    {
        stageNameText.text = stageNames[m_currentIndex];
    }

    /// <summary>
    /// �V�t�g�����B
    /// </summary>
    /// <param name="stageState">���ɑI������X�e�[�W�̕����B</param>
    private void ShiftObjects(StageState stageState)
    {
        if (m_isMoving) return; // ���łɈړ����Ȃ珈�����I��

        m_isMoving = true; // �ړ����J�n�����炷���Ƀt���O��ݒ�

        m_nextStage = stageState; // MoveStage���̌㑱�̊֐��ׂ̈ɒl�����B

        // �V�����z����쐬
        GameObject[] shiftedObjects = new GameObject[StageObjects.Length];

        // �V�t�g����
        for (int i = 0; i < StageObjects.Length; i++)
        {
            ShiftStage(i,shiftedObjects);
        }

        // �I���W�i���̔z���V�����z��Œu������
        StageObjects = shiftedObjects;
    }

    /// <summary>
    /// �Ō�̗v�f��擪�ɁA����1�����炷�����B
    /// </summary>
    private void ShiftStage(int stageNumber,GameObject[] gameObjects)
    {

        if (m_nextStage == StageState.enRight)
        {
            gameObjects[stageNumber] = StageObjects[(stageNumber + 1 + StageObjects.Length) % StageObjects.Length];
        }
        else if (m_nextStage == StageState.enLeft)
        {
            gameObjects[stageNumber] = StageObjects[(stageNumber - 1 + StageObjects.Length) % StageObjects.Length];
        }
    }

    /// <summary>
    /// �X�e�[�W�𓮂��������B
    /// </summary>
    private void MoveStage()
    {
        if (m_isMoving == true)
        {
            for (int i = 0; i < StageObjects.Length; i++)
            {
                int nextStage = i;
                // �X�e�[�W�𓮂����B
                if (m_nextStage == StageState.enRight)
                {
                    nextStage = (i + (int)m_nextStage + StageObjects.Length) % StageObjects.Length;
                }
                else if (m_nextStage == StageState.enLeft)
                {
                    nextStage = (i + (int)m_nextStage + StageObjects.Length) % StageObjects.Length;
                }

                StageObjects[i].transform.position = Vector3.Lerp(StageObjects[i].transform.position, MovePositions[nextStage], Time.deltaTime * ShiftMoveSpeed);

                if (Vector3.Distance(StageObjects[i].transform.position, MovePositions[nextStage]) > 5.0f)
                {
                    allMoved = false;
                }
                else
                {
                    allMoved = true;
                }
            }

            if (allMoved)
            {
                UpdateIndex();
                m_isMoving = false;
                m_nextStage = StageState.enStop;
            }
        }
    }

    /// <summary>
    /// �X�e�[�W�̃X�P�[����������֐��B
    /// </summary>
    private void UpdateStageScale()
    {
        // �X�P�[�����f�t�H���g�̒l�ŏ������B
        Vector3 targetScale = Vector3.one * DEFAULT_SCALE;

        for (int i = 0; i < StageObjects.Length; i++)
        {
            if (i == m_forwardStageScale)
            {
                // �I�����Ă���I�u�W�F�N�g�̃X�P�[���B
                targetScale = Vector3.one * SELECTED_SCALE;
            }
            else
            {
                targetScale = Vector3.one * DEFAULT_SCALE;
            }
            StageObjects[i].transform.localScale =
                Vector3.Lerp(StageObjects[i].transform.localScale, targetScale, Time.deltaTime * ShiftMoveSpeed);
        }
    }

    /// <summary>
    /// �C���f�b�N�X���X�V�B
    /// </summary>
    private void UpdateIndex()
    {
        if (m_nextStage == StageState.enRight)
        {
            m_currentIndex = (m_currentIndex + 1 + StageObjects.Length) % StageObjects.Length;

        }
        else if (m_nextStage == StageState.enLeft)
        {
            m_currentIndex = (m_currentIndex - 1 + StageObjects.Length) % StageObjects.Length;
        }
        
    }
    
    /// <summary>
    /// �I�v�V������ʂ��J��
    /// �X�e�[�W�Z���N�g��ʂ����
    /// </summary>
    public void OpenOption()
    {
        Option.SetActive(true);
        Stage.SetActive(false);
    }
}
