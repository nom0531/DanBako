using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField, Header("巡回地点 (座標)")]
    private Vector3[] goals;

    private int destNum = 0;
    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private bool isWaiting = false;

    [SerializeField, Header("プレイヤー")]
    private Transform player;
    [SerializeField]
    private float chaseRange = 10f;  // 追跡範囲
    [SerializeField]
    private float attackRange = 2f;  // 攻撃範囲
    [SerializeField]
    private float attackCooldown = 2f;  // 攻撃のクールダウン時間

    private bool isChasing = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    [SerializeField, Header("ゲーム時間管理オブジェクト")]
    private GameTime gameTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

        if (goals.Length > 0)
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
        if (gameTime != null && gameTime.TimeStopFlag)
        {
            HandleTimeStop();
            return;
        }

        ResumeFromTimeStop();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && !isChasing)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > chaseRange && isChasing)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else if (!isAttacking)
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
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAtGoal());
        }
    }

    private void SetGoalPosition()
    {
        agent.destination = goals[destNum];
    }

    private void NextGoal()
    {
        destNum = (destNum + 1) % goals.Length;
        SetGoalPosition();
    }

    private System.Collections.IEnumerator WaitAtGoal()
    {
        isWaiting = true;
        agent.isStopped = true;

        enemyAnimator.SetBool("Run", false);
        enemyAnimator.SetBool("Idle", true);

        yield return new WaitForSeconds(2f);

        enemyAnimator.SetBool("Idle", false);
        agent.isStopped = false;
        NextGoal();
        isWaiting = false;
    }

    private void UpdateAnimation()
    {
        if (agent.velocity.magnitude > 0.1f && !agent.isStopped)
        {
            enemyAnimator.SetBool("Run", true);
        }
        else
        {
            enemyAnimator.SetBool("Run", false);
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.destination = player.position;
    }

    private void AttackPlayer()
    {
        transform.LookAt(player.position);

        if (isAttacking || Time.time < lastAttackTime + attackCooldown) return;

        isAttacking = true;
        agent.isStopped = true;

        enemyAnimator.SetTrigger("Attack");

        lastAttackTime = Time.time;

        // 攻撃終了後に再び動けるようにする
        Invoke(nameof(EndAttack), 1.5f); // アニメーションの終了時間に合わせて調整
    }

    // アニメーションイベントから呼ばれるメソッド
    public void ApplyDamage()
    {
        if (player.TryGetComponent<Player>(out Player playerScript))
        {
            Debug.Log("アニメーションイベントでプレイヤーにダメージを適用");
            playerScript.TakeDamage(1); // 例として1ダメージ
        }
    }



    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }

    // 時間停止時の処理
    private void HandleTimeStop()
    {
        if (!agent.isStopped)
        {
            agent.isStopped = true;
            enemyAnimator.speed = 0f; // アニメーションを停止
        }
    }

    // 時間停止から再開時の処理
    private void ResumeFromTimeStop()
    {
        if (agent.isStopped && !gameTime.TimeStopFlag)
        {
            agent.isStopped = false;
            enemyAnimator.speed = 1f; // アニメーションを再開
        }
    }
}
