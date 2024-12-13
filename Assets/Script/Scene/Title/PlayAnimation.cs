using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Animator;

    /// <summary>
    /// �A�j���[�V�������Đ�����B
    /// </summary>
    public void Play(int number, string triggerName)
    {
        Animator[number].GetComponent<Animator>().SetTrigger(triggerName);
    }
}
