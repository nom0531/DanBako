using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動を制御するクラス。
/// </summary>
public class PlayerAction : MonoBehaviour
{
    [SerializeField, Header("ステータス")]
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
    /// 行動処理。
    /// </summary>
    void Move()
    {
        Vector3 move = Vector3.zero;

        // 前後移動
        if (Input.GetKey(KeyCode.W))
        {
            move.z += m_moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.z += -m_moveSpeed;
        }
        // 左右移動
        if (Input.GetKey(KeyCode.D))
        {
            move.x += m_moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move.x += -m_moveSpeed;
        }

        // 移動させる
        transform.position += move;
        Rotation(move);
    }

    /// <summary>
    /// 回転処理。
    /// </summary>
    void Rotation(Vector3 move)
    {
        // 回転
        if (move.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

    /// <summary>
    /// ジャンプ処理。
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
