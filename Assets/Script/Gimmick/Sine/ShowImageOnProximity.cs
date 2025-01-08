using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowImageOnProximity : MonoBehaviour
{
    [SerializeField, Header("表示するオブジェクト")]
    private GameObject Canvas;
    [SerializeField,Tooltip("表示する範囲")]
    private float DisplayDistance = 10.0f;
    [SerializeField, Header("生成するテキスト")]
    private GameObject TextMessageCanvas;
    [SerializeField, Header("表示するテキストの番号")]
    private int Number;

    private Player_Main m_player;
    private Transform m_target;
    private TypeWritterEffect m_typeWritterEffect;
    private GameObject m_messageCanvas;
    private Animator m_canvasAnimator;
    private bool m_isShowImage = false;             // Canvasを生成したならtrue。


    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Main>();
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // プレイヤーとターゲットの距離を測定
        float distance = Vector3.Distance(m_target.position, transform.position);

        // 距離が一定範囲内ならImageを表示、範囲外なら非表示
        if (distance <= DisplayDistance)
        {
            Canvas.gameObject.SetActive(true);
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
            {
                ButtonPush();
            }
        }
        else
        {
            Canvas.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ボタンを押したときの処理。
    /// </summary>
    private void ButtonPush()
    {
        if (m_isShowImage == true)
        {
            m_isShowImage = false;
            m_player.MoveFlag = false;
            // Canvasを削除。
            Destroy(m_messageCanvas);
            return;
        }
        m_messageCanvas = Instantiate(TextMessageCanvas);
        // 表示するアニメーションを再生。
        m_canvasAnimator = m_messageCanvas.GetComponent<Animator>();
        m_canvasAnimator.SetTrigger("Active");
        // テキストを表示する。
        m_typeWritterEffect = m_messageCanvas.gameObject.transform.GetChild(0).
            gameObject.transform.GetChild(0).GetComponent<TypeWritterEffect>();
        m_typeWritterEffect.Show(Number);
        m_isShowImage = true;
        m_player.MoveFlag = true;
        return;
    }
}
