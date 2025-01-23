using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeStatus : MonoBehaviour
{
    [SerializeField, Header("ボックスコライダー")]
    private GameObject[] BoxColliders;
    [SerializeField, Header("フラグを反転させるかどうか")]
    private bool IsReversFlag = false;

    private GimmickAnimation m_gimmickAnimation;
    private bool m_isLanding = false;               // 落下したならばture。

    public bool LandingFlag
    {
        get => m_isLanding;
    }

    private void Start()
    {
        m_gimmickAnimation = GetComponent<GimmickAnimation>();
        SetLandingFlag();
    }

    private void Update()
    {
        if (IsReversFlag == false && m_gimmickAnimation.NotStartFlag == m_isLanding)
        {
            return;
        }
        if(IsReversFlag == true && m_gimmickAnimation.NotStartFlag != m_isLanding)
        {
            return;
        }
        SetLandingFlag();
    }

    private void SetLandingFlag()
    {
        m_isLanding = m_gimmickAnimation.NotStartFlag;
        if (IsReversFlag == true)
        {
            m_isLanding = !m_gimmickAnimation.NotStartFlag;
        }
        BoxColliderDisplay();
    }

    /// <summary>
    /// ボックスコライダーの表示を切り替える。
    /// </summary>
    private void BoxColliderDisplay()
    {
        for (int i = 0; i < BoxColliders.Length; i++)
        {
            BoxColliders[i].SetActive(m_isLanding);
        }
    }
}
