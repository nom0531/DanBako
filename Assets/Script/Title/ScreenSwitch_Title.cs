using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSwitch_Title : MonoBehaviour
{
    [SerializeField, Header("各遷移先"),Tooltip("はじめから/つづきから")]
    SceneChange Scene_StageSelect;
    [SerializeField, Tooltip("オプション")]
    SceneChange Scene_Option;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    SE SE_Determination;
    [SerializeField, Tooltip("セレクト音")]
    SE SE_Select;

    /// <summary>
    /// 選択中のコマンド。
    /// </summary>
    private enum ComandState
    {
        enFromBeginning,    // はじめから。
        enFromContinuation, // つづきから。
        enQuitGame,         // ゲーム終了。
        enOption            // オプション。
    }

    private SaveDataManager m_saveDataManager;
    private Gamepad m_gamepad;
    private Cursor m_cursor;
    private ComandState m_comandState = ComandState.enFromBeginning;
    private bool m_isPush = false;      // ボタンを押したならture。

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ボタンを押したなら実行しない。
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
        // ゲームパッドを取得。
        m_gamepad = Gamepad.current;

        // ↑キーを押したとき。
        if (m_gamepad.dpad.up.wasPressedThisFrame || Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_comandState--;
            // 補正。
            if(m_comandState < ComandState.enFromBeginning)
            {
                m_comandState = ComandState.enOption;
            }
            m_cursor.Move((int)m_comandState);
            SE_Select.PlaySE();
        }
        // ↓キーを押したとき。
        if (m_gamepad.dpad.down.wasPressedThisFrame || Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_comandState++;
            // 補正。
            if (m_comandState > ComandState.enOption)
            {
                m_comandState = ComandState.enFromBeginning;
            }
            m_cursor.Move((int)m_comandState);
            SE_Select.PlaySE();
        }
        Debug.Log(m_comandState);
    }

    /// <summary>
    /// 遷移処理。
    /// </summary>
    private void SceneChange()
    {
        // Aボタンを押したとき。
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            ButtonPush();
            m_isPush = true;
        }
    }

    /// <summary>
    /// ボタンを押したときの処理。
    /// </summary>
    private void ButtonPush()
    {
        // ステートに応じて処理を変更。
        switch (m_comandState){
            case ComandState.enFromBeginning:
                FromBiginning();
                break;
            case ComandState.enFromContinuation:
                FromContinuation();
                break;
            case ComandState.enQuitGame:
                Quit();
                break;
            case ComandState.enOption:
                Option();
                break;
        }
        SE_Determination.PlaySE();
    }

    /// <summary>
    /// はじめからを選択した場合の処理。
    /// </summary>
    private void FromBiginning()
    {
        m_saveDataManager.Delete(); // 以前までのデータを削除。
        Scene_StageSelect.CreateFadeCanvas();
    }

    /// <summary>
    /// つづきからを選択した場合の処理。
    /// </summary>
    private void FromContinuation()
    {
        Scene_StageSelect.CreateFadeCanvas();
    }

    /// <summary>
    /// ゲームを終了する処理。
    /// </summary>
    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// オプション選択時の処理。
    /// </summary>
    private void Option()
    {
        Scene_Option.CreateFadeCanvas();
    }
}
