using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameTime : MonoBehaviour
{
    private bool m_timeStop = false; // ゲーム全体の停止フラグ
    public bool TimeStopFlag => m_timeStop;

    [SerializeField, Header("停止対象のタグリスト")]
    private List<string> stopTags = new List<string> { "Enemy", "Environment" }; // 停止対象のタグ

    private List<MonoBehaviour> affectedScripts = new List<MonoBehaviour>();
    private List<Animator> affectedAnimators = new List<Animator>(); // アニメーターを保存するリスト
    private List<NavMeshAgent> affectedNavAgents = new List<NavMeshAgent>(); // NavMeshAgent を保存するリスト

    private bool isPlayerInTrigger = false; // プレイヤーがトリガー内にいるかどうか

    [SerializeField] private Animator targetAnimator; // 対象オブジェクトのアニメーター

    private void Update()
    {
        // プレイヤーが接触している状態でHキーが押された場合、時間を停止/再開
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.H))
        {
            if (m_timeStop)
            {
                ResumeTimeForOthers();
                Debug.Log("時間再開");
            }
            else
            {
                StopTimeForOthers();
                Debug.Log("時間停止");
            }
        }

        // 時間停止中に特定のアニメーションを再生/逆再生
        if (m_timeStop)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                // アニメーションを再生（前進）
                if (targetAnimator != null)
                {
                    targetAnimator.Play("AnimationName"); // "AnimationName"は再生したいアニメーションの名前に変更
                    Debug.Log("アニメーション再生");
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                // アニメーションを逆再生
                if (targetAnimator != null)
                {
                    targetAnimator.Play("AnimationName", -1, 1); // 逆再生を実行
                    Debug.Log("アニメーション逆再生");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true; // プレイヤーがトリガー内に入った
            Debug.Log("プレイヤー接触 - 時間操作可能");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false; // プレイヤーがトリガー外に出た
            Debug.Log("プレイヤー離脱 - 時間操作不可");
        }
    }

    /// <summary>
    /// プレイヤー以外の動作を停止する
    /// </summary>
    private void StopTimeForOthers()
    {
        m_timeStop = true;

        foreach (string tag in stopTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                // 各オブジェクトのスクリプトを停止
                MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    if (script.enabled && script != this) // 自分自身を無効化しない
                    {
                        affectedScripts.Add(script);
                        script.enabled = false;
                    }
                }

                // アニメーターを無効化
                Animator animator = obj.GetComponent<Animator>();
                if (animator != null)
                {
                    affectedAnimators.Add(animator);
                    animator.enabled = false; // アニメーターを無効化
                }

                // NavMeshAgent を無効化
                NavMeshAgent navAgent = obj.GetComponent<NavMeshAgent>();
                if (navAgent != null)
                {
                    affectedNavAgents.Add(navAgent);
                    navAgent.isStopped = true; // エネミーの移動を停止
                    navAgent.velocity = Vector3.zero; // エネミーの速度をゼロにして完全に停止
                }
            }
        }
    }

    /// <summary>
    /// プレイヤー以外の動作を再開する
    /// </summary>
    private void ResumeTimeForOthers()
    {
        m_timeStop = false;

        // 停止していたスクリプトを再有効化
        foreach (MonoBehaviour script in affectedScripts)
        {
            script.enabled = true;
        }
        affectedScripts.Clear();

        // 停止していたアニメーターを再有効化
        foreach (Animator animator in affectedAnimators)
        {
            animator.enabled = true; // アニメーターを再有効化
        }
        affectedAnimators.Clear();

        // 停止していたNavMeshAgentを再開
        foreach (NavMeshAgent navAgent in affectedNavAgents)
        {
            navAgent.isStopped = false; // エネミーの移動を再開
            navAgent.velocity = Vector3.zero; // 速度をゼロにして移動を強制的に開始
        }
        affectedNavAgents.Clear();
    }
}