using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�̃V�[���ԍ�")]
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

        // ����L�[�������ꂽ��
        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Return)))
        {
            CreateFadeCanvas();
        }
    }

    /// <summary>
    /// �t�F�[�h���������s�B
    /// </summary>
    private void CreateFadeCanvas()
    {
        var gameObject = Instantiate(FadeCanvas);
        gameObject.GetComponent<FadeScene>().FadeStart(SceneNumber,Color.black,false);
        m_sceneChange = true;
    }
}
