using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �n�ʂɐݒu���Ă��邩�ǂ������ׂ�N���X�B
/// </summary>
public class GroundCheck : MonoBehaviour
{
    // �ڒn���Ă��邩���i�[����ϐ�
    bool m_isGround = false;

    public bool GroundCheckFlag
    {
        get => m_isGround;
        set => m_isGround = value;
    }

    // ���g�ɉ������Փ˂��Ă���ԌĂ΂��
    void OnTriggerStay(Collider other)
    {
        // �n�ʂ̃^�O���t�����I�u�W�F�N�g�ɏՓ˂��Ă���
        if (other.CompareTag("Ground"))
        {
            m_isGround = true;
        }
    }
}
