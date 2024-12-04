using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class OptionsMenu_Main : MonoBehaviour
{
    private enum SelectOption
    {
        enBGMOption,
        enSEOption,
        enCameraOption,
        enInitOption
    }

    [SerializeField,Header("BGM Slider")]
    private Slider bgmSlider;                   //bgmスライダー
    [SerializeField]
    private TextMeshProUGUI bgmVolumeText;      //BGMのボリュームを表示するテキスト
    [SerializeField,Header("SE Slider")]
    private Slider seSlider;                    //seスライダー
    [SerializeField]
    private TextMeshProUGUI seVolumeText;       //SEのボリュームを表示するテキスト
    [SerializeField,Header("カメラ設定")]
    private TextMeshProUGUI cameraOption;       //カメラ設定
    [SerializeField,Header("カーソル")]
    private Cursor Cursor;
    [SerializeField]
    private GameObject Panel;
    [SerializeField, Header("SE"), Tooltip("エラー音")]
    private SE SE_Error;
    [SerializeField, Tooltip("カーソル移動音")]
    private SE SE_MoveCursor;
    [SerializeField, Tooltip("決定音")]
    private SE SE_Determination;
    [SerializeField, Tooltip("キャンセル音")]
    private SE SE_Cancel;

    private Gamepad m_gamepad;
    private BGM m_bgm;
    private AnimationEvent m_animationEvent;
    private SelectOption m_comandState = SelectOption.enBGMOption;  //選択中のインデックス
    private int m_cameraIndex = 0;    //カメラ設定のインデックス
    private string[] cameraOptionName = { "ノーマル", "リバース" };
    private bool m_isSelectOption = false;    // 自身が選択されているならture。

    private SaveDataManager m_saveDataManager;

    public bool SelectOptionFlag
    {
        get => m_isSelectOption;
        set => m_isSelectOption = value;
    }

    void Start()
    {
        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        m_bgm.ResetVolume();

        m_animationEvent = Panel.GetComponent<AnimationEvent>();

        //bgmとseのスライダーの値を変更
        bgmSlider.value = m_saveDataManager.SaveData.saveData.BGMVolume;
        seSlider.value = m_saveDataManager.SaveData.saveData.SEVolume;
        ChangeVolumeText();

        //カメラ設定の初期化
        InitCameraOption();
    }

    void Update()
    {
        // 自身が選択されていないなら中断。
        if(SelectOptionFlag == false)
        {
            return;
        }

        if(Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.J))
        {
            SE_Cancel.PlaySE();
            m_animationEvent.OptionFinish();
            return;
        }

        MoveSelect();
        SetParamator();
    }

    /// <summary>
    /// 十字キー上下で移動
    /// </summary>
    void MoveSelect()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PushUp();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushDown();
        }


        m_gamepad = Gamepad.current;

        if(m_gamepad == null)
        {
            return;
        }

        // 十字キーで上下の入力を取得
        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            PushUp();
        }
        else if (Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            PushDown();
        }
    }

    /// <summary>
    /// ↑キーを押したときの処理。
    /// </summary>
    private void PushUp()
    {
        m_comandState--;
        // 補正。
        if (m_comandState < SelectOption.enBGMOption)
        {
            m_comandState = SelectOption.enInitOption;
        }
        Cursor.Move((int)m_comandState);
    }

    /// <summary>
    /// ↓キーを押したときの処理。
    /// </summary>
    private void PushDown()
    {
        m_comandState++;
        // 補正。
        if (m_comandState > SelectOption.enInitOption)
        {
            m_comandState = SelectOption.enBGMOption;
        }
        Cursor.Move((int)m_comandState);
    }

    /// <summary>
    /// 各パラメータの設定。
    /// </summary>
    private void SetParamator()
    {
        // ステートに応じて処理を変更。
        switch (m_comandState)
        {
            case SelectOption.enBGMOption:
                ChangeBGMValue();
                ChangeVolumeText();
                break;
            case SelectOption.enSEOption:
                ChangeSEValue();
                ChangeVolumeText();
                break;
            case SelectOption.enCameraOption:
                ChangeCameraOption();
                break;
            case SelectOption.enInitOption:
                InitGameOption();
                break;
        }
    }

    /// <summary>
    /// カメラ設定の初期化
    /// </summary>
    void InitCameraOption()
    {
        if (m_saveDataManager.CameraStete == false)
        {
            cameraOption.text = cameraOptionName[0];
            return;
        }
        cameraOption.text = cameraOptionName[1];
    }

    /// <summary>
    /// BGMのスライダーの値を変更
    /// </summary>
    void ChangeBGMValue()
    {
        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            if (bgmSlider.value < 1.0f)
            {
                bgmSlider.value += 0.1f;
            }
            //音量を変更
            SetBGMVolume();
            //seを再生
            SE_MoveCursor.PlaySE();
        }
        else if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            if (bgmSlider.value > 0.0f)
            {
                bgmSlider.value -= 0.1f;
            }
            //音量を変更
            SetBGMVolume();
            //seを再生
            SE_MoveCursor.PlaySE();
        }
    }

    /// <summary>
    /// SEのスライダーの値を変更
    /// </summary>
    void ChangeSEValue()
    {
        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            if (seSlider.value < 1.0f)
            {
                seSlider.value += 0.1f;
            }
            //音量を変更
            SetSEVolume();
            //seを再生
            SE_MoveCursor.PlaySE();
        }
        else if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            if (seSlider.value > 0.0f)
            {
                seSlider.value -= 0.1f;
            }
            //音量を変更
            SetSEVolume();
            //seを再生
            SE_MoveCursor.PlaySE();
        }
    }

    /// <summary>
    /// BGMの音量の変更
    /// </summary>
    void SetBGMVolume()
    {
        //セーブデータのbgmのボリューム調整
        m_saveDataManager.BGMVolume = bgmSlider.value;
        m_saveDataManager.Save();
        m_bgm.ResetVolume(m_saveDataManager.BGMVolume);
    }

    /// <summary>
    /// SEの音量の変更
    /// </summary>
    void SetSEVolume()
    {
        m_saveDataManager.SEVolume = seSlider.value;
        m_saveDataManager.Save();
    }

    /// <summary>
    /// BGMとSEのボリュームの表示を変更
    /// </summary>
    void ChangeVolumeText()
    {
        //bgmとseの値をテキストに代入
        float volumeText = bgmSlider.value * 10;
        bgmVolumeText.text = volumeText.ToString("F0");
        float volumeText2 = seSlider.value * 10;
        seVolumeText.text = volumeText2.ToString("F0");
    }

    /// <summary>
    /// カメラ操作の変更
    /// </summary>
    void ChangeCameraOption()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetCameraOption(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetCameraOption(-1);
            return;
        }

        if(m_gamepad == null)
        {
            return;
        }

        if (m_gamepad.dpad.right.wasPressedThisFrame)
        {
            SetCameraOption(1);
            return;
        }
        else if (m_gamepad.dpad.left.wasPressedThisFrame)
        {
            SetCameraOption(-1);
            return;
        }

    }

    /// <summary>
    /// カメラオプションを設定する。
    /// </summary>
    void SetCameraOption(int addValue)
    {
        int oldIndex = m_cameraIndex;
        m_cameraIndex = (m_cameraIndex + addValue + cameraOptionName.Length) % cameraOptionName.Length;

        if (oldIndex == m_cameraIndex)
        {
            SE_Error.PlaySE();
            return;
        }

        cameraOption.text = cameraOptionName[m_cameraIndex];
        //カメラ設定がノーマルの場合
        if (m_cameraIndex == 0)
        {
            m_saveDataManager.CameraStete = false;
            m_saveDataManager.Save();
            return;
        }
        //カメラ設定が別の場合
        m_saveDataManager.CameraStete = true;
        m_saveDataManager.Save();
    }

    /// <summary>
    /// 初期化ボタンを押すと初期設定に変更
    /// </summary>
    void InitGameOption()
    {
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.K))
        {
            //BGMとSEを0.5に設定する
            m_saveDataManager.InitOption();
            //スライダーにも現在の音量の値を入れる
            bgmSlider.value = m_saveDataManager.BGMVolume;
            seSlider.value = m_saveDataManager.SEVolume;
            InitCameraOption();
            SetBGMVolume();
            SetSEVolume();
            ChangeVolumeText();
            SE_Determination.PlaySE();
        }
    }
}