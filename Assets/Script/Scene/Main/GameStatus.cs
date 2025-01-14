using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    // Start is called before the first frame update
    private bool m_isChangeCamera = false;     // カメラが変更されたならtrue。

    public bool ChangeCamaeraFlag
    {
        get => m_isChangeCamera;
        set => m_isChangeCamera = value;
    }

    private void Awake()
    {
        SceneManager.LoadScene("Main_UI", LoadSceneMode.Additive);
    }
}
