using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_Text versionText;

    void Start()
    {
        // アプリケーションのバージョン番号を取得してテキストに設定
        versionText = GetComponent<TextMeshProUGUI>();
        versionText.text = "ver. " + Application.version;
    }
}
