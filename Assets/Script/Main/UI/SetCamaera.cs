using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamaera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ���g��RenderCamera�Ƀ��C���J������ݒ肷��B
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
