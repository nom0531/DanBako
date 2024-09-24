using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
/// <summary>
/// デバッグログ用クラス。ビルド時は実行されない。
/// </summary>
public class DebugLogs : MonoBehaviour
{
    [SerializeField]
    GameObject DebugText;

    // Update is called once per frame
    void Update()
    {
        DebugText.GetComponent<TextMeshProUGUI>().text =
            $"FPS: {FPS().ToString()}\n" +
            $"TimeScale: {Time.timeScale}\n";
    }

    /// <summary>
    /// FPSを取得する処理。
    /// </summary>
    /// <returns>FPS。</returns>
    float FPS()
    {
        return 1f / Time.deltaTime;
    }
}
#endif
