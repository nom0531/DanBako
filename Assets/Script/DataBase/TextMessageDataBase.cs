using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  表示するテキストの構造体。
/// </summary>
[System.Serializable]
public class TextMessageData
{
    [SerializeField, Header("レベル情報")]
    public int ID;
    public string Name;
    [SerializeField, Header("表示するテキスト"), Multiline(10)]
    public string Detail;
}

[CreateAssetMenu(fileName = "TextMessageDataBase", menuName = "CreateTextMessageDataBase")]
public class TextMessageDataBase : ScriptableObject
{
    public List<TextMessageData> textMessegeDataList = new List<TextMessageData>();
}
