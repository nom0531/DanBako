using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickAnimations : MonoBehaviour
{
    Animator m_animator;
   
    private bool isRewind = false;
    private bool m_isNotStart = false;      // 初期状態ならfalse;
    private bool m_isPushButton = false;    // ボタンを押したならtrue。

    public List<Animator> targetAnimators;  // アニメーションを適用するAnimatorをリストで指定

    private void Start()
    {
       

        // targetAnimatorsにAnimatorを設定（例：特定のオブジェクトにアタッチされているAnimator）
        if (targetAnimators.Count == 0)
        {
            m_animator = GetComponent<Animator>(); // defaultAnimator (必要なら)
        }
    }

    private void Update()
    {
       

        // LBキーが押されたらアニメーションを再生
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.Y))
        {
            if (m_isPushButton == true)
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

        // 指定したAnimatorすべてにアニメーションを適用
        foreach (var targetAnimator in targetAnimators)
        {
            targetAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオフ
            targetAnimator.SetBool("IsPlaying", true); // 再生フラグをオン
        }

        m_isPushButton = false; // フラグを戻す。
    }

    IEnumerator TriggerRewindWithDelay()
    {
        // トリガーを設定
        foreach (var targetAnimator in targetAnimators)
        {
            targetAnimator.SetTrigger("Rewind");
        }

        // 0.5秒待機
        yield return new WaitForSeconds(0.5f);

        // フラグを変更して巻き戻し開始
        TriggerRewind();
    }

    private void TriggerRewind()
    {
        isRewind = true; // 巻き戻しフラグを設定

        // 指定したAnimatorすべてに巻き戻しアニメーションを適用
        foreach (var targetAnimator in targetAnimators)
        {
            targetAnimator.SetBool("IsPlaying", false); // 再生フラグをオフ
            targetAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオン
        }

        m_isPushButton = false; // フラグを戻す。
    }
}
