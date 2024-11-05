using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Animator;

    /// <summary>
    /// アニメーションを再生する。
    /// </summary>
    public void Play(int number, string triggerName)
    {
        Animator[number].GetComponent<Animator>().SetTrigger(triggerName);
    }
}
