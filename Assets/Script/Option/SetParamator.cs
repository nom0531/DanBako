using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SetParamator : MonoBehaviour
{
    [SerializeField, Header("カメラ"),Tooltip("テキスト")]
    private GameObject CameraText;
    [SerializeField, Tooltip("カーソル")]
    private GameObject[] CameraCursor;
    [SerializeField, Header("サウンド"), Tooltip("カーソル")]
    private GameObject[] SoundCursor;
    [SerializeField, Tooltip("テキスト")]
    private GameObject[] SoundText;
    [SerializeField, Tooltip("限度値")]
    private float PositionX_Min, PositionX_Max;
    [SerializeField, Tooltip("段階")]
    private int SoundStage = 10;
    [SerializeField, Header("SE"), Tooltip("カーソル移動音")]
    private SE SE_CursorMove;
    [SerializeField, Tooltip("エラー音")]
    SE SE_Error;

    /// <summary>
    /// サウンドデータ。
    /// </summary>
    struct SoundData
    {
        public RectTransform rectTransform; // 座標。
        public int soundStage;              // 現在の音量。
    }

    /// <summary>
    /// サウンドステート。
    /// </summary>
    private enum SoundState
    {
        enBGM,
        enSE,
        enNum
    }

    private SaveDataManager m_saveDataManager;
    private Gamepad m_gamepad;
    private BGM m_bgm;
    private SoundData[] m_soundDatas = new SoundData[(int)SoundState.enNum];
    private OptionState m_comandState = OptionState.enBGMSound;
    private float m_moveLength = 0.0f;                          // 一度のカーソルの移動量。


    private void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        m_soundDatas[(int)SoundState.enBGM].rectTransform = SoundCursor[(int)SoundState.enBGM].GetComponent<RectTransform>();
        m_soundDatas[(int)SoundState.enSE].rectTransform = SoundCursor[(int)SoundState.enSE].GetComponent<RectTransform>();

        // 移動量を計算。
        m_moveLength = (PositionX_Min - PositionX_Max) / SoundStage;
        Init();
    }

    /// <summary>
    /// 初期化処理。
    /// </summary>
    private void Init()
    {
        ChangeCameraRotation(m_saveDataManager.SaveData.saveData.CameraStete);
        InitSoundVolume();
    }

    /// <summary>
    /// 音量の初期化。
    /// </summary>
    private void InitSoundVolume()
    {
        // ボリュームを計算。
        var bgm = m_saveDataManager.SaveData.saveData.BGMVolume * SoundStage;
        var se = m_saveDataManager.SaveData.saveData.SEVolume * SoundStage;
        // 番号。
        int bgmNumber = (int)SoundState.enBGM;
        int seNumber = (int)SoundState.enSE;

        // カーソル位置を更新。
        CursorMove(bgm, bgmNumber);
        CursorMove(se, seNumber);
        // 現在の段階。（0～SoundStageまで）
        m_soundDatas[bgmNumber].soundStage = (int)bgm;
        m_soundDatas[seNumber].soundStage = (int)se;
        // テキストを更新。
        SoundText[bgmNumber].GetComponent<TextMeshProUGUI>().text = m_soundDatas[bgmNumber].soundStage.ToString();
        SoundText[seNumber].GetComponent<TextMeshProUGUI>().text = m_soundDatas[seNumber].soundStage.ToString();
        // 音量を適用する。
        m_bgm.ResetVolume();
    }

    /// <summary>
    /// カーソルの位置を決定する。
    /// </summary>
    /// <param name="soundStage">音量の段階。</param>
    private void CursorMove(float soundStage, int number)
    {
        // 現在の音量と更新後の音量を比較。
        var diff = (int)soundStage - m_soundDatas[number].soundStage;
        // カーソルの移動量を計算。
        MoveX(m_soundDatas[number].rectTransform, diff * -m_moveLength);
    }

    /// <summary>
    /// パラメータを設定する処理。
    /// </summary>
    public void Set(OptionState comandState, Gamepad gamepad)
    {
        m_comandState = comandState;
        m_gamepad = gamepad;

        SetBGMParamator();
        SetSEParamator();
        SetCameraParamator();
    }

    /// <summary>
    /// BGMのパラメータを設定する処理。
    /// </summary>
    private void SetBGMParamator()
    {
        if (m_comandState != OptionState.enBGMParamator)
        {
            return;
        }
        SetVolume(SoundState.enBGM);
    }

    /// <summary>
    /// SEのパラメータを設定する処理。
    /// </summary>
    private void SetSEParamator()
    {
        if (m_comandState != OptionState.enSEParamator)
        {
            return;
        }
        SetVolume(SoundState.enSE);
    }

    /// <summary>
    /// 音量を設定。
    /// </summary>
    /// <param name="rectTransform">対象オブジェクトの座標。</param>
    private void SetVolume(SoundState soundState)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            VolumeUp((int)soundState);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            VolumeDown((int)soundState);
        }

        // ゲームパッドが接続されていない場合。
        if(m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            VolumeUp((int)soundState);
        }
        if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            VolumeDown((int)soundState);
        }
    }

    /// <summary>
    /// 音量を上げる処理。
    /// </summary>
    private void VolumeUp(int number)
    {
        m_soundDatas[number].soundStage++;
        if (m_soundDatas[number].soundStage > SoundStage)
        {
            m_soundDatas[number].soundStage = SoundStage;
            SE_Error.PlaySE();
            return;
        }        

        MoveX(m_soundDatas[number].rectTransform, -m_moveLength);
        SoundText[number].GetComponent<TextMeshProUGUI>().text = m_soundDatas[number].soundStage.ToString();

        if (m_comandState == OptionState.enSEParamator)
        {
            m_saveDataManager.SaveData.saveData.SEVolume = (float)m_soundDatas[number].soundStage / SoundStage;
        }
        if (m_comandState == OptionState.enBGMParamator)
        {
            m_saveDataManager.SaveData.saveData.BGMVolume = (float)m_soundDatas[number].soundStage / SoundStage;
            m_bgm.ResetVolume();
        }
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// 音量を下げる処理。
    /// </summary>
    private void VolumeDown(int number)
    {
        m_soundDatas[number].soundStage--;
        if (m_soundDatas[number].soundStage < 0)
        {
            m_soundDatas[number].soundStage = 0;
            SE_Error.PlaySE();
            return;
        }

        MoveX(m_soundDatas[number].rectTransform, m_moveLength);
        SoundText[number].GetComponent<TextMeshProUGUI>().text = m_soundDatas[number].soundStage.ToString();

        if (m_comandState == OptionState.enSEParamator)
        {
            m_saveDataManager.SaveData.saveData.SEVolume = (float)m_soundDatas[number].soundStage / SoundStage;
        }
        if (m_comandState == OptionState.enBGMParamator)
        {
            m_saveDataManager.SaveData.saveData.BGMVolume = (float)m_soundDatas[number].soundStage / SoundStage;
            m_bgm.ResetVolume();
        }
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// 移動処理。
    /// </summary>
    /// <param name="MoveLength">移動量。</param>
    /// <param name="rectTransform">対象オブジェクトの座標。</param>
    private void MoveX(RectTransform rectTransform, float MoveLength)
    {
        var length = rectTransform.anchoredPosition.x + MoveLength;
        var x = Mathf.Clamp(length, PositionX_Min, PositionX_Max);
        rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
    }


    /// <summary>
    /// カメラのパラメータを設定する処理。
    /// </summary>
    private void SetCameraParamator()
    {
        if (m_comandState != OptionState.enCameraParamator)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PushRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PushLeft();
        }

        // ゲームパッドが接続されていない場合。
        if(m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            PushRight();
        }
        if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            PushLeft();
        }
    }

    /// <summary>
    /// →キーを押したときの処理。
    /// </summary>
    private void PushRight()
    {
        if (m_saveDataManager.SaveData.saveData.CameraStete == false)
        {
            SE_Error.PlaySE();
            return;
        }
        ChangeCameraRotation(false);
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// ←キーを押したときの処理。
    /// </summary>
    private void PushLeft()
    {
        if (m_saveDataManager.SaveData.saveData.CameraStete == true)
        {
            SE_Error.PlaySE();
            return;
        }
        ChangeCameraRotation(true);
        SE_CursorMove.PlaySE();
        m_saveDataManager.Save();
    }

    /// <summary>
    /// カメラ回転を変更する。
    /// </summary>
    /// <param name="flag">falseならノーマル回転。tureならリバース回転。</param>
    private void ChangeCameraRotation(bool flag)
    {
        var text = "ノーマル";
        int notActivenumber = 0;
        int activeNumber = 1;
        if(flag == true)
        {
            text = "リバース";
            notActivenumber = 1;
            activeNumber = 0;
        }

        CameraCursor[notActivenumber].SetActive(false);
        CameraCursor[activeNumber].SetActive(true);
        CameraText.GetComponent<TextMeshProUGUI>().text = $"{text}";
        m_saveDataManager.SaveData.saveData.CameraStete = flag;
    }

    /// <summary>
    /// データを初期化する。
    /// </summary>
    public void ResetStatus()
    {
        m_saveDataManager.InitOption();
        Init();
        m_saveDataManager.Save();
    }
}
