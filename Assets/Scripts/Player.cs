using UnityEngine;

public class Player : MonoBehaviour
{
    public float gravity = -1000f;         // 重力の強さ
    public float fallSpeed = 10f;         // 現在の落下速度
    public float groundCheckDistance = 0.1f; // 地面との判定距離
    public float moveSpeed = 5f;         // 移動速度
    public float slopeLimit = 45f;       // 登れる坂道の最大角度
    public float slopeSmooth = 0.1f;     // 坂道でのスムーズな移動
    public float airControlFactor = 0.5f; // 空中制御の効き具合

    public int maxHealth = 100;          // 最大HP
    private int currentHealth;           // 現在のHP

    private bool isGrounded = false;     // 地面にいるかどうか
    private Animator m_playerAnimator;   // アニメーター

    private bool m_moveFlag = false;     // 移動フラグ
    private bool isDamaged = false;      // ダメージを受けているかどうか

    private CharacterController controller;

    void Start()
    {
        currentHealth = maxHealth;       // 初期HPを設定
        m_playerAnimator = GetComponent<Animator>(); // Animatorを取得
        controller = GetComponent<CharacterController>(); // CharacterControllerを取得
    }

    void Update()
    {
        CheckGrounded();    // 地面判定
        if (!isDamaged)     // ダメージを受けていないときに移動
        {
            Move();         // 移動処理
        }
        ApplyGravity();     // 重力処理
        Animate();          // アニメーション制御
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        m_moveFlag = move.sqrMagnitude > 0.0f; // 移動フラグを更新

        if (m_moveFlag)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized); // 向きを変更
        }

        // 坂道補正を行い、移動ベクトルを更新
        AdjustForSlope(ref move);

        // 空中でも制御を効かせる
        if (!isGrounded)
        {
            move.x *= airControlFactor;
            move.z *= airControlFactor;
        }

        // 実際にプレイヤーを移動
        controller.Move(move);
    }

    private void AdjustForSlope(ref Vector3 move)
    {
        if (isGrounded)
        {
            RaycastHit hitCenter;
            Vector3 origin = transform.position;

            bool hitDetectedCenter = Physics.Raycast(
                origin + Vector3.up * 0.1f, Vector3.down,
                out hitCenter, groundCheckDistance + 0.1f, LayerMask.GetMask("BackGround"));

            if (hitDetectedCenter)
            {
                Vector3 normal = hitCenter.normal; // 地面の法線を取得
                float slopeAngle = Vector3.Angle(normal, Vector3.up);

                if (slopeAngle <= slopeLimit)
                {
                    // 坂道に沿った移動ベクトルを計算
                    Vector3 slopeDirection = Vector3.ProjectOnPlane(move, normal);
                    move = Vector3.Lerp(move, slopeDirection, slopeSmooth);

                    // プレイヤーの位置を地面にスムーズに接地させる
                    Vector3 targetPosition = new Vector3(
                        transform.position.x,
                        hitCenter.point.y,
                        transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

                    // プレイヤーの回転を坂道に合わせる
                    Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
                else
                {
                    // 急すぎる坂道では停止
                    move = Vector3.zero;
                    Debug.Log("急な坂道で停止中");
                }
            }
            else
            {
                Debug.Log("地面が検知されません！");
            }
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            fallSpeed += gravity * Time.deltaTime; // 重力の適用
            controller.Move(Vector3.up * fallSpeed * Time.deltaTime);
        }
        else
        {
            fallSpeed = 0f; // 地面についている場合、落下速度をリセット
        }
    }


    private void CheckGrounded()
    {
        isGrounded = controller.isGrounded;
    }

    private void Animate()
    {
        m_playerAnimator.SetBool("MoveFlag", m_moveFlag);
    }

    public void TakeDamage(int damage)
    {
        if (isDamaged) return; // ダメージ中は処理しない

        isDamaged = true; // ダメージ中フラグを立てる
        m_playerAnimator.SetTrigger("DamageTri");
        currentHealth -= damage; // HPを減少

        Debug.Log($"プレイヤーに {damage} ダメージ。残りHP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // ダメージ後に一定時間移動できないようにする
            Invoke(nameof(ResetDamageState), 1f); // 1秒後にダメージ状態を解除
        }
    }

    private void ResetDamageState()
    {
        isDamaged = false; // ダメージ状態解除
    }

    private void Die()
    {
        Debug.Log("プレイヤーが死亡しました！");
        m_playerAnimator.SetTrigger("DieTri"); // 死亡アニメーション
        enabled = false; // スクリプトを無効化
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
