using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : ChangeVCam
{
    private GameStatus m_gameStatus;
    private SaveDataManager m_saveDataManager;

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーでないなら実行しない。
        if(other.tag != "Player")
        {
            return;
        }
        ResetPriority();
        // 既にカメラが変更されているなら。
        if (m_gameStatus.ChangeCamaeraFlag == true)
        {
            Change(0);
            m_gameStatus.ChangeCamaeraFlag = false;
        }
        else
        {
            Change(1);
            m_gameStatus.ChangeCamaeraFlag = true;
        }
    }

    /// <summary>
    /// メインで使用する仮想カメラを変更する。
    /// </summary>
    /// <param name="number">番号。</param>
    private void Change(int number)
    {
        ChangeVcam(number);
        Vcam_Stanging[number].GetComponent<GameCamera>().RotReverse = m_saveDataManager.CameraStete;
    }
}
