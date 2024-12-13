using System.Collections;
using UnityEngine;

public class MovingRock : MonoBehaviour
{
    Animator m_rockAnimator;
    GameTime m_gameTime;
    private bool isRewind = false;

    private void Start()
    {
        m_rockAnimator = GetComponent<Animator>();
        m_gameTime = GameObject.FindGameObjectWithTag("TimeObject").GetComponent<GameTime>();
    }

    private void Update()
    {
        // 停止していないなら実行しない。
        if(m_gameTime.TimeStopFlag == false)
        {
            return;
        }

        // Zキーが押されたらアニメーションを再生
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayAnimation();
        }

        // Xキーが押されたら巻き戻しを実行
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(TriggerRewindWithDelay());
        }
    }

    private void PlayAnimation()
    {
        isRewind = false; // 巻き戻しフラグをリセット
        m_rockAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオフ
        m_rockAnimator.SetBool("IsPlaying", true); // 再生フラグをオン
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
    }
}
