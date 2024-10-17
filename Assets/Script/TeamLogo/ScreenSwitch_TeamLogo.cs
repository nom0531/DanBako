using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_TeamLogo : MonoBehaviour
{
    [SerializeField, Header("�؂�ւ���܂ł̑ҋ@����")]
    private float WaitTime = 5.0f;

    private SceneChange m_sceneChange;
    private float m_timer = 0.0f;
    private bool m_isChange = false;    // �V�[���؂�ւ����Ȃ�true�B

    private void Start()
    {
        m_sceneChange = GetComponent<SceneChange>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_isChange == true)
        {
            return;
        }

        // �{�^���������ꂽ�Ȃ�V�[���؂�ւ������s����B
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.K))
        {
            m_sceneChange.CreateFadeCanvas();
            m_isChange = true;
            return;
        }

        SceneChange();
    }

    /// <summary>
    /// �����I�ɃV�[����؂�ւ��鏈���B
    /// </summary>
    private void SceneChange()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= WaitTime)
        {
            m_sceneChange.CreateFadeCanvas();
            m_isChange = true;
            m_timer = 0.0f;
        }
    }
}
