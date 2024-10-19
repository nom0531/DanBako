using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Title : MonoBehaviour
{
    /// <summary>
    /// 選択中のコマンド。
    /// </summary>
    private enum ComandState
    {
        enFromBeginning,    // はじめから。
        enFromContinuation, // つづきから。
        enGameEnd,          // ゲーム終了。
        enOption            // オプション。
    }

    [SerializeField, Header("各遷移先"),Tooltip("はじめから/つづきから")]
    SceneChange Scene_StageSelect;
    [SerializeField, Tooltip("オプション")]
    SceneChange Scene_Option;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    SE SE_Determination;
    [SerializeField, Tooltip("セレクト音")]
    SE SE_Select;
    [SerializeField, Header("Animation")]
    Animator[] Animators;

    private SaveDataManager m_saveDataManager;
    private ComandState m_comandState = ComandState.enFromBeginning;
    private bool m_isPush = false;      // ボタンを押したならture。

    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
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

        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.K))
        {
            ButtonPush();
            m_isPush = true;
        }
    }

    /// <summary>
    /// コマンド選択処理。
    /// </summary>
    private void SelectCommand()
    {
        Animators[(int)m_comandState].SetBool("IsSelect", false);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_comandState--;
            if(m_comandState < ComandState.enFromBeginning)
            {
                m_comandState = ComandState.enOption;
            }
            SE_Determination.PlaySE();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_comandState++;
            if (m_comandState > ComandState.enOption)
            {
                m_comandState = ComandState.enFromBeginning;
            }
            SE_Determination.PlaySE();
        }
        Animators[(int)m_comandState].SetBool("IsSelect",true);
        Animators[(int)m_comandState].SetTrigger("Select");
    }

    /// <summary>
    /// ボタンを押したときの処理
    /// </summary>
    private void ButtonPush()
    {
        switch (m_comandState){
            case ComandState.enFromBeginning:
                FromBiginning();
                break;
            case ComandState.enFromContinuation:
                FromContinuation();
                break;
            case ComandState.enGameEnd:
                Quit();
                break;
            case ComandState.enOption:
                Option();
                break;
        }
        SE_Select.PlaySE();
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
