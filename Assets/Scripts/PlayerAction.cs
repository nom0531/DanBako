using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動を制御するクラス。
/// </summary>
public class PlayerAction : MonoBehaviour
{
    [SerializeField, Header("ステータス")]
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
    /// 行動処理。
    /// </summary>
    void Move()
    {
        // カメラを考慮した移動
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


        // 移動速度に上記で計算したベクトルを加算する
        PlayerMove += right + forward;


        // プレイヤーの速度を設定することで移動させる
        PlayerMove = (PlayerMove * MoveSpeed * Time.deltaTime);
        PlayerMove.y = m_rigidBody.velocity.y;
        m_rigidBody.velocity = PlayerMove;
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
        m_rigidBody.AddForce(new Vector3(0.0f, JumpPower, 0.0f), ForceMode.VelocityChange);
    }
}
