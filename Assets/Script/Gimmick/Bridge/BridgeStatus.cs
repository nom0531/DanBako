using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeStatus : MonoBehaviour
{
    private GimmickAnimation m_gimmickAnimation;
    private bool m_isLanding = false;           // 落下したならばture。

    public bool LandingFlag
    {
        get => m_isLanding;
    }

    private void Start()
    {
        m_gimmickAnimation = GetComponent<GimmickAnimation>();
    }

    private void Update()
    {
        if(m_isLanding == m_gimmickAnimation.NotStartFlag)
        {
            return;
        }
        // フラグを切り替える。
        m_isLanding = m_gimmickAnimation.NotStartFlag;

        Debug.Log(m_isLanding);
    }
}
