using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float airControlFactor = 0.5f;

    [Header("重力設定")]
    public float gravity = -20f;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    [Header("HP設定")]
    public int maxHealth = 100;

    private int currentHealth;
    private bool isGrounded = false;
    private bool isDamaged = false;
    private bool hasWon = false;

    private bool isOnSlope = false;
    private bool isAgainstWall = false;

    private float fallSpeed = 0f;
    private bool isMoving = false;

    private CharacterController controller;
    private Animator playerAnimator;
    GameObject m_mainCamera;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        // メインカメラのゲームオブジェクトを取得する
        m_mainCamera = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        if (!hasWon && !isDamaged)
        {
            GroundCheck();
            ApplyGravity();
            Move();
            UpdateAnimations();
        }
    }

    private void GroundCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * controller.height / 2f;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up); // 地形の角度を計算

            if (angle <= 15f)
            {
                // 地面判定
                isGrounded = true;
                isOnSlope = false;
                isAgainstWall = false;
                Debug.Log("地面に接地しています: " + hit.collider.name);
            }
            else if (angle > 15f && angle <= 45f)
            {
                // 坂判定
                isGrounded = true;
                isOnSlope = true;
                isAgainstWall = false;
                Debug.Log("坂にいます: " + hit.collider.name);
            }
            else if (angle > 45f)
            {
                // 壁判定
                isGrounded = false;
                isOnSlope = false;
                isAgainstWall = true;
                Debug.Log("壁に接しています: " + hit.collider.name);
            }
        }
        else
        {
            // 空中状態
            isGrounded = false;
            isOnSlope = false;
            isAgainstWall = false;
            Debug.Log("空中です");
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = m_mainCamera.transform.forward;
        Vector3 cameraRight = m_mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = (cameraForward * moveZ + cameraRight * moveX) * moveSpeed;

        if (isOnSlope)
        {
            // 坂にいる場合は、移動ベクトルを地形の法線に合わせる
            Vector3 slopeNormal = Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, groundCheckDistance, groundLayer) ? slopeHit.normal : Vector3.up;
            move = Vector3.ProjectOnPlane(move, slopeNormal);
        }

        isMoving = move.sqrMagnitude > 0.01f;

        if (isMoving && !isAgainstWall)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }

        if (!isGrounded)
        {
            move.x *= airControlFactor;
            move.z *= airControlFactor;
        }

        controller.Move(move * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (isGrounded)
        {
            fallSpeed = 0f;
        }
        else
        {
            fallSpeed += Physics.gravity.y * Time.deltaTime;
            controller.Move(Vector3.up * fallSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimations()
    {
        playerAnimator.SetBool("MoveFlag", isMoving);
    }

    public void TakeDamage(int damage)
    {
        if (isDamaged) return;

        isDamaged = true;
        currentHealth -= damage;
        Debug.Log("DamageTri トリガーをセットします"); // デバッグログ
        playerAnimator.SetTrigger("DamageTri");

        Debug.Log($"ダメージ: {damage}, 残りHP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetDamageState), 1f);
        }
    }

    private void ResetDamageState()
    {
        isDamaged = false;
    }

    private void Die()
    {
        Debug.Log("死亡しました");
        playerAnimator.SetTrigger("DieTri");
        enabled = false;
    }

    public void Celebrate()
    {
        if (hasWon) return;

        hasWon = true;
        playerAnimator.SetTrigger("WinTri");
        Debug.Log("勝利しました！");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * groundCheckDistance);
    }
}
