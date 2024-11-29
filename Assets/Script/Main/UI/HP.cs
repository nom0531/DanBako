using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    private Image m_image;
    private int m_ID = 0;               // 自身の番号。
    private bool m_isEnpty = false;     // 体力がないならture。

    public int MyID
    {
        get => m_ID;
        set => m_ID = value;
    }

    public bool EnptyFlag
    {
        get => m_isEnpty;
    }

    /// <summary>
    /// 画像を再設定する。
    /// </summary>
    public void SetImage(Sprite sprite, bool flag)
    {
        if(m_image == null)
        {
            m_image = GetComponent<Image>();
        }

        m_image.sprite = sprite;
        m_isEnpty = flag;
    }
}
