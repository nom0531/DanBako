using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour
{
    [SerializeField] float X = 0.0f;
    [SerializeField] float Y = 0.0f;
    [SerializeField] float Z = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(X, Y, Z));
    }
}

