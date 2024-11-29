using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatus : MonoBehaviour
{
    private int m_ID = 0;

    public int MyID
    {
        get => m_ID;
        set => m_ID = value;
    }

    private void Update()
    {
        transform.Rotate(0.0f, 0.5f, 0.0f);
    }
}
