using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_TeamLogo : MonoBehaviour
{
    [SerializeField, Header("�؂�ւ���܂ł̑ҋ@����")]
    private float WaitTime = 5.0f;
    [SerializeField, Header("Animator")]
    private Animator Animator;

    private SceneChange m_sceneChange;
    private float m_timer = 0.0f;
    private bool m_isChange = false;    // �V�[���؂�ւ����Ȃ�true�B

    private void Start()
    {
        m_sceneChange = GetComponent<SceneChange>();
        Animator.SetTrigger("Active");
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_isChange == true)
        {
            return;
        }

        // B�{�^�����������Ƃ��B
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.J))
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
        // ��莞�Ԃ��o�߂����玩���I�ɐ؂�ւ���B
        if (m_timer >= WaitTime)
        {
            m_isChange = true;
            m_timer = 0.0f;
            m_sceneChange.CreateFadeCanvas();
        }
    }
}
