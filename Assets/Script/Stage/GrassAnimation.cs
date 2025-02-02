using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
    // Animatorコンポーネント
    private Animator animator;

    // 再生するアニメーションステートの名前
    [SerializeField] private string animationStateName;

    // アニメーションの再生スピードの範囲
    [SerializeField] private Vector2 speedRange = new Vector2(0.1f, 0.5f);

    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animatorが見つかりません！");
            return;
        }

        // ランダムな再生スピードを設定
        float randomSpeed = Random.Range(speedRange.x, speedRange.y);
        animator.speed = randomSpeed;

        // ランダムな再生開始位置 (normalized time)
        float randomStartTime = Random.Range(0.0f, 2.0f);

        // アニメーションをランダムな位置から再生
        animator.Play(animationStateName, 0, randomStartTime);
    }
}
