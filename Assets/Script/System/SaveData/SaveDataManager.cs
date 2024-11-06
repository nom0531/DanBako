using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
{
    [SerializeField, Header("�Z�[�u�f�[�^")]
    private SaveData GameSaveData;
    [SerializeField, Header("�X�e�[�W�f�[�^")]
    private StageDataBase StageData;

    private const float DEFAULT_VOLUME = 0.5f;

    private string m_filePath = "";  // �������ݐ�̃t�@�C���p�X�B

    private static readonly string m_encryptKey = "c6eahbq9sjuawhvdr9kvhpsm5qv393ga";
    private static readonly int m_encryptPasswordCount = 16;
    private static readonly string m_passwordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly int m_passwordCharsLength = m_passwordChars.Length;

    public SaveData SaveData
    {
        get => GameSaveData;
    }

    protected override void Awake()
    {
        base.Awake();
        // �Z�[�u�f�[�^��ǂݍ��ށB
        m_filePath = $"{Application.persistentDataPath}/.savedata.json";
        var isLoad = Load();
        // �Z�[�u�f�[�^���Ȃ��Ȃ珉�����B
        if (isLoad == false)
        {
            InitOption();
            InitData();
        }
    }

    /// <summary>
    /// �Z�[�u�f�[�^���폜����B
    /// </summary>
    public void Delete()
    {
        InitData();
#if UNITY_EDITOR
        Debug.Log("�f�[�^���폜�B\n" +
            "�ۑ��ꏊ�F" + m_filePath);
#endif
    }

    /// <summary>
    /// ���݂̏󋵂��Z�[�u����B
    /// </summary>
    public void Save()
    {
        // �Í����B
        var json = JsonUtility.ToJson(GameSaveData);
        var iv = "";
        var base64 = "";
        EncryptAesBase64(json, out iv, out base64);
        // �ۑ��B
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
        Debug.Log("�Z�[�u�����B\n" +
            "�ۑ��ꏊ�F" + m_filePath);
#endif
    }

    /// <summary>
    /// ���݂̏󋵂����[�h����B
    /// </summary>
    /// <returns>����������true�A���s������false��Ԃ��B</returns>
    private bool Load()
    {
        if (File.Exists(m_filePath) == false)
        {
            // �Z�[�u�f�[�^��������Ȃ������̂�false��Ԃ��B
            return false;
        }
        // �ǂݍ��݁B
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
        // �������B
        string json;
        string iv = Encoding.UTF8.GetString(ivBytes);
        string base64 = Encoding.UTF8.GetString(base64Bytes);
        DecryptAesBase64(base64, iv, out json);

        // �Z�[�u�f�[�^�����B
        GameSaveData = JsonUtility.FromJson<SaveData>(json);
        return true;
    }

    /// <summary>
    /// �I�v�V�����̏������B
    /// </summary>
    public void InitOption()
    {
        // �l���������B
        GameSaveData.saveData.BGMVolume = DEFAULT_VOLUME;
        GameSaveData.saveData.SEVolume = DEFAULT_VOLUME;
        GameSaveData.saveData.CameraStete = false;
        Save();
#if UNITY_EDITOR
        Debug.Log("�f�[�^���������B\n" +
            "�ۑ��ꏊ�F" + m_filePath);
#endif
    }

    /// <summary>
    /// �f�[�^�̏������B
    /// </summary>
    private void InitData()
    {
        // �f�[�^��p�ӂ���B
        GameSaveData.saveData.ClearStage = new bool[StageData.stageDataList.Count];
        // �X�e�[�W���N���A���Ă��邩�ǂ����B
        for (int stageNumber = 0; stageNumber < StageData.stageDataList.Count; stageNumber++)
        {
            GameSaveData.saveData.ClearStage[stageNumber] = false;  // �N���A���Ă��Ȃ��̂�false�B
        }

        Save();
    }

    /// <summary>
    /// AES�Í���(Base64�`��)�B
    /// </summary>
    public static void EncryptAesBase64(string json, out string iv, out string base64)
    {
        byte[] src = Encoding.UTF8.GetBytes(json);
        byte[] dst;
        EncryptAes(src, out iv, out dst);
        base64 = Convert.ToBase64String(dst);
    }

    /// <summary>
    /// AES������(Base64�`��)�B
    /// </summary>
    public static void DecryptAesBase64(string base64, string iv, out string json)
    {
        byte[] src = Convert.FromBase64String(base64);
        byte[] dst;
        DecryptAes(src, iv, out dst);
        json = Encoding.UTF8.GetString(dst).Trim('\0');
    }

    /// <summary>
    /// AES�Í����B
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
    /// AES�������B
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
    /// �p�X���[�h�����B
    /// </summary>
    /// <param name="count">�����񐔁B</param>
    /// <returns>�p�X���[�h�B</returns>
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
