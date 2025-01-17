using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingArea : MonoBehaviour
{
    [SerializeField]
    private GameObject Bridge;

    private void OnTriggerEnter(Collider collision)
    {
        // エネミーでないときは実行しない。
        if(collision.tag != "Enemy")
        {
            return;
        }
        if(Bridge.GetComponent<BridgeStatus>().LandingFlag == false)
        {
            return;
        }
        collision.GetComponent<EnemyPatrol_Main>().Landing();
    }
}
