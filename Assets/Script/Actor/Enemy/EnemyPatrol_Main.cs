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

    private GameTime m_gameTime;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_gameTime = GameObject.FindGameObjectWithTag("TimeObject").GetComponent<GameTime>();
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
        // 時間停止中は全ての処理をスキップ
        if (m_gameTime != null && m_gameTime.TimeStopFlag)
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

        // 攻撃終了後に再び動けるようにする
        Invoke(nameof(EndAttack), 1.5f); // アニメーションの終了時間に合わせて調整
    }

    // アニメーションイベントから呼ばれるメソッド
    public void ApplyDamage()
    {
        if (m_player.TryGetComponent<Player_Main>(out Player_Main playerScript))
        {

            Debug.Log("アニメーションイベントでプレイヤーにダメージを適用");
            playerScript.TakeDamage(); // 例として1ダメージ
        }
    }

    private void EndAttack()
    {
        m_isAttacking = false;
        m_agent.isStopped = false;
    }

    // 時間停止時の処理
    private void HandleTimeStop()
    {
        if (!m_agent.isStopped)
        {
            m_agent.isStopped = true;
            m_enemyAnimator.speed = 0.0f; // アニメーションを停止
        }
    }

    // 時間停止から再開時の処理
    private void ResumeFromTimeStop()
    {
        if (m_agent.isStopped && !m_gameTime.TimeStopFlag)
        {
            m_agent.isStopped = false;
            m_enemyAnimator.speed = 1f; // アニメーションを再開
        }
    }

    private void InitializeAgentAndAnimator()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_enemyAnimator = GetComponent<Animator>();

        // 高速移動設定
        m_agent.speed = 10000.0f;         // 高速移動
        m_agent.acceleration = 100.0f; // 高速加速

    }

}
