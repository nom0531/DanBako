using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toge : MonoBehaviour
{
    private BoxCollider m_boxCollider;
    private Animator m_animator;

    void Start()
    {
        // BoxColliderを取得
        m_boxCollider = GetComponent<BoxCollider>();
        if (m_boxCollider == null)
        {
            Debug.LogError("BoxColliderが見つかりません。");
        }

        // 自分自身のAnimatorを取得
        m_animator = GetComponent<Animator>();
        if (m_animator == null)
        {
            Debug.LogError("Animatorが見つかりません。");
        }
    }

    void Update()
    {
        if (m_animator == null || m_boxCollider == null)
        {
            return;
        }

        // アニメーションのフラグに基づいてBoxColliderを表示/非表示にする
        if (m_animator.GetBool("IsPlaying"))
        {
            UpdateBoxCollider(false); // BoxColliderを有効化
        }
        else
        {
            UpdateBoxCollider(true); // BoxColliderを無効化
        }
    }

    private void UpdateBoxCollider(bool isActive)
    {
        if (m_boxCollider != null)
        {
            m_boxCollider.enabled = isActive;
        }
    }
}
