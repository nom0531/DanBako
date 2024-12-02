using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatus : MonoBehaviour
{
    private int m_ID = 0;
    private bool m_isRotate = false;    // 自身を回転させるならtrue。

    public int MyID
    {
        get => m_ID;
        set => m_ID = value;
    }

    public bool RotateFlag
    {
        set => m_isRotate = value;
    }

    private void Update()
    {
        if(m_isRotate == false)
        {
            return;
        }
        transform.Rotate(0.0f, 0.3f, 0.0f);
    }
}
