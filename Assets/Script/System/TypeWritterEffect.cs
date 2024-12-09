using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWritterEffect : MonoBehaviour
{
    [SerializeField]
    private TextMessageDataBase textMessageData;
    [SerializeField, Header("次の文字を表示するまでの時間")]
    private float DelayDuration = 0.1f;

    private TMP_Text m_text;                        // 対象のテキスト。
    private float m_timer = 0.0f;                   // 次の文字を表示するまでのタイマー。
    private int m_currentMaxVisibleCharacters = 0;
    private bool m_isRunning = false;               // 演出を行うならtrue。

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// テキストの演出を開始する。
    /// </summary>
    /// <param name="number">表示したいテキストのID。</param>
    public void Show(int number)
    {
        gameObject.SetActive(true);
        m_text.text = textMessageData.textMessegeDataList[number].Detail;
        m_isRunning = true;
        m_timer = 0.1f;
        m_currentMaxVisibleCharacters = 0;
    }

    private void Update()
    {
        ButtonPush();
        // 演出を行わないなら中断。
        if (m_isRunning == false)
        {
            return;
        }
        WriteText();
    }

    /// <summary>
    /// テキストを1文字ずつ表示する。
    /// </summary>
    private void WriteText()
    {


        // 次の文字表示までの残り時間を更新。
        m_timer -= Time.deltaTime;
        if(m_timer > 0.0f)
        {
            return;
        }
        // 表示する文字数を一つ増やす。
        m_text.maxVisibleCharacters = ++m_currentMaxVisibleCharacters;
        m_timer = DelayDuration;
        // 文字全てを表示し終わったなら終了。
        if(m_currentMaxVisibleCharacters >= m_text.text.Length)
        {
            m_isRunning = false;
        }
    }

    /// <summary>
    /// ボタンを押したときの処理。
    /// </summary>
    private void ButtonPush()
    {
        if(m_isRunning == true)
        {

        }
        else
        {

        }
    }
}
