using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("�J�����̒����_"), SerializeField]
    GameObject TargetObject;
    [Header("�J�����̉�]���x"), SerializeField]
    float RotSpeed = 200.0f;
    [Header("�J�����̉�]���[�h�iX�j"), SerializeField]
    bool CameraModeX = false;
    [Header("�J�����̉�]���[�h�iY�j"), SerializeField]
    bool CameraModeY = false;


    [Header("X��]�ő�l"), SerializeField]
    float MaxX = 60.0f;
    [Header("X��]�ŏ��l"), SerializeField]
    float MinX = -40.0f;


    // �������W
    Vector3 m_defPos;


    void Awake()
    {
        // �������[�J�����W��ۑ�
        m_defPos = transform.localPosition;
    }


    void Update()
    {

    }


    void LateUpdate()
    {
        // �E�X�e�B�b�N�ŃJ������]


        // �㉺
        float rot = Time.deltaTime * RotSpeed;
        if (Input.GetAxisRaw("Vertical2") != 0.0f)
        {
            rot *= Input.GetAxisRaw("Vertical2");

            if (CameraModeY)
            {
                // ���[�h�ɉ����ĕύX
                rot *= -1.0f;
            }
        }
        else
        {
            rot = 0.0f;
        }
        transform.RotateAround(transform.position, transform.right, rot);


        // X�p�x����
        float nowRot = transform.localEulerAngles.x;
        // �擾�����p�x��0�`360���Ȃ̂ŕ␳����
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


        // ���E
        rot = Time.deltaTime * RotSpeed;

        if (CameraModeX)
        {
            // ���[�h�ɉ����ĕύX
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

        // Z���̉�]������Ƃ�₱�����̂Ő�������
        Vector3 angles = transform.eulerAngles;
        angles.z = 0.0f;
        transform.eulerAngles = angles;

        //// ���W�͌Œ�
        //transform.localPosition = m_defPos;
    }

}
