using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitch_TeamLogo : MonoBehaviour
{
    [SerializeField, Header("切り替えるまでの待機時間")]
    private float WaitTime = 5.0f;
    [SerializeField, Header("Animator")]
    private Animator Animator;

    private SceneChange m_sceneChange;
    private float m_timer = 0.0f;
    private bool m_isChange = false;    // シーン切り替え中ならtrue。

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

        // Bボタンを押したとき。
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.J))
        {
            m_sceneChange.CreateFadeCanvas();
            m_isChange = true;
            return;
        }
        SceneChange();
    }

    /// <summary>
    /// 自動的にシーンを切り替える処理。
    /// </summary>
    private void SceneChange()
    {
        m_timer += Time.deltaTime;
        // 一定時間が経過したら自動的に切り替える。
        if (m_timer >= WaitTime)
        {
            m_isChange = true;
            m_timer = 0.0f;
            m_sceneChange.CreateFadeCanvas();
        }
    }
}
