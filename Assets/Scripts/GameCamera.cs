using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("カメラの注視点"), SerializeField]
    GameObject TargetObject;
    [Header("カメラの回転速度"), SerializeField]
    float RotSpeed = 200.0f;
    [Header("カメラの回転モード（X）"), SerializeField]
    bool CameraModeX = false;
    [Header("カメラの回転モード（Y）"), SerializeField]
    bool CameraModeY = false;


    [Header("X回転最大値"), SerializeField]
    float MaxX = 60.0f;
    [Header("X回転最小値"), SerializeField]
    float MinX = -40.0f;


    // 初期座標
    Vector3 m_defPos;


    void Awake()
    {
        // 初期ローカル座標を保存
        m_defPos = transform.localPosition;
    }


    void Update()
    {

    }


    void LateUpdate()
    {
        // 右スティックでカメラ回転


        // 上下
        float rot = Time.deltaTime * RotSpeed;
        if (Input.GetAxisRaw("Vertical2") != 0.0f)
        {
            rot *= Input.GetAxisRaw("Vertical2");

            if (CameraModeY)
            {
                // モードに応じて変更
                rot *= -1.0f;
            }
        }
        else
        {
            rot = 0.0f;
        }
        transform.RotateAround(transform.position, transform.right, rot);


        // X角度制限
        float nowRot = transform.localEulerAngles.x;
        // 取得した角度は0～360°なので補正する
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


        // 左右
        rot = Time.deltaTime * RotSpeed;

        if (CameraModeX)
        {
            // モードに応じて変更
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
        transform.RotateAround(TargetObject.transform.position, Vector3.up, rot);

        // Z軸の回転があるとややこしいので制限する
        Vector3 angles = transform.eulerAngles;
        angles.z = 0.0f;
        transform.eulerAngles = angles;

        //// 座標は固定
        //transform.localPosition = m_defPos;
    }

}
