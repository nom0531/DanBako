using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField, Header("ステータス"), Tooltip("HP")]
    private int HP = 3;
    [SerializeField, Tooltip("移動速度")]
    private float MoveSpeed = 1.0f;

    /// <summary>
    /// プレイヤーステータス。
    /// </summary>
    public struct Status
    {
        public int HP;
        public float MoveSpeed;
    }

    private Status m_status;
    private SetImage m_setImage;
    private bool m_isClear = false;     // ゲームをクリアしたらture。

    public Status MyStatus
    {
        get => m_status;
    }

    public bool ClearFlag
    {
        get => m_isClear;
    }

    public SetImage HPImage
    {
        set => m_setImage = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Damage();
        }
    }

    /// <summary>
    /// 初期化処理。
    /// </summary>
    private void Init()
    {
        m_status.HP = HP;
        m_status.MoveSpeed = MoveSpeed;
    }


    /// <summary>
    /// ダメージ処理。
    /// </summary>
    public void Damage()
    {
        if(m_status.HP < 0)
        {
            return;
        }
        --m_status.HP;
        m_setImage.ChangeHPImage();
    }

}
