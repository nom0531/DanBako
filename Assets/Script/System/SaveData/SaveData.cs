using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeInfo
{
    public int Hour;
    public int Minute;
    public float Seconds;
}

[System.Serializable]
public class Stage
{
    public bool ClearFlag;                          // クリアしたかどうか。
    public TimeInfo ClearTime;                      // クリアまでの時間。
}

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
        public Stage[] StageData;                   // ステージデータ。
    }

    // 各情報
    public Data saveData;
}