using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float MoveSpeed = 0.01f;
    public float JumpPower = 6.0f;

    public int m_HP = 5;

    private GameManager m_gameManager;
    private GameObject m_gameCamera;

    private bool m_moveFlag, m_damageFlag;
    private bool m_jumpFlag; // �W�����v�t���O
    private bool m_airFlag; // �󒆃t���O
    private bool m_touchFlag;//�G��t���O

    // �W�����v���̍������Ǘ����邽�߂̕ϐ�
    private float m_jumpHeight;
    private float m_gravity = -9.81f; // �d�͂̒l

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_gameCamera = Camera.main.gameObject;
    }

    void Update()
    {
        if(m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }

        if (m_damageFlag) return;

        Move();
        HandleJump();
    }

    /// <summary>
    /// �ړ������B
    /// </summary>
    private void Move()
    {
        Vector3 playerMove = Vector3.zero;
        Vector3 stickL = Vector3.zero;

        // �Q�[���p�b�h�̓��͂��擾
        stickL.x = Input.GetAxis("Horizontal");
        stickL.z = Input.GetAxis("Vertical");

        Vector3 forward = m_gameCamera.transform.forward;
        Vector3 right = m_gameCamera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;

        right *= stickL.x;
        forward *= stickL.z;

        playerMove += right + forward;

        Vector3 move = playerMove * MoveSpeed;

        // �ړ�������
        transform.position += move;

        m_moveFlag = move.sqrMagnitude > 0.0f;

        if (m_moveFlag)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

    /// <summary>
    /// �W�����v�����B
    /// </summary>
    private void HandleJump() // �W�����v�����̐V�������\�b�h
    {
        // �n�ʂɂ���ꍇ�A�W�����v�̏������s��
        if (transform.position.y <= 0) // y ���W�� 0 �ȉ��Ȃ�n�ʂɂ���Ɣ��f
        {
            // �W�����v���͂��m�F
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_jumpFlag = true;
                m_jumpHeight = JumpPower; // �W�����v�͂�ݒ�
                m_airFlag = true; // �󒆃t���O�𗧂Ă�
            }
        }

        // �󒆂ɂ���ꍇ�A�d�͂�K�p
        if (m_airFlag)
        {
            // �W�����v�̍��������������Ĉړ�
            m_jumpHeight += m_gravity * Time.deltaTime; // �d�͂̓K�p
            transform.position += new Vector3(0, m_jumpHeight * Time.deltaTime, 0);

            // �������n�ʂɒB������A�󒆃t���O�ƃW�����v�t���O�����Z�b�g
            if (transform.position.y <= 0)
            {
                m_airFlag = false;
                m_jumpFlag = false;
                transform.position = new Vector3(transform.position.x, 0, transform.position.z); // �n�ʂɖ߂�
            }
        }
    }
}
