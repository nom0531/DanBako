using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct Data
    {
        public float BGMVolume;                     // BGM�̉��ʁB
        public float SEVolume;                      // SE�̉��ʁB
        public bool CameraStete;                    // �J�����̉�]���@�B
                                                    // false�Ȃ�m�[�}���Atrue�Ȃ烊�o�[�X�B
        public bool[] ClearStage;                   // �X�e�[�W�̃N���A���B
    }

    // �e���
    public Data saveData;
}