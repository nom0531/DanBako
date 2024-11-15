using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[�����̎��Ԃ𐧌䂷��N���X�B
/// </summary>
public class GameTime : MonoBehaviour
{
    /// <summary>
    /// ���Ԃ̍Đ����x�B
    /// </summary>
    enum TimeState
    {
        enStop,
        enDefault,
        enFast,
    }

    bool m_timeStop = false;    // ���Ԃ��~���Ă��邩�ǂ����B

    public bool TimeStopFlag
    {
        get => m_timeStop;
    }

    /// <summary>
    /// ���Ԃ��~���鏈���B
    /// </summary>
    public void StopTime()
    {
        Debug.Log("��~");
        Time.timeScale = (float)TimeState.enStop;
    }

    /// <summary>
    /// ���Ԃ̗����W���ɂ��鏈���B
    /// </summary>
    public void DefaultTime()
    {
        Debug.Log("�f�t�H���g");
        Time.timeScale = (float)TimeState.enDefault;
    }

    /// <summary>
    /// ���Ԃ�i�߂鏈���B
    /// </summary>
    public void AdvanceTime()
    {
        Debug.Log("�{��");
        Time.timeScale = (float)TimeState.enFast;
    }

    /// <summary>
    /// ���Ԃ�߂������B
    /// </summary>
    public void TurnbackTime()
    {
        Time.timeScale = (float)TimeState.enFast;
    }
}
