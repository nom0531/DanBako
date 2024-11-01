using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField,Header("�ړ���̍��W")]
    private Vector2[] Position;
    [SerializeField, Header("�ړ����x")]
    private float Speed;

    private RectTransform m_rectTransform;
    private Vector2 m_startPosition = Vector2.zero;     // �J�n�_�B
    private Vector2 m_endPosition = Vector2.zero;       // �I���_�B
    private float m_time = 0.0f;                        // �^�C�}�[�B
    private bool m_isStart = false;                     // ���`�⊮���J�n����Ȃ�ture�B

    public Vector2[] SetPosition
    {
        set => Position = value;
        get => Position;
    }

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
        if(m_isStart == false)
        {
            return;
        }

        // �o�ߎ��Ԃ��X�V�B
        m_time += Time.unscaledDeltaTime;
        // �������v�Z�B
        var t = Mathf.Clamp01(m_time / Speed);
        // ��_�Ԃ���`�⊮�B
        m_rectTransform.anchoredPosition = Vector2.Lerp(m_startPosition, m_endPosition, t);

        // ���`�⊮���I�������Ȃ�B
        if (t >= 1.0f)
        {
            m_rectTransform.anchoredPosition = m_endPosition;
            m_isStart = false;
            return;
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
        // ���`�⊮���J�n����B
        m_isStart = true;
    }
}
