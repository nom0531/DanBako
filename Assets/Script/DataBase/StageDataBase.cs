using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  �X�e�[�W�̍\���́B
/// </summary>
[System.Serializable]
public class StageData
{
    [SerializeField, Header("���x�����")]
    public int ID;
    public string Name;
    public BGMNumber BGM;
    [SerializeField, Header("Prefab")]
    public GameObject Model;
    [SerializeField, Header("�ڍאݒ�"), Multiline(3)]
    public string Detail;
}

[CreateAssetMenu(fileName = "StageDataBase", menuName = "CreateStageDataBase")]
public class StageDataBase : ScriptableObject
{
    public List<StageData> stageDataList = new List<StageData>();
}

