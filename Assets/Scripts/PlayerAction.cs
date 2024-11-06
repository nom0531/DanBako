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
    private bool m_jumpFlag; // ジャンプフラグ
    private bool m_airFlag; // 空中フラグ
    private bool m_touchFlag;//触るフラグ

    // ジャンプ中の高さを管理するための変数
    private float m_jumpHeight;
    private float m_gravity = -9.81f; // 重力の値

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
    /// 移動処理。
    /// </summary>
    private void Move()
    {
        Vector3 playerMove = Vector3.zero;
        Vector3 stickL = Vector3.zero;

        // ゲームパッドの入力を取得
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

        // 移動させる
        transform.position += move;

        m_moveFlag = move.sqrMagnitude > 0.0f;

        if (m_moveFlag)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

    /// <summary>
    /// ジャンプ処理。
    /// </summary>
    private void HandleJump() // ジャンプ処理の新しいメソッド
    {
        // 地面にいる場合、ジャンプの処理を行う
        if (transform.position.y <= 0) // y 座標が 0 以下なら地面にいると判断
        {
            // ジャンプ入力を確認
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_jumpFlag = true;
                m_jumpHeight = JumpPower; // ジャンプ力を設定
                m_airFlag = true; // 空中フラグを立てる
            }
        }

        // 空中にいる場合、重力を適用
        if (m_airFlag)
        {
            // ジャンプの高さを減少させて移動
            m_jumpHeight += m_gravity * Time.deltaTime; // 重力の適用
            transform.position += new Vector3(0, m_jumpHeight * Time.deltaTime, 0);

            // 高さが地面に達したら、空中フラグとジャンプフラグをリセット
            if (transform.position.y <= 0)
            {
                m_airFlag = false;
                m_jumpFlag = false;
                transform.position = new Vector3(transform.position.x, 0, transform.position.z); // 地面に戻す
            }
        }
    }
}
