using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField, Header("����n�_")]
    private Transform[] goals;  // ����n�_
    private int destNum = 0;
    private NavMeshAgent agent;
    private Animator m_EnemyAnimator;
    private bool isWaiting = false;  // �ҋ@�����ǂ����̃t���O

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        m_EnemyAnimator = GetComponent<Animator>(); // Animator����x�����擾

        // ����n�_���ݒ肳��Ă��邩�m�F
        if (goals.Length > 0)
        {
            // �ŏ��̖ړI�n��ݒ�
            SetGoalPosition();
        }
        else
        {
            Debug.LogError("����n�_���ݒ肳��Ă��܂���I");
        }
    }

    void NextGoal()
    {
        // ����n�_�̔ԍ���i�߂�
        destNum = (destNum + 1) % goals.Length;  // 3�̒n�_���J��Ԃ�

        // ���̖ړI�n��ݒ�
        SetGoalPosition();
        Debug.Log("���݂̏���n�_: " + destNum);
    }

    // Y���W��ύX�����A�ړI�n��ݒ�
    void SetGoalPosition()
    {
        // ���݂̖ړI�n��X, Z���W�����̂܂܎g�p���AY���W�͕ύX���Ȃ�
        Vector3 newDestination = new Vector3(goals[destNum].position.x, goals[destNum].position.y, goals[destNum].position.z);
        agent.destination = newDestination;  // �V�����ړI�n��ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        // �ړI�n�ɓ��������玟�̏���n�_��
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
            m_EnemyAnimator.SetBool("Run", true);  // ������
        }
        else
        {
            m_EnemyAnimator.SetBool("Run", false); // �ҋ@���
        }
    }

    private IEnumerator WaitAtGoal()
    {
        isWaiting = true;

        // �ҋ@�A�j���[�V�������Đ�
        m_EnemyAnimator.SetBool("Idle", true);

        // �A�j���[�V�����̏I����ҋ@
        yield return new WaitUntil(() => {
            AnimatorStateInfo stateInfo = m_EnemyAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Idle") && stateInfo.normalizedTime >= 1.0f;
        });

        // �A�j���[�V����������������ҋ@�t���O���I�t�ɂ��A���̒n�_��
        m_EnemyAnimator.SetBool("Idle", false);
        NextGoal();
        isWaiting = false;
    }
}
