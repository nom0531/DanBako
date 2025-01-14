using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeVCam : MonoBehaviour
{
    [SerializeField, Header("仮想カメラ")]
    protected CinemachineVirtualCameraBase[] Vcam_Stanging;

    const int VCAM_PRIORITY = 10;

    private void Start()
    {
        // 初期化。
        ResetPriority();
        ChangeVcam(0);
    }

    /// <summary>
    /// 優先度を初期化
    /// </summary>
    protected void ResetPriority()
    {
        for (int i = 0; i < Vcam_Stanging.Length; i++)
        {
            Vcam_Stanging[i].Priority = 0;
        }
    }

    /// <summary>
    /// MainCameraに設定する仮想カメラを切り替える。
    /// </summary>
    protected void ChangeVcam(int number)
    {
        // 優先度を設定する
        Vcam_Stanging[number].Priority = VCAM_PRIORITY;
    }

    /// <summary>
    /// イベント用に座標を設定する。
    /// </summary>
    public void SetEventPosition(Transform transform, int number)
    {
        Vcam_Stanging[number].gameObject.transform.position = transform.position;
        Vcam_Stanging[number].gameObject.transform.rotation = transform.rotation;
    }
}
