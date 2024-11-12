using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField, Header("遷移先"), Tooltip("GameOver")]
    private SceneChange GameOver;
    [SerializeField, Header("HP")]
    private GameObject Content;
    [SerializeField, Tooltip("Image")]
    private GameObject HPImageObject;
    [SerializeField, Tooltip("画像")]
    private Sprite[] HPSprite;
    [SerializeField, Tooltip("最大値")]
    private int HPCount = 3;
    [SerializeField, Tooltip("Animator")]
    private GameObject HPAnimator;
    [SerializeField, Header("SE"), Tooltip("ダメージ")]
    private SE SE_Damage;

    private const float SCALE = 0.5f;

    private Animator m_animator;
    private List<HP> m_hpList;
    private int hp_test = 3;

    // Start is called before the first frame update
    void Start()
    {
        CreateHPUI();
        m_animator = HPAnimator.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // デバッグコード。
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeHPImage(--hp_test);
        }
    }

    /// <summary>
    /// HPのUIを生成。
    /// </summary>
    private void CreateHPUI()
    {
        for (int i = 0; i < HPCount; i++)
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
    private void ChangeHPImage(int hp)
    {
        if (hp_test < 0)
        {
            GameOver.CreateFadeCanvas();
            return;
        }
        SE_Damage.PlaySE();
        m_animator.SetTrigger("Break");
        m_hpList[hp].SetImage(HPSprite[1], true);
    }
}
