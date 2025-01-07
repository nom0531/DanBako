using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSwitch_Title : MonoBehaviour
{
    [SerializeField, Header("遷移先"),Tooltip("はじめから/つづきから")]
    private SceneChange Scene_StageSelect;
    [SerializeField, Tooltip("オプション")]
    private SceneChange Scene_Option;
    [SerializeField, Tooltip("ヘルプ")]
    private SceneChange Scene_Help;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    private SE SE_Determination;

    /// <summary>
    /// 選択中のコマンド。
    /// </summary>
    private enum TitleState
    {
        enFromBeginning,    // はじめから。
        enFromContinuation, // つづきから。
        enQuitGame,         // ゲーム終了。
        enOption,           // オプション。
        enHelp              // ヘルプ。
    }

    private SaveDataManager m_saveDataManager;
    private Gamepad m_gamepad;
    private Cursor m_cursor;
    private PlayAnimation m_playAnimation;
    private TitleState m_comandState = TitleState.enFromBeginning;
    private bool m_isPush = false;      // ボタンを押したならture。

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        m_playAnimation = GetComponent<PlayAnimation>();
        m_playAnimation.Play((int)m_comandState, "Active");
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
        ButtonDown();
    }

    /// <summary>
    /// コマンド選択処理。
    /// </summary>
    private void SelectCommand()
    {
        // ゲームパッドを取得。
        m_gamepad = Gamepad.current;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushDown();
        }

        if(m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.up.wasPressedThisFrame)
        {
            PushUp();
        }
        if (m_gamepad.dpad.down.wasPressedThisFrame)
        {
            PushDown();
        }
    }

    /// <summary>
    /// ↑キーを押したときの処理。
    /// </summary>
    private void PushUp()
    {
        int oldCcommandState = (int)m_comandState;
        m_comandState--;
        // 補正。
        if (m_comandState < TitleState.enFromBeginning)
        {
            m_comandState = TitleState.enHelp;
        }
        m_cursor.Move((int)m_comandState);
        // アニメーションを再生。
        m_playAnimation.Play(oldCcommandState, "NotActive");
        m_playAnimation.Play((int)m_comandState, "Active");
    }

    /// <summary>
    /// ↓キーを押したときの処理。
    /// </summary>
    private void PushDown()
    {
        int oldCcommandState = (int)m_comandState;
        m_comandState++;
        // 補正。
        if (m_comandState > TitleState.enHelp)
        {
            m_comandState = TitleState.enFromBeginning;
        }
        m_cursor.Move((int)m_comandState);
        // アニメーションを再生。
        m_playAnimation.Play(oldCcommandState, "NotActive");
        m_playAnimation.Play((int)m_comandState, "Active");
    }

    /// <summary>
    /// ボタン処理。
    /// </summary>
    private void ButtonDown()
    {
        // Bボタンを押したとき。
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
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
            case TitleState.enFromBeginning:
                FromBiginning();
                break;
            case TitleState.enFromContinuation:
                FromContinuation();
                break;
            case TitleState.enQuitGame:
                Quit();
                break;
            case TitleState.enOption:
                Option();
                break;
            case TitleState.enHelp:
                Help();
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

    /// <summary>
    /// ヘルプ選択時の処理。
    /// </summary>
    private void Help()
    {
        Scene_Help.CreateFadeCanvas();
    }
}
