using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("遷移先のシーン番号")]
    private SceneNumber SceneNumber;
    [SerializeField]
    GameObject FadeCanvas;

    /// <summary>
    /// フェード処理を実行。
    /// </summary>
    public void CreateFadeCanvas()
    {
        var gameObject = Instantiate(FadeCanvas);
        var fadeScene = gameObject.GetComponent<FadeScene>();

        if (FadeCanvas.name == "FadeCanvas_Rule")
        {
            // マテリアルを使用したフェード。
            fadeScene.FadeStart(SceneNumber, Color.black, false);
        }
        if(FadeCanvas.name == "FadeCanvas_Normal")
        {
            // 通常のスプライトを使用したフェード。
            Color color = new Vector4(0, 0, 0, 0);
            fadeScene.FadeStart(SceneNumber, color, true);
        }
    }
}
