using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
{
    [SerializeField, Header("セーブデータ")]
    private SaveData GameSaveData;
    [SerializeField, Header("ステージデータ")]
    private StageDataBase StageData;

    private const float DEFAULT_VOLUME = 0.7f;

    private string m_filePath = "";  // 書き込み先のファイルパス。

    private static readonly string m_encryptKey = "c6eahbq9sjuawhvdr9kvhpsm5qv393ga";
    private static readonly int m_encryptPasswordCount = 16;
    private static readonly string m_passwordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly int m_passwordCharsLength = m_passwordChars.Length;

    public SaveData SaveData
    {
        get => GameSaveData;
    }

    public float BGMVolume
    {
        get => GameSaveData.saveData.BGMVolume;
        set => GameSaveData.saveData.BGMVolume = Mathf.Clamp(value, 0.0f, 1.0f);

    }

    public float SEVolume
    {
        get => GameSaveData.saveData.SEVolume;
        set => GameSaveData.saveData.SEVolume = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    public bool CameraStete
    {
        get => GameSaveData.saveData.CameraStete;
        set => GameSaveData.saveData.CameraStete = value;
    }

    protected override void Awake()
    {
        base.Awake();
        // セーブデータを読み込む。
        m_filePath = $"{Application.persistentDataPath}/.savedata.json";
        var isLoad = Load();
        // セーブデータがないなら初期化。
        if (isLoad == false)
        {
            InitOption();
            InitData();
        }
    }

    /// <summary>
    /// セーブデータを削除する。
    /// </summary>
    public void Delete()
    {
        InitData();
#if UNITY_EDITOR
        Debug.Log("データを削除。\n" +
            "保存場所：" + m_filePath);
#endif
    }

    /// <summary>
    /// 現在の状況をセーブする。
    /// </summary>
    public void Save()
    {
        // 暗号化。
        var json = JsonUtility.ToJson(GameSaveData);
        var iv = "";
        var base64 = "";
        EncryptAesBase64(json, out iv, out base64);
        // 保存。
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
        using (FileStream fs = new FileStream(m_filePath, FileMode.Create, FileAccess.Write))
        using (BinaryWriter bw = new BinaryWriter(fs))
        {
            bw.Write(ivBytes.Length);
            bw.Write(ivBytes);
            bw.Write(base64Bytes.Length);
            bw.Write(base64Bytes);
            bw.Close();
        }
#if UNITY_EDITOR
        Debug.Log("セーブ完了。\n" +
            "保存場所：" + m_filePath);
#endif
    }

    /// <summary>
    /// 現在の状況をロードする。
    /// </summary>
    /// <returns>成功したらtrue、失敗したらfalseを返す。</returns>
    private bool Load()
    {
        if (File.Exists(m_filePath) == false)
        {
            // セーブデータが見つからなかったのでfalseを返す。
            return false;
        }
        // 読み込み。
        byte[] ivBytes = null;
        byte[] base64Bytes = null;
        using (FileStream fs = new FileStream(m_filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader br = new BinaryReader(fs))
        {
            int length = br.ReadInt32();
            ivBytes = br.ReadBytes(length);

            length = br.ReadInt32();
            base64Bytes = br.ReadBytes(length);
        }
        // 複合化。
        string json;
        string iv = Encoding.UTF8.GetString(ivBytes);
        string base64 = Encoding.UTF8.GetString(base64Bytes);
        DecryptAesBase64(base64, iv, out json);

        // セーブデータ復元。
        GameSaveData = JsonUtility.FromJson<SaveData>(json);
        return true;
    }

    /// <summary>
    /// オプションの初期化。
    /// </summary>
    public void InitOption()
    {
        // 値を初期化。
        GameSaveData.saveData.BGMVolume = DEFAULT_VOLUME;
        GameSaveData.saveData.SEVolume = DEFAULT_VOLUME;
        GameSaveData.saveData.CameraStete = false;
        Save();
#if UNITY_EDITOR
        Debug.Log("データを初期化。\n" +
            "保存場所：" + m_filePath);
#endif
    }

    /// <summary>
    /// データの初期化。
    /// </summary>
    private void InitData()
    {
        // データを用意する。
        GameSaveData.saveData.ClearStage = new bool[StageData.stageDataList.Count];
        GameSaveData.saveData.DrawStamp = new bool[StageData.stageDataList.Count];
        // データの初期化。
        for (int stageNumber = 0; stageNumber < StageData.stageDataList.Count; stageNumber++)
        {
            GameSaveData.saveData.ClearStage[stageNumber] = false;  // クリアしていないのでfalse。
            GameSaveData.saveData.DrawStamp[stageNumber] = false;
        }

        Save();
    }

    /// <summary>
    /// AES暗号化(Base64形式)。
    /// </summary>
    public static void EncryptAesBase64(string json, out string iv, out string base64)
    {
        byte[] src = Encoding.UTF8.GetBytes(json);
        byte[] dst;
        EncryptAes(src, out iv, out dst);
        base64 = Convert.ToBase64String(dst);
    }

    /// <summary>
    /// AES複合化(Base64形式)。
    /// </summary>
    public static void DecryptAesBase64(string base64, string iv, out string json)
    {
        byte[] src = Convert.FromBase64String(base64);
        byte[] dst;
        DecryptAes(src, iv, out dst);
        json = Encoding.UTF8.GetString(dst).Trim('\0');
    }

    /// <summary>
    /// AES暗号化。
    /// </summary>
    public static void EncryptAes(byte[] src, out string iv, out byte[] dst)
    {
        iv = CreatePassword(m_encryptPasswordCount);
        dst = null;
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.Padding = PaddingMode.PKCS7;
            rijndael.Mode = CipherMode.CBC;
            rijndael.KeySize = 256;
            rijndael.BlockSize = 128;

            byte[] key = Encoding.UTF8.GetBytes(m_encryptKey);
            byte[] vec = Encoding.UTF8.GetBytes(iv);

            using (ICryptoTransform encryptor = rijndael.CreateEncryptor(key, vec))
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(src, 0, src.Length);
                cs.FlushFinalBlock();
                dst = ms.ToArray();
            }
        }
    }

    /// <summary>
    /// AES複合化。
    /// </summary>
    public static void DecryptAes(byte[] src, string iv, out byte[] dst)
    {
        dst = new byte[src.Length];
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.Padding = PaddingMode.PKCS7;
            rijndael.Mode = CipherMode.CBC;
            rijndael.KeySize = 256;
            rijndael.BlockSize = 128;

            byte[] key = Encoding.UTF8.GetBytes(m_encryptKey);
            byte[] vec = Encoding.UTF8.GetBytes(iv);

            using (ICryptoTransform decryptor = rijndael.CreateDecryptor(key, vec))
            using (MemoryStream ms = new MemoryStream(src))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                cs.Read(dst, 0, dst.Length);
            }
        }
    }

    /// <summary>
    /// パスワード生成。
    /// </summary>
    /// <param name="count">文字列数。</param>
    /// <returns>パスワード。</returns>
    public static string CreatePassword(int count)
    {
        StringBuilder sb = new StringBuilder(count);
        for (int i = count - 1; i >= 0; i--)
        {
            char c = m_passwordChars[UnityEngine.Random.Range(0, m_passwordCharsLength)];
            sb.Append(c);
        }
        return sb.ToString();
    }
}
