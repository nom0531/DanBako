using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Option : MonoBehaviour
{
    [SerializeField, Header("SE"), Tooltip("決定音")]
    SE SE_Determination;
    [SerializeField, Tooltip("キャンセル音")]
    SE SE_Chancel;
    [SerializeField, Tooltip("セレクト音")]
    SE SE_Select;

    /// <summary>
    /// 選択中のコマンド。
    /// </summary>
    private enum ComandState
    {
        enBGMSound,
        enSESound,
        enCamera,
        enReset
    }

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManager;
    private SceneChange m_sceneChange;
    private ComandState m_comandState = ComandState.enBGMSound;
    private bool m_isPush = false;    // ボタンを押したならture。

    // Start is called before the first frame update
    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_soundManager = GameManager.Instance.SoundManager;
        m_sceneChange = GetComponent<SceneChange>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_isPush == true)
        {
            return;
        }
        SelectCommand();
        SceneChange();
    }

    /// <summary>
    /// コマンド選択処理。
    /// </summary>
    private void SelectCommand()
    {
        // ↑キーを押したとき。
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_comandState--;
            // 補正。
            if (m_comandState < ComandState.enBGMSound)
            {
                m_comandState = ComandState.enReset;
            }
            SE_Select.PlaySE();
        }
        // ↓キーを押したとき。
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_comandState++;
            // 補正。
            if (m_comandState > ComandState.enReset)
            {
                m_comandState = ComandState.enBGMSound;
            }
            SE_Select.PlaySE();
        }
        Debug.Log(m_comandState);
    }

    /// <summary>
    /// 遷移処理。
    /// </summary>
    private void SceneChange()
    {
        // Bボタンを押したとき。
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            m_sceneChange.CreateFadeCanvas();
            SE_Chancel.PlaySE();
            m_isPush = true;
        }
    }

    /// <summary>
    /// ボタンを押したときの処理。
    /// </summary>
    private void ButtonPush()
    {
        // ステートに応じて処理を変更。
        switch (m_comandState)
        {
            case ComandState.enBGMSound:
                break;
            case ComandState.enSESound:
                break;
            case ComandState.enCamera:
                break;
            case ComandState.enReset:
                ResetStatus();
                break;
        }
        SE_Determination.PlaySE();
    }

    /// <summary>
    /// データを初期化する。
    /// </summary>
    private void ResetStatus()
    {
        // 音量をデフォルトに戻す。

        // カメラ回転をデフォルトに戻す。
        Debug.Log("初期値に戻す");
    }
}
