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
    GameObject m_gameCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameTime>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_groundCheck = transform.GetChild(0).gameObject.GetComponent<GroundCheck>();
        m_gameCamera = Camera.main.gameObject;
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
        // �J�������l�������ړ�
        Vector3 PlayerMove = Vector3.zero;
        Vector3 stickL = Vector3.zero;

        stickL.z = Input.GetAxis("Vertical");
        stickL.x = Input.GetAxis("Horizontal");


        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        forward.y = 0.0f;
        right.y = 0.0f;


        right *= stickL.x;
        forward *= stickL.z;

        // �ړ����x�ɏ�L�Ōv�Z�����x�N�g�������Z����
        PlayerMove += right + forward;


        // �v���C���[�̑��x��ݒ肷�邱�Ƃňړ�������
        PlayerMove = (PlayerMove * m_moveSpeed * Time.unscaledDeltaTime);
        transform.position += new Vector3(PlayerMove.x, 1.5f, PlayerMove.z);
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
