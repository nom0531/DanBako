using System.Collections;
using UnityEngine;

public class MovingRock_Main : MonoBehaviour
{
    Animator m_rockAnimator;
    GameTime_Main m_gameTime;
    private bool isRewind = false;
    private bool m_isPushButton = false;    // ボタンを押したならtrue。

    private void Start()
    {
        m_rockAnimator = GetComponent<Animator>();
        m_gameTime = GameObject.FindGameObjectWithTag("TimeObject").GetComponent<GameTime_Main>();
    }

    private void Update()
    {
        // 停止していないなら実行しない。
        if(m_gameTime.TimeStopFlag == false)
        {
            return;
        }

        // LBキーが押されたらアニメーションを再生
        if (Input.GetKeyDown("joystick button 4"))
        {
            if(m_isPushButton == true)
            {
                return;
            }
            m_isPushButton = true;
            PlayAnimation();
        }

        // RBキーが押されたら巻き戻しを実行
        if (Input.GetKeyDown("joystick button 5"))
        {
            if (m_isPushButton == true)
            {
                return;
            }
            m_isPushButton = true;
            StartCoroutine(TriggerRewindWithDelay());
        }
    }

    private void PlayAnimation()
    {
        isRewind = false; // 巻き戻しフラグをリセット
        m_rockAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオフ
        m_rockAnimator.SetBool("IsPlaying", true); // 再生フラグをオン
        m_isPushButton = false; // フラグを戻す。
    }

    IEnumerator TriggerRewindWithDelay()
    {
        // トリガーを設定
        m_rockAnimator.SetTrigger("Rewind");

        // 2秒待機
        yield return new WaitForSeconds(1.0f);

        // フラグを変更して巻き戻し開始
        TriggerRewind();
    }

    private void TriggerRewind()
    {
        isRewind = true; // 巻き戻しフラグを設定
        m_rockAnimator.SetBool("IsPlaying", false); // 再生フラグをオフ
        m_rockAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオン
        m_isPushButton = false; // フラグを戻す。
    }
}
