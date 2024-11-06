using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] goals;  // 巡回地点
    private int destNum = 0;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 巡回地点が設定されているか確認
        if (goals.Length > 0)
        {
            agent.destination = goals[destNum].position;
        }
        else
        {
            Debug.LogError("巡回地点が設定されていません！");
        }
    }

    void nextGoal()
    {
        // 巡回地点の番号を進める
        destNum = (destNum + 1) % goals.Length;  // 3つの地点を繰り返す

        agent.destination = goals[destNum].position;
        Debug.Log("現在の巡回地点: " + destNum);
    }

    // Update is called once per frame
    void Update()
    {
        // 目的地に到着したら次の巡回地点へ
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            nextGoal();
        }
    }
}
