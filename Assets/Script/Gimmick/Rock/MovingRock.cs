using System.Collections;
using UnityEngine;

public class MovingRock : MonoBehaviour
{
    Animator m_RockAnimator;
    private bool isRewind = false;

    private void Start()
    {
        m_RockAnimator = GetComponent<Animator>();
    }

    void Update()
    {
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

    void PlayAnimation()
    {
        isRewind = false; // 巻き戻しフラグをリセット
        m_RockAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオフ
        m_RockAnimator.SetBool("IsPlaying", true); // 再生フラグをオン
    }

    IEnumerator TriggerRewindWithDelay()
    {
        // トリガーを設定
        m_RockAnimator.SetTrigger("Rewind");

        // 2秒待機
        yield return new WaitForSeconds(1.0f);

        // フラグを変更して巻き戻し開始
        TriggerRewind();
    }

    void TriggerRewind()
    {
        isRewind = true; // 巻き戻しフラグを設定
        m_RockAnimator.SetBool("IsPlaying", false); // 再生フラグをオフ
        m_RockAnimator.SetBool("IsRewind", isRewind); // 巻き戻しフラグをオン
    }
}
