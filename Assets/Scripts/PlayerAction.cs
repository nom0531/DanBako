using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̍s���𐧌䂷��N���X�B
/// </summary>
public class PlayerAction : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�^�X")]
    int m_hP = 3;
    [SerializeField]
    float m_moveSpeed = 0.1f;
    [SerializeField]
    float m_jumpPower = 0.6f;

    GameTime m_gameTime = null;
    GroundCheck m_groundCheck = null;
    Rigidbody m_rigidBody = null;

    // Start is called before the first frame update
    void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameTime>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_groundCheck = transform.GetChild(0).gameObject.GetComponent<GroundCheck>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    /// <summary>
    /// �s�������B
    /// </summary>
    void Move()
    {
        Vector3 move = Vector3.zero;

        // �O��ړ�
        if (Input.GetKey(KeyCode.W))
        {
            move.z += m_moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.z += -m_moveSpeed;
        }
        // ���E�ړ�
        if (Input.GetKey(KeyCode.D))
        {
            move.x += m_moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move.x += -m_moveSpeed;
        }

        // �ړ�������
        transform.position += move;
        Rotation(move);
    }

    /// <summary>
    /// ��]�����B
    /// </summary>
    void Rotation(Vector3 move)
    {
        // ��]
        if (move.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

    /// <summary>
    /// �W�����v�����B
    /// </summary>
    void Jump()
    {
        if (!Input.GetKeyDown(KeyCode.K))
        {
            return;
        }
        if (!m_groundCheck.GroundCheckFlag)
        {
            return;
        }

        m_groundCheck.GroundCheckFlag = false;
        m_rigidBody.AddForce(new Vector3(0.0f, m_jumpPower, 0.0f), ForceMode.VelocityChange);
    }
}
