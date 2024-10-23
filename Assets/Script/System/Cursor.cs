using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField,Header("�ړ���̍��W")]
    private Vector3[] Position;
    [SerializeField, Header("�ړ����x")]
    private float Speed;

    private RectTransform m_rectTransform;
    private Vector3 m_startPosition = Vector3.zero; // �J�n�_�B
    private Vector3 m_endPosition = Vector3.zero;   // �I���_�B
    private bool m_isLeap = false;                  // ���`�⊮���J�n�����Ȃ�true�B
    private float m_time = 0.0f;                    // �^�C�}�[�B

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = Position[0];
    }

    private void Update()
    {
        Leap();
    }

    /// <summary>
    /// ���`�⊮�̏����B
    /// </summary>
    private void Leap()
    {
        // �o�ߎ��Ԃ��X�V�B
        m_time += Time.deltaTime;
        // �������v�Z�B
        var t = m_time / Speed;
        // ��_�Ԃ���`�⊮�B
        m_rectTransform.anchoredPosition = Vector3.Lerp(m_startPosition, m_endPosition, t);
        // ���`�⊮���I�������Ȃ�B
        if (t >= 1.0f)
        {
            m_time = Speed;
            m_isLeap = false;
        }
    }

    /// <summary>
    /// �ړ������B
    /// </summary>
    /// <param name="number">�ړ���̔ԍ��B</param>
    public void Move(int number)
    {
        // �l���������B
        m_startPosition = m_rectTransform.anchoredPosition;
        m_endPosition = Position[number];
        m_time = 0.0f;
        // ���`�⊮���J�n�B
        m_isLeap = true;
    }
}
