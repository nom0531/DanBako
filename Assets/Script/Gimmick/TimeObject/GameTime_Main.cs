using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameTime_Main : MonoBehaviour
{
    private bool m_timeStop = false; // ゲーム全体の停止フラグ
    public bool TimeStopFlag => m_timeStop;

    [SerializeField, Header("停止対象のタグリスト")]
    private List<string> stopTags = new List<string> { "Enemy", "Environment" }; // 停止対象のタグ

    private List<MonoBehaviour> affectedScripts = new List<MonoBehaviour>();
    private List<Animator> affectedAnimators = new List<Animator>(); // アニメーターを保存するリスト
    private List<NavMeshAgent> affectedNavAgents = new List<NavMeshAgent>(); // NavMeshAgent を保存するリスト

    [SerializeField] private Animator targetAnimator; // 対象オブジェクトのアニメーター

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTimeForOthers();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResumeTimeForOthers();
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
