using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScanObject : MonoBehaviour
{
    [SerializeField, Header("スキャン演出")]
    private GameObject ScanningObject;
    [SerializeField, Header("TimeStopFlagを使用するかどうか")]
    private bool IsTimeStopFlag = false;

    private GameStatus m_gameStatus = null;
    private GimmickAnimations m_gimmickAnimations = null;

    private void Start()
    {
        // TimeStopFlagを使用するならば。
        if(IsTimeStopFlag == true)
        {
            // GameStatusのTimeStopFlagを参照する。
            m_gameStatus = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStatus>();
            return;
        }
        // GimmickAnimationsのTimeStopFlagを参照する。
        m_gimmickAnimations = GetComponent<GimmickAnimations>();
    }

    private void Update()
    {
        // LBキーが押されたらアニメーションを再生
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.Y))
        {
            InstantiateObject();
        }
        // RBキーが押されたら巻き戻しを実行
        if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown(KeyCode.Z))
        {
            InstantiateObject();
        }
    }

    /// <summary>
    /// オブジェクトを生成。
    /// </summary>
    private void InstantiateObject()
    {
        if(IsTimeStopFlag == true)
        {
            if (m_gameStatus.TimeStopFlag == false)
            {
                return;
            }
            Instantiate(ScanningObject, gameObject.transform);
            return;
        }
        if (m_gimmickAnimations.TimeStopFlag == false)
        {
            return;
        }
        Instantiate(ScanningObject, gameObject.transform);
    }
}
