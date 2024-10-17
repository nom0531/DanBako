using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�̃V�[���ԍ�")]
    private SceneNumber SceneNumber;
    [SerializeField]
    GameObject FadeCanvas;

    /// <summary>
    /// �t�F�[�h���������s�B
    /// </summary>
    public void CreateFadeCanvas()
    {
        var gameObject = Instantiate(FadeCanvas);
        var fadeScene = gameObject.GetComponent<FadeScene>();

        if (FadeCanvas.name == "FadeCanvas_Rule")
        {
            // �}�e���A�����g�p�����t�F�[�h�B
            fadeScene.FadeStart(SceneNumber, Color.black, false);
        }
        if(FadeCanvas.name == "FadeCanvas_Normal")
        {
            // �ʏ�̃X�v���C�g���g�p�����t�F�[�h�B
            fadeScene.FadeStart(SceneNumber, new Vector4(0,0,0,0), true);
        }
    }
}
