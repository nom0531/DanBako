using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLocation : MonoBehaviour
{
    private Animator m_animator;
    private int m_ID = 0;               // 自身の番号。

    public int MyID
    {
        set => m_ID = value;
        get => m_ID;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// アニメーション処理。
    /// </summary>
    public void PlayAnimaton(string triggerName)
    {
        m_animator.SetTrigger(triggerName);
    }
}
