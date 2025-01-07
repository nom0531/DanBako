using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private GameManager m_gameManager;
    private SaveDataManager m_saveDataManager;

    private int m_hours = 0;
    private int m_minute = 0;
    private float m_seconds = 0.0f;
    //　最初の時間
    private float m_startTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Instance;
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }
        if(m_gameManager.GameMode == CurrentGameMode.enClear)
        {
            m_saveDataManager.Stage[m_gameManager.StageID].ClearTime.Hour = m_hours;
            m_saveDataManager.Stage[m_gameManager.StageID].ClearTime.Minute = m_minute;
            m_saveDataManager.Stage[m_gameManager.StageID].ClearTime.Seconds = m_seconds;
            return;
        }
        // 時間を計測。
        m_seconds = Time.time - m_startTime;
        m_minute = (int)m_seconds / 60;
        m_hours = m_minute / 60;
    }
}
