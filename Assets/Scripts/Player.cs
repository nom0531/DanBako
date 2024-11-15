using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 0.01f;
    public float JumpPower = 6.0f;

    private Animator m_playerAnimator;

    public int m_HP = 5;

    private bool m_moveFlag, m_damageFlag;
    private bool m_jumpFlag; // �W�����v�t���O
    private bool m_airFlag; // �󒆃t���O
    private bool m_touchFlag;//�G��t���O

    // �W�����v���̍������Ǘ����邽�߂̕ϐ�
    private float m_jumpHeight;
    private float m_gravity = -9.81f; // �d�͂̒l

    void Start()
    {
        m_playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_damageFlag || IsPlayingDeathAnimation()) return;

        // �ړ��ƃW�����v�̏���
        Action();
        HandleJump(); // �W�����v�������Ăяo��

        // �A�j���[�V�����̍X�V
        Animation();
    }

    private void Action()
    {
        // �Q�[���p�b�h�̓��͂��擾
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical) * MoveSpeed;

        // �ړ�������
        transform.position += move;

        m_moveFlag = move.sqrMagnitude > 0.0f;

        if (m_moveFlag)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

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
                m_playerAnimator.SetBool("JumpFlag", true);
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
                m_playerAnimator.SetBool("JumpFlag", false);
            }
        }
    }

    private void Animation()
    {
        if (m_HP <= 0 && !IsPlayingDeathAnimation())
        {
            m_playerAnimator.SetTrigger("DieTri");
        }

        m_playerAnimator.SetBool("MoveFlag", m_moveFlag);
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_playerAnimator.SetTrigger("WinTri");
        }

        if (Input.GetKeyDown(KeyCode.J) && m_HP > 0)
        {
            m_HP -= 1;
            m_damageFlag = true;

            m_playerAnimator.SetTrigger("DamageTri");
            StartCoroutine(DamageRecovery());
        }
        if (Input.GetKey(KeyCode.Y))
        {
            m_touchFlag = true;
            m_playerAnimator.SetBool("TouchFlag", true);
            Debug.Log("adad");
        }
        else if (Input.GetKey(KeyCode.U))
        {
            m_touchFlag = false;
            m_playerAnimator.SetBool("TouchFlag", false);
        }
    }

    private IEnumerator DamageRecovery()
    {
        yield return new WaitForSeconds(1.0f);
        m_damageFlag = false;
    }

    private bool IsPlayingDeathAnimation()
    {
        AnimatorStateInfo stateInfo = m_playerAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Die");
    }
}
