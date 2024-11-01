using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Help : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("タイトル")]
    private SceneChange Title;
    [SerializeField, Header("SE"), Tooltip("決定音")]
    SE SE_Determination;
    [SerializeField, Tooltip("キャンセル音")]
    SE SE_Cancel;
    [SerializeField, Tooltip("カーソル移動音")]
    SE SE_CursorMove;
    [SerializeField, Tooltip("エラー音")]
    SE SE_Error;

    // Update is called once per frame
    void Update()
    {
        // Aボタンを押したとき。
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            Title.CreateFadeCanvas();
            SE_Cancel.PlaySE();
        }
    }
}
