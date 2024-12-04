using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Main : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("GameOver")]
    private SceneChange GameOver;

    private PlayTimeline m_playTimeline;

    // Start is called before the first frame update
    private void Start()
    {
        m_playTimeline = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayTimeline>();
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
        m_playTimeline.GameClear();
    }
}
