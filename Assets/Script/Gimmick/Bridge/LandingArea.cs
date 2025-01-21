using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingArea : MonoBehaviour
{
    [SerializeField]
    private GameObject Bridge;

    private GameStatus m_gameStatus;

    private void Start()
    {
        m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
    }

    private void OnTriggerStay(Collider collision)
    {
        if(m_gameStatus.TimeStopFlag == true)
        {
            return;
        }
        // エネミーでないときは実行しない。
        if (collision.tag != "Enemy")
        {
            return;
        }
        if (Bridge.GetComponent<BridgeStatus>().LandingFlag == false)
        {
            // 落下していないのでフラグはfalse。
            var enemyPatrol = collision.GetComponent<EnemyPatrol_Main>();
            enemyPatrol.LandingFlag = false;
            enemyPatrol.RigidBodyParam(0, false);
            return;
        }
        collision.GetComponent<EnemyPatrol_Main>().Landing();
    }
}
