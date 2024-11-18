using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Main : MonoBehaviour
{
    [SerializeField]
    private StageDataBase StageData;

    private GameManager m_gameManager;
    private BGM m_bgm;

    // Start is called before the first frame update
    private void Start()
    {
        m_gameManager = GameManager.Instance;

        m_bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        m_bgm.SetBGM = StageData.stageDataList[m_gameManager.StageID].BGM;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameClear();
        }
    }

    private void GameOver()
    {
        Debug.Log("ゲームオーバー");
    }

    private void GameClear()
    {
        Debug.Log("ステージクリア");
    }
}
