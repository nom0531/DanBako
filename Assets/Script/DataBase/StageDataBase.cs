using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ステージの構造体。
/// </summary>
[System.Serializable]
public class StageData
{
    [SerializeField, Header("レベル情報")]
    public int ID;
    public string Name;
    [SerializeField, Header("Prefab")]
    public GameObject Model;
}

[CreateAssetMenu(fileName = "StageDataBase", menuName = "CreateStageDataBase")]
public class StageDataBase : ScriptableObject
{
    public List<StageData> stageDataList = new List<StageData>();
}

