using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamaera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 自身のRenderCameraにメインカメラを設定する。
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
