using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImage : MonoBehaviour
{
    [SerializeField, Header("HP")]
    private GameObject Content;
    [SerializeField, Tooltip("Image")]
    private GameObject HPImageObject;
    [SerializeField, Tooltip("画像")]
    private Sprite[] HPSprite;
    [SerializeField, Tooltip("Animator")]
    private GameObject HPAnimator;
    [SerializeField, Header("SE"), Tooltip("ダメージ量")]
    private SE SE_Damage;

    private const float SCALE = 0.5f;

    private ScreenSwitch_Main m_screenSwitch;
    private Animator m_animator;
    private PlayerStatus m_playerStatus;
    private List<HP> m_hpList;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = HPAnimator.GetComponent<Animator>();
        m_screenSwitch = GetComponent<ScreenSwitch_Main>();
        m_playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        m_playerStatus.HPImage = this;
        CreateHPUI();
    }

    /// <summary>
    /// HPのUIを生成。
    /// </summary>
    private void CreateHPUI()
    {
        for (int i = 0; i < m_playerStatus.MyStatus.HP; i++)
        {
            var gameObject = Instantiate(HPImageObject);
            gameObject.transform.SetParent(Content.transform);
            gameObject.transform.localScale = new Vector3(SCALE, SCALE, 0.0f);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            // 自身の番号を教える。
            var hp = gameObject.GetComponent<HP>();
            hp.MyID = i;
            hp.SetImage(HPSprite[0], false);
        }
        // HPのオブジェクトをリスト化。
        var hpList = FindObjectsOfType<HP>();
        m_hpList = new List<HP>(hpList);
        // ソート。
        m_hpList.Sort((a, b) => a.MyID.CompareTo(b.MyID));
    }

    /// <summary>
    /// HPの画像を変更する。
    /// </summary>
    public void ChangeHPImage()
    {
        SE_Damage.PlaySE();
        m_animator.SetTrigger("Break");
        m_hpList[m_playerStatus.MyStatus.HP].SetImage(HPSprite[1], true);
        // 体力が0以下なら。
        if (m_playerStatus.MyStatus.HP <= 0)
        {
            m_screenSwitch.PlayGameOver();
        }
    }
}
