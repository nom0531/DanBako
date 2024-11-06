using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Oparation : MonoBehaviour
{
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
        var oldComandState = m_comandState;
        m_comandState++;
        // �␳�B
        if (m_comandState > OparationState.enBehind)
        {
            SE_Error.PlaySE();
            m_comandState = OparationState.enBehind;
            return;
        }
        m_currentLocationList[(int)oldComandState].PlayAnimaton("NotActive");
        m_currentLocationList[(int)m_comandState].PlayAnimaton("Active");
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ���L�[���������Ƃ��̏����B
    /// </summary>
    private void PushLeft()
    {
        var oldComandState = m_comandState;
        m_comandState--;
        // �␳�B
        if (m_comandState < OparationState.enFront)
        {
            SE_Error.PlaySE();
            m_comandState = OparationState.enFront;
            return;
        }
        m_currentLocationList[(int)oldComandState].PlayAnimaton("NotActive");
        m_currentLocationList[(int)m_comandState].PlayAnimaton("Active");
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// �A�j���[�V�����̍Đ������B
    /// </summary>
    /// <param name="triggerName">�g���K�[�̖��O�B</param>
    /// <param name="oparationState">�X�e�[�g�B</param>
    private void PlayAnimation(string triggerName, OparationState oparationState)
    {
        m_animator[(int)oparationState].SetTrigger(triggerName);
    }
}
