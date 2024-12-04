using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    [SerializeField]
    private StageDataBase StageData;
    [SerializeField, Header("ステージの生成位置")]
    private Vector3 Position;
    [SerializeField]
    private Quaternion Rotation;
    [SerializeField]
    private Vector3 Scale;

    private BGM m_bgm;
    private GameManager m_gameManager;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Main_UI", LoadSceneMode.Additive);

        m_gameManager = GameManager.Instance;
        // BGMを設定。
        m_bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        m_bgm.SetBGM = StageData.stageDataList[m_gameManager.StageID].BGM;
        // ステージを生成。
        var stageObject = Instantiate(StageData.stageDataList[m_gameManager.StageID].Model);
        stageObject.transform.localPosition = Position;
        stageObject.transform.localRotation = Rotation;
        stageObject.transform.localScale = Scale;
        // 番号を教える。
        var stageStatus = stageObject.GetComponent<StageStatus>();
        stageStatus.MyID = m_gameManager.StageID;
    }
}
