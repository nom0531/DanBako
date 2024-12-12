using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField,Header("移動設定")]
    private float MoveSpeed = 5.0f;
    [SerializeField]
    private float AirControlFactor = 0.5f;
    [SerializeField, Header("重力設定")]
    private float Gravity = -20.0f;
    [SerializeField]
    private float GroundCheckDistance = 0.5f;
    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField, Header("HP設定")]
    private int MaxHealth = 3;

    private int m_currentHealth = 0;
    private bool m_isGrounded = false;
    private bool m_isDamaged = false;
    private bool m_hasWon = false;

    private bool m_isOnSlope = false;
    private bool m_isAgainstWall = false;

    private float m_fallSpeed = 0.0f;
    private bool m_isMoving = false;

    private GameManager m_gameManager;
    private CharacterController m_characterController;
    private Animator m_animator;
    private GameObject m_mainCamera;

    public int HP
    {
        get => m_currentHealth;
        set => m_currentHealth = value;
    }

    void Start()
    {
        m_gameManager = GameManager.Instance;
        m_currentHealth = MaxHealth;
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        // メインカメラのゲームオブジェクトを取得する
        m_mainCamera = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        if (m_gameManager.GameMode == CurrentGameMode.enClear)
        {
            return;
        }
        if (m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }

        if (!m_hasWon && !m_isDamaged)
        {
            GroundCheck();
            ApplyGravity();
            Move();
            UpdateAnimations();
        }
    }

    private void GroundCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * m_characterController.height / 2f;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, GroundCheckDistance, GroundLayer))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up); // 地形の角度を計算

            if (angle <= 15.0f)
            {
                // 地面判定
                m_isGrounded = true;
                m_isOnSlope = false;
                m_isAgainstWall = false;
            }
            else if (angle > 15.0f && angle <= 65.0f)
            {
                // 坂判定
                m_isGrounded = true;
                m_isOnSlope = true;
                m_isAgainstWall = false;
            }
            else if (angle > 90.0f)
            {
                // 壁判定
                m_isGrounded = false;
                m_isOnSlope = false;
                m_isAgainstWall = true;
            }
        }
        else
        {
            // 空中状態
            m_isGrounded = false;
            m_isOnSlope = false;
            m_isAgainstWall = false;
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

        Vector3 move = (cameraForward * moveZ + cameraRight * moveX) * MoveSpeed;

        if (m_isOnSlope)
        {
            // 坂にいる場合は、移動ベクトルを地形の法線に合わせる
            Vector3 slopeNormal = Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, GroundCheckDistance, GroundLayer) ? slopeHit.normal : Vector3.up;
            move = Vector3.ProjectOnPlane(move, slopeNormal);
        }

        m_isMoving = move.sqrMagnitude > 0.01f;

        if (m_isMoving && !m_isAgainstWall)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }

        if (!m_isGrounded)
        {
            move.x *= AirControlFactor;
            move.z *= AirControlFactor;
        }

        m_characterController.Move(move * Time.unscaledDeltaTime);
    }

    private void ApplyGravity()
    {
        if (m_isGrounded)
        {
            m_fallSpeed = 0.0f;
        }
        else
        {
            m_fallSpeed += (Physics.gravity.y + Gravity) * Time.deltaTime;
            m_characterController.Move(Vector3.up * m_fallSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimations()
    {
        m_animator.SetBool("MoveFlag", m_isMoving);
    }

    public void TakeDamage()
    {
        if (m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }
        if (HP < 0)
        {
            return;
        }
        if (m_isDamaged) return;

        m_isDamaged = true;
        --m_currentHealth;

        m_animator.SetTrigger("DamageTri");

        if (m_currentHealth <= 0)
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
        m_isDamaged = false;
    }

    private void Die()
    {
        m_animator.SetTrigger("DieTri");
        enabled = false;
    }

    public void Celebrate()
    {
        if (m_hasWon) return;

        m_hasWon = true;
        m_animator.SetTrigger("WinTri");
    }
}
