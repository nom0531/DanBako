using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    // Start is called before the first frame update
    private bool m_isChangeCamera = false;      // カメラが変更されたならtrue。
    private bool m_timeStop = false;            // ゲーム全体の停止フラグ。

    public bool ChangeCamaeraFlag
    {
        get => m_isChangeCamera;
        set => m_isChangeCamera = value;
    }

    public bool TimeStopFlag
    {
        get => m_timeStop;
        set => m_timeStop = value;
    }

    private void Awake()
    {
        SceneManager.LoadScene("Main_UI", LoadSceneMode.Additive);
    }
}
