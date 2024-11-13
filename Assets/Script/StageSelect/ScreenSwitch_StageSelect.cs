using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_StageSelect : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�"),Tooltip("�^�C�g��")]
    private SceneChange Title;
    [SerializeField,Tooltip("�Q�[���{��")]
    private SceneChange Main;
    [SerializeField, Header("SE"), Tooltip("���艹")]
    private SE SE_Determination;
    [SerializeField, Tooltip("�L�����Z����")]
    private SE SE_Cancel;
    [SerializeField, Tooltip("�J�[�\���ړ���")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("�G���[��")]
    private SE SE_Error;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // A�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            Title.CreateFadeCanvas();
            SE_Cancel.PlaySE();
        }
        // B�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            Main.CreateFadeCanvas();
            SE_Determination.PlaySE();
        }
    }


}
