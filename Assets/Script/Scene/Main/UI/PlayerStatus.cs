using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Player_Main m_player;
    private SetImage m_setImage;

    public SetImage HPImage
    {
        set => m_setImage = value;
    }

    public int HP
    {
        get => m_player.HP;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_player = GetComponent<Player_Main>();
    }

    /// <summary>
    /// ダメージ処理。
    /// </summary>
    public void Damage()
    {
        m_player.TakeDamage();
        m_setImage.ChangeHPImage();
    }
}
