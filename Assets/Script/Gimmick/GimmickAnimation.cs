using System.Collections;
using UnityEngine;

public class GimmickAnimation : MonoBehaviour
{
    Animator m_animator;
    GameStatus m_gameStatus;
    private bool isRewind = false;
    private bool m_isNotStart = false;      // 初期状態ならfalse;
    private bool m_isPushButton = false;    // ボタンを押したならtrue。

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
    }

    private void Update()
    {
        // 停止していないなら実行しない。
        if(m_gameStatus.TimeStopFlag == false)
        {
            return;
        }

        // LBキーが押されたらアニメーションを再生
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.Y))
        {
            if(m_isPushButton == true)
            {
                return;
            }
            m_isPushButton = true;
            m_isNotStart = true;
            PlayAnimation();
        }

        // RBキーが押されたら巻き戻しを実行
        if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown(KeyCode.Z))
        {
            if (m_isNotStart == false)
            {
                return;
            }
            if (m_isPushButton == true)
            {
                return;
            }
            m_isPushButton = true;
            m_isNotStart = false;
            StartCoroutine(TriggerRewindWithDelay());
        }
    }

    private void PlayAnimation()
    {
        isRewind = false; // 巻き戻しフラグをリセット
        m_animator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオフ
        m_animator.SetBool("IsPlaying", true); // 再生フラグをオン
        m_isPushButton = false; // フラグを戻す。
    }

    IEnumerator TriggerRewindWithDelay()
    {
        // トリガーを設定
        m_animator.SetTrigger("Rewind");

        // 2秒待機
        yield return new WaitForSeconds(0.5f);

        // フラグを変更して巻き戻し開始
        TriggerRewind();
    }

    private void TriggerRewind()
    {
        isRewind = true; // 巻き戻しフラグを設定
        m_animator.SetBool("IsPlaying", false); // 再生フラグをオフ
        m_animator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオン
        m_isPushButton = false; // フラグを戻す。
    }
}
