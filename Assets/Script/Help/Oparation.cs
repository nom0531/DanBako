using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Oparation : MonoBehaviour
{
    [SerializeField, Header("�\������I�u�W�F�N�g")]
    private GameObject[] Panels;
    [SerializeField, Header("�y�[�W")]
    private GameObject Content;
    [SerializeField, Tooltip("���݈ʒu�������I�u�W�F�N�g")]
    private GameObject CurrentLocationObject;
    [SerializeField, Header("SE"), Tooltip("�J�[�\���ړ���")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("�G���[��")]
    private SE SE_Error;

    /// <summary>
    /// �I�𒆂̃R�}���h�B
    /// </summary>
    private enum OparationState
    {
        enFront,
        enBehind,
        enNum
    }

    private const float SCALE = 0.2f;

    private Gamepad m_gamepad;
    private Animator[] m_animator;
    private List<CurrentLocation> m_currentLocationList;
    private OparationState m_comandState = OparationState.enFront;

    // Start is called before the first frame update
    void Start()
    {
        //for(int i = 0; i< Panels.Length; i++)
        //{
        //    m_animator[i] = Panels[i].GetComponent<Animator>();
        //}

        CreateCurrentLocationObject();
    }

    /// <summary>
    /// �y�[�W�����I�u�W�F�N�g�𐶐�����B
    /// </summary>
    private void CreateCurrentLocationObject()
    {
        for (int i = 0; i < (int)OparationState.enNum; i++)
        {
            var gameObject = Instantiate(CurrentLocationObject);
            gameObject.transform.SetParent(Content.transform);
            gameObject.transform.localScale = new Vector3(SCALE, SCALE, 0.0f);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            // ���g�̔ԍ���������B
            var currentLocation = gameObject.GetComponent<CurrentLocation>();
            currentLocation.MyID = i;
            if(i != 0)
            {
                currentLocation.PlayAnimaton("NotActive");
            }
        }
        // HP�̃I�u�W�F�N�g�����X�g���B
        var currentLocations = FindObjectsOfType<CurrentLocation>();
        m_currentLocationList = new List<CurrentLocation>(currentLocations);
        // �\�[�g�B
        m_currentLocationList.Sort((a, b) => a.MyID.CompareTo(b.MyID));
    }

    // Update is called once per frame
    void Update()
    {
        CursorMove();
    }

    /// <summary>
    /// �J�[�\���ړ��B
    /// </summary>
    private void CursorMove()
    {
        m_gamepad = Gamepad.current;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PushRinght();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PushLeft();
        }

        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�B
        if (m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            PushRinght();
        }
        if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            PushLeft();
        }
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushRinght()
    {
        int oldComandState = (int)m_comandState;
        m_comandState++;
        // �␳�B
        if (m_comandState >= OparationState.enNum)
        {
            SE_Error.PlaySE();
            m_comandState = OparationState.enNum - 1;
            return;
        }
        Change(oldComandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushLeft()
    {
        int oldComandState = (int)m_comandState;
        m_comandState--;
        // �␳�B
        if (m_comandState < OparationState.enFront)
        {
            SE_Error.PlaySE();
            m_comandState = OparationState.enFront;
            return;
        }
        Change(oldComandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// �\������f�[�^��ύX�B
    /// </summary>
    /// <param name="numger">�\������߂�I�u�W�F�N�g�̔ԍ��B</param>
    private void Change(int numger)
    {
        // �\������Panel��ύX�B
        Panels[numger].gameObject.SetActive(false);
        Panels[(int)m_comandState].gameObject.SetActive(true);
        // ���݂̃y�[�W����ύX�B
        m_currentLocationList[numger].PlayAnimaton("NotActive");
        m_currentLocationList[(int)m_comandState].PlayAnimaton("Active");
    }

    /// <summary>
    /// �A�j���[�V�����̍Đ������B
    /// </summary>
    /// <param name="oparationState">�X�e�[�g�B</param>
    /// <param name="triggerName">�g���K�[�̖��O�B</param>
    private void PlayAnimation(OparationState oparationState, string triggerName)
    {
        m_animator[(int)oparationState].SetTrigger(triggerName);
    }
}
