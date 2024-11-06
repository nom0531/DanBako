using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamera : MonoBehaviour
{
    [SerializeField, Header("�J������]"), Tooltip("�J�����̉�]���x")]
    private float RotSpeed = 200.0f;
    [SerializeField, Tooltip("�J�����̉�]���[�h�iX�j")]
    private bool CameraModeX = false;
    [SerializeField, Tooltip("X��]�ő�l")]
    private float MaxX = 60.0f;
    [SerializeField, Tooltip("X��]�ŏ��l")]
    private float MinX = -40.0f;
    [SerializeField, Header("�J�����ʒu"), Tooltip("���a")]
    private float CameraRange = -44.0f;
    [SerializeField, Tooltip("�����グ���")]
    private float CameraY_Up = 1.5f;
    [SerializeField, Header("�Y�[���C���E�Y�[���A�E�g"), Tooltip("�g�嗦�ő�l")]
    private float ViewMax = 45.0f;
    [SerializeField, Tooltip("�g�嗦�ŏ��l")]
    private float ViewMin = 10.0f;

    private GameManager m_gameManager;
    private Camera m_camera;
    private Gamepad m_gamepad;
    private GameObject m_player = null;

    private void Start()
    {
        m_camera = Camera.main;
        m_gameManager = GameManager.Instance;
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        // �|�[�Y���Ȃ���s���Ȃ��B
        if(m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }
        Rotation();
        Move();
        Zoom();
    }

    /// <summary>
    /// ��]�����B
    /// </summary>
    private void Rotation()
    {
        float rot = 0.0f;
        // X�p�x�����B
        float nowRot = transform.localEulerAngles.x;
        // �擾�����p�x��0�`360���Ȃ̂ŕ␳����B
        if (nowRot >= 180.0f)
        {
            nowRot -= 360.0f;
        }
        if (nowRot > MaxX)
        {
            transform.localEulerAngles = new Vector3(
                MaxX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        if (nowRot < MinX)
        {
            transform.localEulerAngles = new Vector3(
                MinX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        // ���E�B
        rot = Time.unscaledDeltaTime * RotSpeed;

        if (CameraModeX)
        {
            // ���[�h�ɉ����ĕύX�B
            rot *= -1.0f;
        }

        if (Input.GetAxisRaw("Horizontal2") != 0.0f)
        {
            rot *= Input.GetAxisRaw("Horizontal2");
        }
        else
        {
            rot = 0.0f;
        }
        transform.RotateAround(m_player.transform.position, Vector3.up, rot);

        // Z���̉�]������Ƃ�₱�����̂Ő�������B
        Vector3 angles = transform.eulerAngles;
        angles.z = 0.0f;
        transform.eulerAngles = angles;
    }

    /// <summary>
    /// �ړ������B
    /// </summary>
    private void Move()
    {
        Vector3 position = transform.forward * CameraRange;
        position.y += CameraY_Up;
        // ���W��ݒ�B
        transform.position = m_player.transform.position + position;
    }

    /// <summary>
    /// �Y�[�������B
    /// </summary>
    private void Zoom()
    {
        // �Q�[���p�b�h���擾�B
        m_gamepad = Gamepad.current;

        ZoomIn();
        ZoomOut();
    }

    /// <summary>
    /// �g�又���B
    /// </summary>
    private void ZoomIn()
    {
        if(m_gamepad == null)
        {
            return;
        }

        float value = m_gamepad.leftTrigger.ReadValue();
        float view = m_camera.fieldOfView - value;

        m_camera.fieldOfView = Mathf.Clamp(view, ViewMin, ViewMax);
    }

    /// <summary>
    /// �k�������B
    /// </summary>
    private void ZoomOut()
    {
        if (m_gamepad == null)
        {
            return;
        }

        float value = m_gamepad.rightTrigger.ReadValue();
        float view = m_camera.fieldOfView + value;

        m_camera.fieldOfView = Mathf.Clamp(view, ViewMin, ViewMax);
    }
}
