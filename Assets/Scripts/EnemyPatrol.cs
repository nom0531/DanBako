using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField, Header("巡回地点")]
    private Transform[] goals;  // 巡回地点
    private int destNum = 0;
    private NavMeshAgent agent;
    private Animator m_EnemyAnimator;
    private bool isWaiting = false;  // 待機中かどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        m_EnemyAnimator = GetComponent<Animator>(); // Animatorを一度だけ取得

        // 巡回地点が設定されているか確認
        if (goals.Length > 0)
        {
            // 最初の目的地を設定
            SetGoalPosition();
        }
        else
        {
            Debug.LogError("巡回地点が設定されていません！");
        }
    }

    void NextGoal()
    {
        // 巡回地点の番号を進める
        destNum = (destNum + 1) % goals.Length;  // 3つの地点を繰り返す

        // 次の目的地を設定
        SetGoalPosition();
        Debug.Log("現在の巡回地点: " + destNum);
    }

    // Y座標を変更せず、目的地を設定
    void SetGoalPosition()
    {
        // 現在の目的地のX, Z座標をそのまま使用し、Y座標は変更しない
        Vector3 newDestination = new Vector3(goals[destNum].position.x, goals[destNum].position.y, goals[destNum].position.z);
        agent.destination = newDestination;  // 新しい目的地を設定
    }

    // Update is called once per frame
    void Update()
    {
        // 目的地に到着したら次の巡回地点へ
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAtGoal());
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (agent.remainingDistance > 0.5f)
        {
            m_EnemyAnimator.SetBool("Run", true);  // 走る状態
        }
        else
        {
            m_EnemyAnimator.SetBool("Run", false); // 待機状態
        }
    }

    private IEnumerator WaitAtGoal()
    {
        isWaiting = true;

        // 待機アニメーションを再生
        m_EnemyAnimator.SetBool("Idle", true);

        // アニメーションの終了を待機
        yield return new WaitUntil(() => {
            AnimatorStateInfo stateInfo = m_EnemyAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Idle") && stateInfo.normalizedTime >= 1.0f;
        });

        // アニメーションが完了したら待機フラグをオフにし、次の地点へ
        m_EnemyAnimator.SetBool("Idle", false);
        NextGoal();
        isWaiting = false;
    }
}
