using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    private Camera m_targetCamera; // 常に向くカメラ

    private void Awake()
    {
        m_targetCamera = Camera.main;
    }

    private void Update()
    {
        // ターゲットカメラの方向にUIを向ける
        if (m_targetCamera != null)
        {
            transform.LookAt(transform.position + m_targetCamera.transform.forward);
        }
    }
}
