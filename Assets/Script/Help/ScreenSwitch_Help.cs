using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_Help : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("タイトル")]
    private SceneChange Title;
    [SerializeField, Header("SE"),Tooltip("キャンセル音")]
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
    }
}
