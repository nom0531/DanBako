using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Main : MonoBehaviour
{
    [SerializeField, Header("巡回地点 (座標)")]
    private Vector3[] m_goals;

    private int m_destNum = 0;
    private NavMeshAgent m_agent;
    private Animator m_enemyAnimator;
    private bool m_isWaiting = false;

    private Transform m_player;
    [SerializeField]
    private float m_chaseRange = 10.0f;  // 追跡範囲
    [SerializeField]
    private float m_attackRange = 2.0f;  // 攻撃範囲
    [SerializeField]
    private float m_attackCooldown = 2.0f;  // 攻撃のクールダウン時間

    private bool m_isChasing = false;
    private bool m_isAttacking = false;
    private float m_lastAttackTime = 0.0f;

    private PlayerStatus m_playerStatus;
    private GameStatus m_gameStatus;

    [SerializeField, Header("敵のHP")]
    private int m_health = 100; // HPの初期値

    private bool m_isDead = false; // 死亡フラグ

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        m_player = player.transform;
        m_playerStatus = player.GetComponent<PlayerStatus>();
        m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
        InitializeAgentAndAnimator();
        if (m_goals.Length > 0)
        {
            SetGoalPosition();
        }
        else
        {
            Debug.LogError("巡回地点が設定されていません！");
        }
    }

    void Update()
    {
        if (m_isDead) return; // 死亡後は何もしない

        // 時間停止中は全ての処理をスキップ
        if (m_gameStatus != null && m_gameStatus.TimeStopFlag)
        {
            HandleTimeStop();
            return;
        }

        ResumeFromTimeStop();

        float distanceToPlayer = Vector3.Distance(transform.position, m_player.position);

        if (distanceToPlayer <= m_chaseRange && !m_isChasing)
        {
            m_isChasing = true;
        }
        else if (distanceToPlayer > m_chaseRange && m_isChasing)
        {
            m_isChasing = false;
        }

        if (m_isChasing)
        {
            if (distanceToPlayer <= m_attackRange)
            {
                AttackPlayer();
            }
            else if (!m_isAttacking)
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }

        UpdateAnimation();
    }

    public void Damage(int damageAmount)
    {
        if (m_isDead) return;

        m_health -= damageAmount;

        if (m_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        m_isDead = true;

        // ナビメッシュエージェントとアニメーションを停止
        m_agent.isStopped = true;
        m_enemyAnimator.SetTrigger("Die");

        // 一定時間後に削除
        Invoke(nameof(DestroyEnemy), 3.0f); // アニメーションが終わる時間に合わせて調整
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void Patrol()
    {
        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f && !m_isWaiting)
        {
            StartCoroutine(WaitAtGoal());
        }
    }

    private void SetGoalPosition()
    {
        m_agent.destination = m_goals[m_destNum];
    }

    private void NextGoal()
    {
        m_destNum = (m_destNum + 1) % m_goals.Length;
        SetGoalPosition();
    }

    private System.Collections.IEnumerator WaitAtGoal()
    {
        m_isWaiting = true;
        m_agent.isStopped = true;

        m_enemyAnimator.SetBool("Run", false);
        m_enemyAnimator.SetBool("Idle", true);

        yield return new WaitForSeconds(2.0f);

        m_enemyAnimator.SetBool("Idle", false);
        m_agent.isStopped = false;
        NextGoal();
        m_isWaiting = false;
    }

    private void UpdateAnimation()
    {
        if (m_agent.velocity.magnitude > 0.1f && !m_agent.isStopped)
        {
            m_enemyAnimator.SetBool("Run", true);
        }
        else
        {
            m_enemyAnimator.SetBool("Run", false);
        }
    }

    private void ChasePlayer()
    {
        m_agent.isStopped = false;
        m_agent.destination = m_player.position;
    }

    private void AttackPlayer()
    {
        transform.LookAt(m_player.position);

        if (m_isAttacking || Time.time < m_lastAttackTime + m_attackCooldown) return;

        m_isAttacking = true;
        m_agent.isStopped = true;

        m_enemyAnimator.SetTrigger("Attack");

        m_lastAttackTime = Time.time;

        Invoke(nameof(EndAttack), 1.5f);
    }

    public void ApplyDamage()
    {
        if (m_player.TryGetComponent<Player_Main>(out Player_Main playerScript))
        {
            m_playerStatus.Damage();
        }
    }

    private void EndAttack()
    {
        m_isAttacking = false;
        m_agent.isStopped = false;
    }

    private void HandleTimeStop()
    {
        if (!m_agent.isStopped)
        {
            m_agent.isStopped = true;
            m_enemyAnimator.speed = 0.0f;
        }
    }

    private void ResumeFromTimeStop()
    {
        if (m_agent.isStopped && !m_gameStatus.TimeStopFlag)
        {
            m_agent.isStopped = false;
            m_enemyAnimator.speed = 1f;
        }
    }

    private void InitializeAgentAndAnimator()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_enemyAnimator = GetComponent<Animator>();
    }
}
