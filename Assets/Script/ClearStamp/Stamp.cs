using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// アニメーションを再生する。
    /// </summary>
    public void PlayAnimation()
    {
        m_animator.SetTrigger("Active");
    }
}
