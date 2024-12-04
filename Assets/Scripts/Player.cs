using UnityEngine;

public class Player : MonoBehaviour
{
    public float gravity = -1000f;         // �d�͂̋���
    public float fallSpeed = 10f;         // ���݂̗������x
    public float groundCheckDistance = 0.1f; // �n�ʂƂ̔��苗��
    public float moveSpeed = 5f;         // �ړ����x
    public float slopeLimit = 45f;       // �o���⓹�̍ő�p�x
    public float slopeSmooth = 0.1f;     // �⓹�ł̃X���[�Y�Ȉړ�
    public float airControlFactor = 0.5f; // �󒆐���̌����

    public int maxHealth = 100;          // �ő�HP
    private int currentHealth;           // ���݂�HP

    private bool isGrounded = false;     // �n�ʂɂ��邩�ǂ���
    private Animator m_playerAnimator;   // �A�j���[�^�[

    private bool m_moveFlag = false;     // �ړ��t���O
    private bool isDamaged = false;      // �_���[�W���󂯂Ă��邩�ǂ���

    private CharacterController controller;

    void Start()
    {
        currentHealth = maxHealth;       // ����HP��ݒ�
        m_playerAnimator = GetComponent<Animator>(); // Animator���擾
        controller = GetComponent<CharacterController>(); // CharacterController���擾
    }

    void Update()
    {
        CheckGrounded();    // �n�ʔ���
        if (!isDamaged)     // �_���[�W���󂯂Ă��Ȃ��Ƃ��Ɉړ�
        {
            Move();         // �ړ�����
        }
        ApplyGravity();     // �d�͏���
        Animate();          // �A�j���[�V��������
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        m_moveFlag = move.sqrMagnitude > 0.0f; // �ړ��t���O���X�V

        if (m_moveFlag)
        {
            transform.rotation = Quaternion.LookRotation(move.normalized); // ������ύX
        }

        // �⓹�␳���s���A�ړ��x�N�g�����X�V
        AdjustForSlope(ref move);

        // �󒆂ł��������������
        if (!isGrounded)
        {
            move.x *= airControlFactor;
            move.z *= airControlFactor;
        }

        // ���ۂɃv���C���[���ړ�
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
                Vector3 normal = hitCenter.normal; // �n�ʂ̖@�����擾
                float slopeAngle = Vector3.Angle(normal, Vector3.up);

                if (slopeAngle <= slopeLimit)
                {
                    // �⓹�ɉ������ړ��x�N�g�����v�Z
                    Vector3 slopeDirection = Vector3.ProjectOnPlane(move, normal);
                    move = Vector3.Lerp(move, slopeDirection, slopeSmooth);

                    // �v���C���[�̈ʒu��n�ʂɃX���[�Y�ɐڒn������
                    Vector3 targetPosition = new Vector3(
                        transform.position.x,
                        hitCenter.point.y,
                        transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

                    // �v���C���[�̉�]���⓹�ɍ��킹��
                    Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
                else
                {
                    // �}������⓹�ł͒�~
                    move = Vector3.zero;
                    Debug.Log("�}�ȍ⓹�Œ�~��");
                }
            }
            else
            {
                Debug.Log("�n�ʂ����m����܂���I");
            }
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            fallSpeed += gravity * Time.deltaTime; // �d�͂̓K�p
            controller.Move(Vector3.up * fallSpeed * Time.deltaTime);
        }
        else
        {
            fallSpeed = 0f; // �n�ʂɂ��Ă���ꍇ�A�������x�����Z�b�g
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
        if (isDamaged) return; // �_���[�W���͏������Ȃ�

        isDamaged = true; // �_���[�W���t���O�𗧂Ă�
        m_playerAnimator.SetTrigger("DamageTri");
        currentHealth -= damage; // HP������

        Debug.Log($"�v���C���[�� {damage} �_���[�W�B�c��HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // �_���[�W��Ɉ�莞�Ԉړ��ł��Ȃ��悤�ɂ���
            Invoke(nameof(ResetDamageState), 1f); // 1�b��Ƀ_���[�W��Ԃ�����
        }
    }

    private void ResetDamageState()
    {
        isDamaged = false; // �_���[�W��ԉ���
    }

    private void Die()
    {
        Debug.Log("�v���C���[�����S���܂����I");
        m_playerAnimator.SetTrigger("DieTri"); // ���S�A�j���[�V����
        enabled = false; // �X�N���v�g�𖳌���
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
