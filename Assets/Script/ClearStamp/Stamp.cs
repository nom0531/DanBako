using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    private SaveDataManager m_saveDataManager;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_animator = GetComponent<Animator>();

    }

    /// <summary>
    /// アニメーションを再生する。
    /// </summary>
    private void PlayAnimation()
    {

    }
}
