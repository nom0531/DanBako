using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Help : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�"), Tooltip("�^�C�g��")]
    private SceneChange Title;
    [SerializeField, Header("SE"),Tooltip("�L�����Z����")]
    private SE SE_Cancel;

    // Update is called once per frame
    void Update()
    {
        // A�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            Title.CreateFadeCanvas();
            SE_Cancel.PlaySE();
        }
    }
}
