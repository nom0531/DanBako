using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField, Header("�J�ڐ�"), Tooltip("GameOver")]
    private SceneChange GameOver;
    [SerializeField, Header("HP")]
    private GameObject Content;
    [SerializeField, Tooltip("Image")]
    private GameObject HPImageObject;
    [SerializeField, Tooltip("�摜")]
    private Sprite[] HPSprite;
    [SerializeField, Tooltip("�ő�l")]
    private int HPCount = 3;
    [SerializeField, Tooltip("Animator")]
    private GameObject HPAnimator;
    [SerializeField, Header("SE"), Tooltip("�_���[�W")]
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
        // �f�o�b�O�R�[�h�B
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeHPImage(--hp_test);
        }
    }

    /// <summary>
    /// HP��UI�𐶐��B
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
            // ���g�̔ԍ���������B
            var hp = gameObject.GetComponent<HP>();
            hp.MyID = i;
            hp.SetImage(HPSprite[0], false);
        }
        // HP�̃I�u�W�F�N�g�����X�g���B
        var hpList = FindObjectsOfType<HP>();
        m_hpList = new List<HP>(hpList);
        // �\�[�g�B
        m_hpList.Sort((a, b) => a.MyID.CompareTo(b.MyID));
    }

    /// <summary>
    /// HP�̉摜��ύX����B
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
