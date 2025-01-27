using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarRotation : MonoBehaviour
{
    [SerializeField, Header("回転速度")]
    private float MoveSpeed = 100.0f;
    [SerializeField, Header("右回転にするかどうか")]
    private bool IsRightRotation = false;

    private float m_isRotationDirection = 1.0f; // 回転方向を指定する変数。
    private float m_angle = 0.0f;

    private void Start()
    {
        if(IsRightRotation == true)
        {
            m_isRotationDirection = -1.0f;  // 反転。
        }
    }

    // Update is called once per frame
    private void Update()
    {
        m_angle = MoveSpeed * Time.unscaledDeltaTime * m_isRotationDirection;
        transform.Rotate(0.0f, 0.0f, m_angle);
    }
}
