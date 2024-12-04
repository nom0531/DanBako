using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_StageSelect_prototype : MonoBehaviour
{
    [SerializeField, Header("遷移先"),Tooltip("タイトル")]
    private SceneChange Title;
    [SerializeField,Tooltip("ゲーム本編")]
    private SceneChange Main;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    private SE SE_Determination;
    [SerializeField, Tooltip("キャンセル音")]
    private SE SE_Cancel;

    // Update is called once per frame
    void Update()
    {
        // Aボタンを押したとき。
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            Title.CreateFadeCanvas();
            SE_Cancel.PlaySE();
        }
        // Bボタンを押したとき。
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            Main.CreateFadeCanvas();
            SE_Determination.PlaySE();
        }
    }
}
