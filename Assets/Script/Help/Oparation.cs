using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Oparation : MonoBehaviour
{
    [SerializeField, Header("ページ")]
    private GameObject Content;
    [SerializeField, Tooltip("現在位置を示すオブジェクト")]
    private GameObject CurrentLocationObject;
    [SerializeField, Header("SE"), Tooltip("カーソル移動音")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("エラー音")]
    private SE SE_Error;

    /// <summary>
    /// 選択中のコマンド。
    /// </summary>
    private enum OparationState
    {
        enFront,
        enBehind,
        enNum
    }

    private const float SCALE = 0.2f;

    private Gamepad m_gamepad;
    private Animator[] m_animator;
    private List<CurrentLocation> m_currentLocationList;
    private OparationState m_comandState = OparationState.enFront;

    // Start is called before the first frame update
    void Start()
    {
        CreateCurrentLocationObject();
    }

    /// <summary>
    /// ページ数分オブジェクトを生成する。
    /// </summary>
    private void CreateCurrentLocationObject()
    {
        for (int i = 0; i < (int)OparationState.enNum; i++)
        {
            var gameObject = Instantiate(CurrentLocationObject);
            gameObject.transform.SetParent(Content.transform);
            gameObject.transform.localScale = new Vector3(SCALE, SCALE, 0.0f);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            // 自身の番号を教える。
            var currentLocation = gameObject.GetComponent<CurrentLocation>();
            currentLocation.MyID = i;
            if(i != 0)
            {
                currentLocation.PlayAnimaton("NotActive");
            }
        }
        // HPのオブジェクトをリスト化。
        var currentLocations = FindObjectsOfType<CurrentLocation>();
        m_currentLocationList = new List<CurrentLocation>(currentLocations);
        // ソート。
        m_currentLocationList.Sort((a, b) => a.MyID.CompareTo(b.MyID));
    }

    // Update is called once per frame
    void Update()
    {
        CursorMove();
    }

    /// <summary>
    /// カーソル移動。
    /// </summary>
    private void CursorMove()
    {
        m_gamepad = Gamepad.current;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PushRinght();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PushLeft();
        }

        // ゲームパッドが接続されていない場合。
        if (m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            PushRinght();
        }
        if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            PushLeft();
        }
    }

    /// <summary>
    /// →キーを押したときの処理。
    /// </summary>
    private void PushRinght()
    {
        var oldComandState = m_comandState;
        m_comandState++;
        // 補正。
        if (m_comandState > OparationState.enBehind)
        {
            SE_Error.PlaySE();
            m_comandState = OparationState.enBehind;
            return;
        }
        m_currentLocationList[(int)oldComandState].PlayAnimaton("NotActive");
        m_currentLocationList[(int)m_comandState].PlayAnimaton("Active");
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// ←キーを押したときの処理。
    /// </summary>
    private void PushLeft()
    {
        var oldComandState = m_comandState;
        m_comandState--;
        // 補正。
        if (m_comandState < OparationState.enFront)
        {
            SE_Error.PlaySE();
            m_comandState = OparationState.enFront;
            return;
        }
        m_currentLocationList[(int)oldComandState].PlayAnimaton("NotActive");
        m_currentLocationList[(int)m_comandState].PlayAnimaton("Active");
        SE_CursorMove.PlaySE();
    }

    /// <summary>
    /// アニメーションの再生処理。
    /// </summary>
    /// <param name="triggerName">トリガーの名前。</param>
    /// <param name="oparationState">ステート。</param>
    private void PlayAnimation(string triggerName, OparationState oparationState)
    {
        m_animator[(int)oparationState].SetTrigger(triggerName);
    }
}
