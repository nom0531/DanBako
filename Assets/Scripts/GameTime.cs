using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム中の時間を制御するクラス。
/// </summary>
public class GameTime : MonoBehaviour
{
    /// <summary>
    /// 時間の再生速度。
    /// </summary>
    enum TimeState
    {
        enStop,
        enDefault,
        enFast,
    }

    bool m_timeStop = false;    // 時間を停止しているかどうか。

    public bool TimeStopFlag
    {
        get => m_timeStop;
    }

    /// <summary>
    /// 時間を停止する処理。
    /// </summary>
    public void StopTime()
    {
        Debug.Log("停止");
        Time.timeScale = (float)TimeState.enStop;
    }

    /// <summary>
    /// 時間の流れを標準にする処理。
    /// </summary>
    public void DefaultTime()
    {
        Debug.Log("デフォルト");
        Time.timeScale = (float)TimeState.enDefault;
    }

    /// <summary>
    /// 時間を進める処理。
    /// </summary>
    public void AdvanceTime()
    {
        Debug.Log("倍速");
        Time.timeScale = (float)TimeState.enFast;
    }

    /// <summary>
    /// 時間を戻す処理。
    /// </summary>
    public void TurnbackTime()
    {
        Time.timeScale = (float)TimeState.enFast;
    }
}
