using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float airControlFactor = 0.5f;

    [Header("重力設定")]
    public float gravity = -20f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    [Header("HP設定")]
    public int maxHealth = 100;

    private int currentHealth;
    private bool isGrounded = false;
    private bool isDamaged = false;
    private bool hasWon = false;

    private float fallSpeed = 0f;
    private bool isMoving = false;

    private CharacterController controller;
    private Animator playerAnimator;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (hasWon) return;

        GroundCheck();
        ApplyGravity();

        if (!isDamaged)
        {
            Move();
        }

        UpdateAnimations();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed;
        isMoving = move.sqrMagnitude > 0.01f;

        if (isMoving)
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


    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            out hit,
            groundCheckDistance,
            groundLayer
        );

        if (isGrounded && hit.collider != null)
        {
            fallSpeed = 0f; // 地面に接地したら落下速度をリセット
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position + Vector3.up * 0.1f,
            transform.position + Vector3.down * groundCheckDistance
        );
    }
}
