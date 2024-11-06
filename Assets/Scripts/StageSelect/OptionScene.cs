using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private SE moveCursorSE;
    [SerializeField]
    private BGM bgm;
    [SerializeField]
    private GameObject Option;       //オプション画面のオブジェクト
    [SerializeField]
    private GameObject Stage;        //ステージセレクト画面のオブジェクト

    [SerializeField]
    private Slider bgmSlider;        //bgmスライダー
    [SerializeField]
    private Slider seSlider;         //seスライダー
    [SerializeField]
    private TextMeshProUGUI cameraOption;      //カメラ設定
    [SerializeField]
    private Image backText;          //戻るボタン
    [SerializeField]
    private Image selectCursor;      //カーソル

    [SerializeField]
    private TextMeshProUGUI bgmVolumeText;  //BGMのボリュームを表示するテキスト
    [SerializeField]
    private TextMeshProUGUI seVolumeText;   //SEのボリュームを表示するテキスト

    private int m_selectedIndex = 0;  //選択中のインデックス
    private GameObject[] menuItems;

    private int m_cameraIndex = 0;    //カメラ設定のインデックス
    private string[] cameraOptionName = { "ノーマル", "たけのこ  " };

    private SaveDataManager m_saveDataManager;
    private SoundManager m_soundManager;

    [SerializeField,Header("スライダー選択時のポジション調整")]
    private Vector3 SliderAdjustmentPosition;   //スライダー選択時のカーソルのポジションの調整に使う

    [SerializeField, Header("カメラ設定選択時のポジション調整")]
    private Vector3 CameraAdjustmentPosition;   //カメラ選択時のカーソルのポジションの調整に使う

    [SerializeField,Header("戻るボタン選択時のポジション調整")]
    private Vector3 ImageAdjustmentPosition;    //戻るボタン選択時のカーソルのポジションの調整に使う

    void Start() 
    {
        // メニュー項目のリストを作成
        menuItems = new GameObject[] { bgmSlider.gameObject, seSlider.gameObject, cameraOption.gameObject ,backText.gameObject };

        m_saveDataManager = GameManager.Instance.SaveDataManager;
        m_soundManager = GameManager.Instance.SoundManager;

        //bgmとseのスライダーの値を変更
        bgmSlider.value = m_saveDataManager.SaveData.saveData.BGMVolume;
        seSlider.value = m_saveDataManager.SaveData.saveData.SEVolume;

        //selectedIndexの初期化
        m_selectedIndex = 0;
    }

    void Update()
    {
        //十字キー上下で移動
        MoveSelect();
        //カーソルの移動
        MoveCursor();
        //十字キー左右でスライダーの値を変更
        ChangeSliderValue();
        //BGMとSEのボリュームの表示を変更
        ChangeVolumeText();
        //十字キー左右でカメラ操作を変更
        ChangeCameraOption();
        //戻るボタンを押すとステージセレクト画面に遷移
        BackToStageSelect();
       
    }

    /// <summary>
    /// 十字キー上下で移動
    /// </summary>
    void MoveSelect()
    {
        // 十字キーで上下の入力を取得
        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            // インデックスを変更（上下移動）
            int direction = -1;
            m_selectedIndex = Mathf.Clamp(m_selectedIndex + direction, 0, menuItems.Length - 1);
            //seを再生
            moveCursorSE.PlaySE();
        }
        else if(Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            int direction = 1;
            m_selectedIndex = Mathf.Clamp(m_selectedIndex + direction, 0, menuItems.Length - 1);
            //seを再生
            moveCursorSE.PlaySE();
        }
    }

    /// <summary>
    /// カーソルの移動
    /// </summary>
    void MoveCursor()
    {
        for(int i = 0;i < menuItems.Length;i++)
        {
            //スライダーが選択されているとき
            if(i == m_selectedIndex && menuItems[m_selectedIndex].TryGetComponent(out Slider selectedSlider))
            {
                selectCursor.transform.position = menuItems[i].transform.position + SliderAdjustmentPosition;
            }
            //カメラ設定が選択されているとき
            if (i == m_selectedIndex && menuItems[m_selectedIndex].TryGetComponent(out TextMeshProUGUI selectedText))
            {
                selectCursor.transform.position = menuItems[i].transform.position + CameraAdjustmentPosition;
            }
            //戻るボタンが選択されているとき
            if (i == m_selectedIndex && menuItems[m_selectedIndex].TryGetComponent(out Image selectedImage))
            {
                selectCursor.transform.position = menuItems[i].transform.position + ImageAdjustmentPosition;
            }
        }
    }

    /// <summary>
    /// 十字キー左右でスライダーの値を変更
    /// </summary>
    void ChangeSliderValue()
    {
        // 選択項目がスライダーなら左右入力で値を調整
        if (menuItems[m_selectedIndex].TryGetComponent(out Slider selectedSlider))
        {
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                selectedSlider.value += 0.1f;
                //音量を変更
                SetBGMVolume();
                //seを再生
                moveCursorSE.PlaySE();
            }
            else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                selectedSlider.value -= 0.1f;
                //音量を変更
                SetBGMVolume();
                //seを再生
                moveCursorSE.PlaySE();
            }
        }
    }

    /// <summary>
    /// 音量の変更
    /// </summary>
    void SetBGMVolume()
    {
        //セーブデータのbgmのボリューム調整
        m_saveDataManager.SaveData.saveData.BGMVolume = bgmSlider.value;
        m_saveDataManager.Save();
        bgm.ResetVolume();
    }

    /// <summary>
    /// BGMとSEのボリュームの表示を変更
    /// </summary>
    void ChangeVolumeText()
    {
        //bgmとseの値をテキストに代入
        bgmVolumeText.text = bgmSlider.value.ToString();
        seVolumeText.text = seSlider.value.ToString();
    }

    /// <summary>
    /// カメラ操作の変更
    /// </summary>
    void ChangeCameraOption()
    {
        //選択項目がカメラ設定だったら左右入力で設定変更
        if(menuItems[m_selectedIndex] == cameraOption.gameObject)
        {
            if(Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                m_cameraIndex = (m_cameraIndex + 1 + cameraOptionName.Length) % cameraOptionName.Length;
            }
            else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                m_cameraIndex = (m_cameraIndex - 1 + cameraOptionName.Length) % cameraOptionName.Length;
            }
        }
        cameraOption.text = cameraOptionName[m_cameraIndex];
    }

    /// <summary>
    /// 戻るボタンを押すとステージセレクト画面に遷移
    /// </summary>
    void BackToStageSelect()
    {
        // 「戻る」ボタンが選択されていてBボタンを押すかStartボタンを押すと戻る処理を実行
        if (menuItems[m_selectedIndex].TryGetComponent(out Image selectedImage) && Gamepad.current.bButton.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame)
        {
            Stage.SetActive(true);
            Option.SetActive(false);
        }
    }
}
