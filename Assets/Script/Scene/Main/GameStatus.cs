using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        SceneManager.LoadScene("Main_UI", LoadSceneMode.Additive);
    }
}
