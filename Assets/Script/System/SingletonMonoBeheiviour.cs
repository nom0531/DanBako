using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var type = typeof(T);
                instance = (T)FindObjectOfType(type);

                if (instance == null)
                {
                    Debug.LogError(type + " ���A�^�b�`���Ă���GameObject�͂���܂���");
                }
            }
            return instance;
        }
    }

    virtual protected void Awake()
    {
        // ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă��邩���ׂ�B
        // �A�^�b�`����Ă���ꍇ�͔j������B
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}