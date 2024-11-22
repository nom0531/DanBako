using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Main : MonoBehaviour
{
    [SerializeField]
    private StageDataBase StageData;
    [SerializeField, Header("遷移先"), Tooltip("GameOver")]
    private SceneChange GameOver;
    [SerializeField, Tooltip("GameClear")]
    private GameObject GameClear;

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayGameClear();
        }
    }

    /// <summary>
    /// ゲームオーバー演出。
    /// </summary>
    public void PlayGameOver()
    {
        GameOver.CreateFadeCanvas();
    }

    /// <summary>
    /// ゲームクリア演出。
    /// </summary>
    public void PlayGameClear()
    {
        Debug.Log("ステージクリア");
    }
}
