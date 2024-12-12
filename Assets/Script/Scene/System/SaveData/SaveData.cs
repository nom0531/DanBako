using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct Data
    {
        public float BGMVolume;                     // BGMの音量。
        public float SEVolume;                      // SEの音量。
        public bool CameraStete;                    // カメラの回転方法。
                                                    // falseならノーマル、trueならリバース。
        public bool[] ClearStage;                   // ステージのクリア数。
        public bool[] DrawStamp;                    // クリアスタンプを描画するかどうか
    }

    // 各情報
    public Data saveData;
}