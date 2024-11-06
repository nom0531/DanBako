using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] goals;  // ����n�_
    private int destNum = 0;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // ����n�_���ݒ肳��Ă��邩�m�F
        if (goals.Length > 0)
        {
            agent.destination = goals[destNum].position;
        }
        else
        {
            Debug.LogError("����n�_���ݒ肳��Ă��܂���I");
        }
    }

    void nextGoal()
    {
        // ����n�_�̔ԍ���i�߂�
        destNum = (destNum + 1) % goals.Length;  // 3�̒n�_���J��Ԃ�

        agent.destination = goals[destNum].position;
        Debug.Log("���݂̏���n�_: " + destNum);
    }

    // Update is called once per frame
    void Update()
    {
        // �ړI�n�ɓ��������玟�̏���n�_��
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            nextGoal();
        }
    }
}
