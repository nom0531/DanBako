using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Main : MonoBehaviour
{
    [SerializeField, Header("巡回地点 (座標)")]
    private Vector3[] m_goals;

    private int m_destNum = 0;
    private NavMeshAgent m_agent;
    private Animator m_enemyAnimator;
    private Rigidbody m_rigidbody;
    private bool m_isWaiting = false;
    private bool m_isNotMove = false;       // 動かない時はtrue。
    private bool m_isLanding = false;       // 落下したならture。

    private const float DRAG = 0.001f;      // 空気抵抗。

    private Transform m_player;
    [SerializeField]
    private float m_chaseRange = 10.0f;  // 追跡範囲
    [SerializeField]
    private float m_attackRange = 2.0f;  // 攻撃範囲
    [SerializeField]
    private float m_attackCooldown = 2.0f;  // 攻撃のクールダウン時間

    private float m_lastAttackTime = 0.0f;
    private bool m_isChasing = false;
    private bool m_isAttacking = false;

    public bool LandingFlag
    {
        get => m_isLanding;
        set => m_isLanding = value;
    }

    public bool NotMoveFlag
    {
        get => m_isNotMove;
        set => m_isNotMove = value;
    }

    private PlayerStatus m_playerStatus;
    private GameStatus m_gameStatus;

    void Start()
    {
        Initialize();

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
        if (m_gameStatus != null && m_gameStatus.TimeStopFlag || NotMoveFlag == true)
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
        StopNavMeshAgent(true);

        m_enemyAnimator.SetBool("Run", false);
        m_enemyAnimator.SetBool("Idle", true);

        yield return new WaitForSeconds(2.0f);

        m_enemyAnimator.SetBool("Idle", false);
        StopNavMeshAgent(false);
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
        StopNavMeshAgent(true);

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
            m_playerStatus.Damage();
        }
    }

    private void EndAttack()
    {
        m_isAttacking = false;
        StopNavMeshAgent(false);
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
        if (m_agent.isStopped && !m_gameStatus.TimeStopFlag)
        {
            m_agent.isStopped = false;
            m_enemyAnimator.speed = 1.0f; // アニメーションを再開
        }
    }

    /// <summary>
    /// 初期化処理。
    /// </summary>
    private void Initialize()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        m_player = player.transform;
        m_playerStatus = player.GetComponent<PlayerStatus>();
        m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
        m_agent = GetComponent<NavMeshAgent>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_enemyAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// ナビメッシュを停止する。
    /// </summary>
    /// <param name="flag">trueなら停止する。</param>
    private void StopNavMeshAgent(bool flag)
    {
        m_agent.isStopped = flag;
    }

    /// <summary>
    /// RigidBodyのパラメータを設定する。
    /// </summary>
    /// <param name="drag">空気抵抗。</param>
    /// <param name="isUseGravity">重力を使用するかどうか。trueなら使用する。</param>
    public void RigidBodyParam(float drag, bool isUseGravity)
    {
        m_rigidbody.drag = drag;                 // 落下速度を速くする。
        m_rigidbody.useGravity = isUseGravity;
        m_rigidbody.freezeRotation = true;
    }

    /// <summary>
    /// 落下処理。
    /// </summary>
    public void Landing()
    {
        // 既に実行されているなら実行しない。
        if(m_isLanding == true)
        {
            return;
        }
        m_isLanding = true;
        // ナビメッシュを無効化する。
        m_agent.enabled = false;
        RigidBodyParam(DRAG, true);
        Debug.Log("落下した");
    }
}
