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
    private bool m_isDead = false;          // 死亡フラグ

    private const int MASS = 10;            // 質量。
    private const float DRAG = 0.01f;       // 空気抵抗。

    private Transform m_player;
    [SerializeField]
    private float m_chaseRange = 10.0f;     // 追跡範囲
    [SerializeField]
    private float m_attackRange = 2.0f;     // 攻撃範囲
    [SerializeField]
    private float m_attackCooldown = 2.0f;  // 攻撃のクールダウン時間

    private float m_defaultSpeed = 0.0f;
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
    private Player_Main m_playerMain;
    private GameStatus m_gameStatus;

    /// <summary>
    /// 徐々に小さくして削除する為のコルーチン。
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator ShrinkAndDestroy()
    {
        float duration = 3.0f; // 縮小にかける時間
        float elapsed = 0.0f;

        Vector3 initialScale = transform.localScale; // 元のスケール
        Vector3 targetScale = Vector3.zero;         // 目標スケール

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // 時間の進行割合 (0から1)
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t); // スケールを線形補間
            yield return null; // 次フレームまで待機
        }

        // 完全に縮小した後に削除
        transform.localScale = targetScale;
        gameObject.SetActive(false);
    }

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

    /// <summary>
    /// 初期化処理。
    /// </summary>
    private void Initialize()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        m_player = player.transform;
        m_playerStatus = player.GetComponent<PlayerStatus>();
        m_playerMain = player.GetComponent<Player_Main>();
        m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
        m_agent = GetComponent<NavMeshAgent>();
        m_defaultSpeed = m_agent.speed;
        m_rigidbody = GetComponent<Rigidbody>();
        m_enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_isDead) return; // 死亡後は何もしない

        // 時間停止中は全ての処理をスキップ
        if (m_gameStatus != null && m_gameStatus.TimeStopFlag || NotMoveFlag == true)
        {
            HandleTimeStop();
            return;
        }
        if(m_playerMain.MoveFlag == true)
        {
            StopNavMeshAgent(true);
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
            m_agent.speed = 0.0f;
            m_enemyAnimator.speed = 0.0f; // アニメーションを停止
            m_agent.isStopped = true;
        }
    }

    // 時間停止から再開時の処理
    private void ResumeFromTimeStop()
    {
        if (m_agent.isStopped && !m_gameStatus.TimeStopFlag)
        {
            m_agent.isStopped = false;
            m_agent.speed = m_defaultSpeed;
            m_enemyAnimator.speed = 1.0f; // アニメーションを再開
        }
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
    /// <param name="mass">質量。</param>
    /// <param name="drag">空気抵抗。</param>
    public void RigidBodyParam(int mass, float drag)
    {
        m_rigidbody.mass = mass;                    // 質量を増やす。
        m_rigidbody.drag = drag;                    // 落下速度を速くする。
        m_rigidbody.useGravity = true;
        m_rigidbody.freezeRotation = true;
        m_rigidbody.isKinematic = false;            // 物理演算を適応する。
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
        RigidBodyParam(MASS, DRAG);
    }

    /// <summary>
    /// 死亡処理。
    /// </summary>
    public void Die()
    {
        // 2度目は実行しない。
        if(m_isDead == true)
        {
            return;
        }
        m_isDead = true;

        // ナビメッシュエージェントとアニメーションを停止
        m_agent.isStopped = true;
        m_enemyAnimator.SetTrigger("Die");

        // スケールを縮小するコルーチンを開始
        StartCoroutine(ShrinkAndDestroy());

        m_agent.enabled = true;
    }
}
