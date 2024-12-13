using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeVCam : MonoBehaviour
{
    [SerializeField, Header("仮想カメラ")]
    private CinemachineVirtualCameraBase[] Vcam_Stanging;
    [SerializeField, Header("StageClear用のTransform")]
    private Transform VcamTransform;

    const int VCAM_PRIORITY = 10;

    private void Start()
    {
        // 初期化。
        ResetPriority();
        ChangeVcam(0);
        SetEventPosition();
    }

    private void FixedUpdate()
    {
        SetEventPosition();
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
    private void SetEventPosition()
    {
        Vcam_Stanging[1].gameObject.transform.position = VcamTransform.transform.position;
        Vcam_Stanging[1].gameObject.transform.rotation = VcamTransform.transform.rotation;
    }
}
