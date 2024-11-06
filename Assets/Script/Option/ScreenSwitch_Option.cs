using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 選択中のコマンド。
/// </summary>
public enum OptionState
{
    enBGMSound,
    enSESound,
    enCamera,
    enReset,
    enBGMParamator,     // BGMのパラメータ。
    enSEParamator,      // SEのパラメータ。
    enCameraParamator,  // カメラのパラメータ。
}

public class ScreenSwitch_Option : MonoBehaviour
{
    [SerializeField, Header("SE"), Tooltip("決定音")]
    private SE SE_Determination;
    [SerializeField, Tooltip("キャンセル音")]
    private SE SE_Cancel;
    [SerializeField, Tooltip("カーソル移動音")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("エラー音")]
    private SE SE_Error;

    private SaveDataManager m_saveDataManager;
    private Cursor m_cursor;
    private SceneChange m_sceneChange;
    private SetParamator m_setParamator;
    private Gamepad m_gamepad;
    private OptionState m_comandState = OptionState.enBGMSound;
    private bool m_isPush = false;    // ボタンを押したならture。

    // Start is called before the first frame update
    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        m_sceneChange = GetComponent<SceneChange>();
        m_setParamator = GetComponent<SetParamator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_isPush == true)
        {
            return;
        }
        SelectCommand();
        ButtonDown();
    }

    /// <summary>
    /// ボタン処理。
    /// </summary>
    private void ButtonDown()
    {
        // Aボタンを押したとき。
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            SceneChange();
            ChangeState();
            SE_Cancel.PlaySE();
            m_saveDataManager.Save();
        }
        // Bボタンを押したとき。
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            SE_Determination.PlaySE();
        }
    }

    /// <summary>
    /// シーンを切り替える処理。
    /// </summary>
    private void SceneChange()
    {
        if(m_comandState > OptionState.enReset)
        {
            return;
        }
        // シーンを切り替える。
        m_sceneChange.CreateFadeCanvas();
        m_isPush = true;
    }

    /// <summary>
    /// ステートを変更する。
    /// </summary>
    private void ChangeState()
    {
        if(m_comandState < OptionState.enBGMParamator)
        {
            return;
        }
        // 選択中のコマンドを変更する。
        m_comandState -= 4;
        m_cursor.Move((int)m_comandState);
    }

    /// <summary>
    /// コマンド選択処理。
    /// </summary>
    private void SelectCommand()
    {
        // ゲームパッドを取得。
        m_gamepad = Gamepad.current;

        CursorMove();
        m_setParamator.Set(m_comandState, m_gamepad);
    }

    /// <summary>
    /// カーソル移動。
    /// </summary>
    private void CursorMove()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushDown();
        }

        // ゲームパッドが接続されていない場合。
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
        if (m_comandState > OptionState.enReset)
        {
            SE_Error.PlaySE();
            return;
        }
        m_comandState--;
        // 補正。
        if (m_comandState < OptionState.enBGMSound)
        {
            m_comandState = OptionState.enReset;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ↓キーを押したときの処理。
    /// </summary>
    private void PushDown()
    {
        if (m_comandState > OptionState.enReset)
        {
            SE_Error.PlaySE();
            return;
        }
        m_comandState++;
        // 補正。
        if (m_comandState > OptionState.enReset)
        {
            m_comandState = OptionState.enBGMSound;
        }
        m_cursor.Move((int)m_comandState);
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ボタンを押したときの処理。
    /// </summary>
    private void ButtonPush()
    {
        // ステートに応じて処理を変更。
        switch (m_comandState)
        {
            case OptionState.enBGMSound:
                m_comandState = OptionState.enBGMParamator;
                break;
            case OptionState.enSESound:
                m_comandState = OptionState.enSEParamator;
                break;
            case OptionState.enCamera:
                m_comandState = OptionState.enCameraParamator;
                break;
            case OptionState.enReset:
                m_setParamator.ResetStatus();
                break;
        }
        m_cursor.Move((int)m_comandState);
    }
}
