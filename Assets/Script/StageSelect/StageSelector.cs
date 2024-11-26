using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class StageSelector : MonoBehaviour
{
    /// <summary>
    /// ステージのステート。
    /// </summary>
    public enum StageState
    {
        enStop,
        enRight,
        enLeft = -1
    }
    [SerializeField]
    private GameObject Option;   //オプション画面のオブジェクト
    [SerializeField]
    private GameObject Stage;    //ステージセレクト画面のオブジェクト
    [SerializeField]
    private TextMeshProUGUI stageNameText;   //ステージ名を表示するオブジェクト

    [SerializeField]
    private StageDataBase stageDataBase;                //ステージ

    [SerializeField]
    private GameObject[] StageObjects;                  // ステージオブジェクトの配列
    [SerializeField, Header("移動先の座標")]
    private Vector3[] MovePositions;                    // 移動先のポジション
    [SerializeField, Header("シフト速度")]
    private float m_ShiftMoveSpeed = 5.0f;                // ステージ移動の速度

    [SerializeField]
    private const float SELECTED_SCALE = 30.0f;         // 選択されたステージの拡大率
    [SerializeField]
    private const float DEFAULT_SCALE = 20.0f;          // 非選択ステージのデフォルトスケール

    private StageState m_nextStage = StageState.enStop; // 次に選択するステージのステート
    private int m_forwardStageScale = 0;                // 拡大するステージのインデックス
    private int m_currentIndex = 0;                     // 現在選択されているステージのインデックス
    [SerializeField]
    private bool m_isMoving = false;                    // スライドしているかどうか
    [SerializeField]
    private bool m_allMoved = true;                       //全ての動き終わったかどうか


    private void Start()
    {
        //ステージ配列にミニステージのモデルを設定する
        InitStageObjects();

        if (StageObjects.Length == 0)
        {
            Debug.LogError("ステージが設定されていません！");
            return;
        }

        //ミニステージを出現させる
        SpawnStages();

        // 座標を設定する。
        for (int i = 0; i < StageObjects.Length; i++)
        {
            StageObjects[i].transform.position = MovePositions[i];
        }

        // 最初の選択ステージを強調
        UpdateStageScale();
    }

    private void Update()
    {
        //ステージの名前を表示
        UpdateStageName();

        //左右の十字キーでステージを変更
        //Startボタンでオプションを表示
        SelectStageAndOption();
        
        //ステージの位置を更新
        MoveStage();
        
        // スケールを滑らかに更新
        UpdateStageScale();
    }

    private void SelectStageAndOption()
    {
        if (m_isMoving == false)
        {
            // 左右の矢印キー入力をチェック
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                // 右にシフト。
                ShiftObjects(StageState.enRight);
            }
            else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                // 左にシフト。
                ShiftObjects(StageState.enLeft);
            }
            else if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                //オプション画面を開く
                OpenOption();
            }
            else
            {
                //上記以外なら
                m_isMoving = true;
            }
        }
    }


    /// <summary>
    /// 選択中のステージ名を表示
    /// </summary>
    private void UpdateStageName()
    {
        stageNameText.text = stageDataBase.stageDataList[m_currentIndex].Name;
    }

    /// <summary>
    /// データベースに設定してるミニステージをステージ配列に入れる
    /// </summary>
    private void InitStageObjects()
    {
        for(int i = 0; i < StageObjects.Length; i++)
        {
            StageObjects[i] = stageDataBase.stageDataList[i].Model;
        }
    }

    /// <summary>
    /// ステージ配列にあるミニステージを出現させる
    /// </summary>
    private void SpawnStages()
    {
        for(int i = 0; i < StageObjects.Length; i++)
        {
             StageObjects[i] = Instantiate(StageObjects[i], MovePositions[i], Quaternion.identity);
        }
    }

    /// <summary>
    /// シフト処理。
    /// </summary>
    /// <param name="stageState">次に選択するステージの方向。</param>
    private void ShiftObjects(StageState stageState)
    {
        if (m_isMoving) return; // すでに移動中なら処理を終了

        m_isMoving = true; // 移動を開始したらすぐにフラグを設定

        m_nextStage = stageState; // MoveStage等の後続の関数の為に値を代入。

        // 新しい配列を作成
        GameObject[] shiftedObjects = new GameObject[StageObjects.Length];

        // シフト処理
        for (int i = 0; i < StageObjects.Length; i++)
        {
            ShiftStage(i,shiftedObjects);
        }

        // オリジナルの配列を新しい配列で置き換え
        StageObjects = shiftedObjects;
    }

    /// <summary>
    /// 最後の要素を先頭に、他を1つずつずらす処理。
    /// </summary>
    private void ShiftStage(int stageNumber,GameObject[] gameObjects)
    {

        if (m_nextStage == StageState.enRight)
        {
            gameObjects[stageNumber] = StageObjects[(stageNumber + 1 + StageObjects.Length) % StageObjects.Length];
        }
        else if (m_nextStage == StageState.enLeft)
        {
            gameObjects[stageNumber] = StageObjects[(stageNumber - 1 + StageObjects.Length) % StageObjects.Length];
        }
    }

    /// <summary>
    /// ステージを動かす処理。
    /// </summary>
    private void MoveStage()
    {
        if (m_isMoving == true)
        {
            for (int i = 0; i < StageObjects.Length; i++)
            {
                int nextStage = i;
                // ステージを動かす。
                if (m_nextStage == StageState.enRight)
                {
                    nextStage = (i + (int)m_nextStage + StageObjects.Length) % StageObjects.Length;
                }
                else if (m_nextStage == StageState.enLeft)
                {
                    nextStage = (i + (int)m_nextStage + StageObjects.Length) % StageObjects.Length;
                }

                StageObjects[i].transform.position = Vector3.Lerp(StageObjects[i].transform.position, MovePositions[nextStage], Time.deltaTime * m_ShiftMoveSpeed);

                if (Vector3.Distance(StageObjects[i].transform.position, MovePositions[nextStage]) > 5.0f)
                {
                    m_allMoved = false;
                }
                else
                {
                    m_allMoved = true;
                }
            }

            if (m_allMoved)
            {
                UpdateIndex();
                m_isMoving = false;
                m_nextStage = StageState.enStop;
            }
        }
    }

    /// <summary>
    /// ステージのスケールをいじる関数。
    /// </summary>
    private void UpdateStageScale()
    {
        // スケールをデフォルトの値で初期化。
        Vector3 targetScale = Vector3.one * DEFAULT_SCALE;

        for (int i = 0; i < StageObjects.Length; i++)
        {
            if (i == m_forwardStageScale)
            {
                // 選択しているオブジェクトのスケール。
                targetScale = Vector3.one * SELECTED_SCALE;
            }
            else
            {
                targetScale = Vector3.one * DEFAULT_SCALE;
            }
            StageObjects[i].transform.localScale =
                Vector3.Lerp(StageObjects[i].transform.localScale, targetScale, Time.deltaTime * m_ShiftMoveSpeed);
        }
    }

    /// <summary>
    /// インデックスを更新。
    /// </summary>
    private void UpdateIndex()
    {
        if (m_nextStage == StageState.enRight)
        {
            m_currentIndex = (m_currentIndex + 1 + StageObjects.Length) % StageObjects.Length;

        }
        else if (m_nextStage == StageState.enLeft)
        {
            m_currentIndex = (m_currentIndex - 1 + StageObjects.Length) % StageObjects.Length;
        }
        
    }
    
    /// <summary>
    /// ステージ決定
    /// </summary>
    void StageDecition()
    {
        
    }

    /// <summary>
    /// オプション画面を開く
    /// ステージセレクト画面を閉じる
    /// </summary>
    public void OpenOption()
    {
        Option.SetActive(true);
        Stage.SetActive(false);
    }
}
