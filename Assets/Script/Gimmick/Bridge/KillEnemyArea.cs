using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Enemy")
        {
            return;
        }
        other.GetComponent<EnemyPatrol_Main>().Die();
    }
}
