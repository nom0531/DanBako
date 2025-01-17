using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopEnemy : MonoBehaviour
{
    [SerializeField, Header("停止させたいエネミー")]
    private GameObject[] Enemys;

    private EnemyPatrol_Main[] m_enemyPatrol;
    private bool m_isMoveEnemys = false;           // 停止しているならtrue。

    // Start is called before the first frame update
    private void Start()
    {
        m_enemyPatrol = new EnemyPatrol_Main[Enemys.Length];
        for (int i=0; i < Enemys.Length; i++)
        {
            m_enemyPatrol[i] = Enemys[i].GetComponent<EnemyPatrol_Main>();
        }
        StopBehavior(m_isMoveEnemys);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーでないなら実行しない。
        if (other.tag != "Player")
        {
            return;
        }
        StopBehavior(m_isMoveEnemys);
    }

    /// <summary>
    /// 指定したエネミーの動作を停止させる。
    /// </summary>
    /// <param name="flag">停止するならture。そうでないならfalse。</param>
    private void StopBehavior(bool flag)
    {
        for(int i = 0; i < Enemys.Length; i++)
        {
            m_enemyPatrol[i].NotMoveFlag = !flag;
        }
        m_isMoveEnemys = !flag;
    }
}
