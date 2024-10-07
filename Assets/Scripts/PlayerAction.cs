using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̍s���𐧌䂷��N���X�B
/// </summary>
public class PlayerAction : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�^�X")]
    int HP = 3;
    [SerializeField]
    float MoveSpeed = 0.1f;
    [SerializeField]
    float JumpPower = 0.6f;

    GameTime m_gameTime = null;
    GroundCheck m_groundCheck = null;
    Rigidbody m_rigidBody = null;
    GameObject m_gameCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameTime>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_groundCheck = transform.GetChild(0).gameObject.GetComponent<GroundCheck>();
        m_gameCamera = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    /// <summary>
    /// �s�������B
    /// </summary>
    void Move()
    {
        // �J�������l�������ړ�
        Vector3 PlayerMove = Vector3.zero;
        Vector3 stickL = Vector3.zero;

        stickL.z = Input.GetAxis("Vertical2");
        stickL.x = Input.GetAxis("Horizontal2");


        Vector3 forward = m_gameCamera.transform.forward;
        Vector3 right = m_gameCamera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;


        right *= stickL.x;
        forward *= stickL.z;


        // �ړ����x�ɏ�L�Ōv�Z�����x�N�g�������Z����
        PlayerMove += right + forward;


        // �v���C���[�̑��x��ݒ肷�邱�Ƃňړ�������
        PlayerMove = (PlayerMove * MoveSpeed * Time.deltaTime);
        PlayerMove.y = m_rigidBody.velocity.y;
        m_rigidBody.velocity = PlayerMove;
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
        m_rigidBody.AddForce(new Vector3(0.0f, JumpPower, 0.0f), ForceMode.VelocityChange);
    }
}
