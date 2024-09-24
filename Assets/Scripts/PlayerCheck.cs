using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�^�O�����Ă��邩���ʂ���N���X�B
/// </summary>
public class PlayerCheck : MonoBehaviour
{
    GameTime m_gameTime;
    bool m_stopTimeFlag = false;

    private void Start()
    {
        m_gameTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTime>();
    }

    private void Update()
    {
        if (m_stopTimeFlag)
        {
            m_gameTime.StopTime();
            return;
        }
        m_gameTime.DefaultTime();
    }

    // ���g�ɉ������Փ˂��Ă���ԌĂ΂��
    void OnTriggerStay(Collider other)
    {
        // ����̃^�O���t�����I�u�W�F�N�g�ɏՓ˂��Ă���
        if (!other.CompareTag("Player"))
        {
            m_stopTimeFlag = false;
            Debug.Log("�G���A�͈̔͊O");
            return;
        }
        m_stopTimeFlag = true;
        Debug.Log("�G���A�͈͓̔�");
        if (Input.GetKey(KeyCode.Space))
        {
            m_gameTime.AdvanceTime();
        }
    }
}
