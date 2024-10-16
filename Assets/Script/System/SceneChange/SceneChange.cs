using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("遷移先のシーン番号")]
    private SceneNumber SceneNumber;
    [SerializeField]
    GameObject FadeCanvas;

    private bool m_sceneChange = false;

    private void Update()
    {
        if(m_sceneChange == true)
        { 
            return;
        }

        // 決定キーが押されたら
        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Return)))
        {
            CreateFadeCanvas();
        }
    }

    /// <summary>
    /// フェード処理を実行。
    /// </summary>
    private void CreateFadeCanvas()
    {
        var gameObject = Instantiate(FadeCanvas);
        gameObject.GetComponent<FadeScene>().FadeStart(SceneNumber,Color.black,false);
        m_sceneChange = true;
    }
}
