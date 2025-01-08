using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameCamera : MonoBehaviour
{
    [SerializeField, Header("カメラ回転"), Tooltip("カメラの回転速度")]
    private float RotSpeed = 200.0f;
    [SerializeField, Tooltip("カメラの回転モード（X）")]
    private bool CameraModeX = false;
    [SerializeField, Tooltip("X回転最大値")]
    private float MaxX = 60.0f;
    [SerializeField, Tooltip("X回転最小値")]
    private float MinX = -40.0f;
    [SerializeField, Header("カメラ位置"), Tooltip("半径")]
    private float CameraRange = -44.0f;
    [SerializeField, Tooltip("持ち上げる量")]
    private float CameraY_Up = 1.5f;
    [SerializeField, Header("ズームイン・ズームアウト"), Tooltip("最大縮小率")]
    private float ViewMax = 45.0f;
    [SerializeField, Tooltip("最大拡大率")]
    private float ViewMin = 10.0f;

    private const float VIEW_MOVESPEED = 5.0f;

    private GameManager m_gameManager;
    private CinemachineVirtualCamera m_camera;
    private Gamepad m_gamepad;
    private GameObject m_player = null;

    private void Start()
    {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        m_gameManager = GameManager.Instance;
        m_player = GameObject.FindGameObjectWithTag("Player");

        CameraModeX = m_gameManager.SaveDataManager.CameraStete;
    }

    private void LateUpdate()
    {
        // ポーズ中なら実行しない。
        if(m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }
        if(m_gameManager.GameMode == CurrentGameMode.enClear)
        {
            return;
        }
        Rotation();
        Move();
        Zoom();
    }

    /// <summary>
    /// 回転処理。
    /// </summary>
    private void Rotation()
    {
        // X角度制限。
        float nowRot = transform.localEulerAngles.x;
        // 取得した角度は0～360°なので補正する。
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

        // 左右。
        float rot = Time.deltaTime * RotSpeed;

        if (CameraModeX)
        {
            // モードに応じて変更。
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

        // Z軸の回転があるとややこしいので制限する。
        Vector3 angles = transform.eulerAngles;
        angles.z = 0.0f;
        transform.eulerAngles = angles;
    }

    /// <summary>
    /// 移動処理。
    /// </summary>
    private void Move()
    {
        Vector3 position = transform.forward * CameraRange;
        position.y += CameraY_Up;
        // 座標を設定。
        transform.position = m_player.transform.position + position;
    }

    /// <summary>
    /// ズーム処理。
    /// </summary>
    private void Zoom()
    {
        // ゲームパッドを取得。
        m_gamepad = Gamepad.current;

        ZoomIn();
        ZoomOut();
    }

    /// <summary>
    /// 拡大処理。
    /// </summary>
    private void ZoomIn()
    {
        if(m_gamepad == null)
        {
            return;
        }

        float value = m_gamepad.leftTrigger.ReadValue() * VIEW_MOVESPEED;
        float view = m_camera.m_Lens.FieldOfView - value;

        m_camera.m_Lens.FieldOfView = Mathf.Clamp(view, ViewMin, ViewMax);
    }

    /// <summary>
    /// 縮小処理。
    /// </summary>
    private void ZoomOut()
    {
        if (m_gamepad == null)
        {
            return;
        }

        float value = m_gamepad.rightTrigger.ReadValue() * VIEW_MOVESPEED;
        float view = m_camera.m_Lens.FieldOfView + value;

        m_camera.m_Lens.FieldOfView = Mathf.Clamp(view, ViewMin, ViewMax);
    }
}
