using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
/// <summary>
/// �f�o�b�O���O�p�N���X�B�r���h���͎��s����Ȃ��B
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
    /// FPS���擾���鏈���B
    /// </summary>
    /// <returns>FPS�B</returns>
    float FPS()
    {
        return 1f / Time.deltaTime;
    }
}
#endif
