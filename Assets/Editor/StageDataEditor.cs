using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class StageDataEditor : EditorWindow
{
    private const int MAX_TEXTNUM = 14;

    // �Ώۂ̃f�[�^�x�[�X
    private static StageDataBase m_stageDataBase;
    // ���O�ꗗ
    private static List<string> m_nameList = new List<string>();
    // �X�N���[���ʒu
    private Vector2 m_leftScrollPosition = Vector2.zero;
    // �I�𒆃i���o�[
    private int m_selectNumber = -1;
    // ������
    private SearchField m_searchField;
    private string m_searchText = "";

    // �E�B���h�E���쐬
    [MenuItem("Window/StageDataBase Editor")]
    private static void Open()
    {
        // �ǂݍ���
        m_stageDataBase = AssetDatabase.LoadAssetAtPath<StageDataBase>("Assets/Data/StageDataBase.asset");
        // ���O��ύX
        GetWindow<StageDataEditor>("StageDataBase Editor");
        // �ύX��ʒm
        EditorUtility.SetDirty(m_stageDataBase);

        ResetNameList();
    }

    /// <summary>
    /// �E�B���h�E��GUI����
    /// </summary>
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            LeftUpdate();
            NameViewUpdate();
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// �r���[�����̍X�V����
    /// </summary>
    private void LeftUpdate()
    {
        // �T�C�Y�𒲐�
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(160), GUILayout.Height(400));
        {
            // ������
            m_searchField ??= new SearchField();
            GUILayout.Label("���O����");
            m_searchText = m_searchField.OnToolbarGUI(m_searchText);

            Search();

            m_leftScrollPosition = EditorGUILayout.BeginScrollView(m_leftScrollPosition, GUI.skin.box);
            {
                // �f�[�^���X�g
                for (int i = 0; i < m_nameList.Count; i++)
                {
                    // �F�ύX
                    if (m_selectNumber == i)
                    {
                        GUI.backgroundColor = Color.cyan;
                    }
                    else
                    {
                        GUI.backgroundColor = Color.white;
                    }

                    // �{�^���������ꂽ���̏���
                    if (GUILayout.Button($"{i}:{m_nameList[i]}"))
                    {
                        // �ΏەύX
                        m_selectNumber = i;
                        GUI.FocusControl("");
                        Repaint();
                    }
                }
                // �F��߂�
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();

            // ���ڑ���{�^��
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("�ǉ�", EditorStyles.miniButtonLeft))
                {
                    AddData();
                }
                if (GUILayout.Button("�폜", EditorStyles.miniButtonRight))
                {
                    DeleteData();
                }
            }
            EditorGUILayout.EndHorizontal();

            // ���ڐ�
            GUILayout.Label($"���ڐ�: {m_nameList.Count}");
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// �r���[�E���̍X�V����
    /// </summary>
    private void NameViewUpdate()
    {
        if (m_selectNumber < 0)
        {
            return;
        }

        // �E�����X�V
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            // ��b����\��
            GUILayout.Label($"ID:{m_selectNumber}   Name:{m_nameList[m_selectNumber]}");
            m_stageDataBase.stageDataList[m_selectNumber].ID = m_selectNumber;

            // ��
            EditorGUILayout.Space();

            // ���O
            m_stageDataBase.stageDataList[m_selectNumber].Name =
                EditorGUILayout.TextField("���O",m_stageDataBase.stageDataList[m_selectNumber].Name);
            // BGM
            m_stageDataBase.stageDataList[m_selectNumber].BGM =
                (BGMNumber)EditorGUILayout.Popup(
                    "BGM", (int)m_stageDataBase.stageDataList[m_selectNumber].BGM,
                    new string[] {"�^�C�g��", "�X�e�[�W�Z���N�g", "�I�v�V����", "�փ��v", "�N���A", "�Q�[���I�[�o�[", "�C���Q�[��", "�C���Q�[��2", "�C���Q�[��3"});
            // �l���ُ�ȏꍇ�͌x����\������
            if (m_stageDataBase.stageDataList[m_selectNumber].BGM < BGMNumber.enMain_Onece)
            {
                EditorGUILayout.HelpBox("�x���F�C���Q�[���ȊO��BGM���I������Ă��܂�", MessageType.Warning);
            }

            EditorGUILayout.Space();
            GUILayout.Label("Prefab");
            m_stageDataBase.stageDataList[m_selectNumber].Model =
                (GameObject)EditorGUILayout.ObjectField(m_stageDataBase.stageDataList[m_selectNumber].Model, typeof(GameObject), true);
            EditorGUILayout.Space();
            GUILayout.Label("�ڍ�");
            m_stageDataBase.stageDataList[m_selectNumber].Detail =
                EditorGUILayout.TextArea(m_stageDataBase.stageDataList[m_selectNumber].Detail);
        }
        EditorGUILayout.EndVertical();
        // �ۑ�
        Undo.RegisterCompleteObjectUndo(m_stageDataBase, "LevelDataBase");
    }

    /// <summary>
    /// ���O�ꗗ�̍쐬
    /// </summary>
    private static void ResetNameList()
    {
        m_nameList.Clear();
        // ���O����͂���
        foreach (var stage in m_stageDataBase.stageDataList)
        {
            m_nameList.Add(stage.Name);
        }
    }

    /// <summary>
    /// �����̏���
    /// </summary>
    private void Search()
    {
        if (m_searchText == "")
        {
            return;
        }

        // ������
        int startNumber = m_selectNumber;
        startNumber = Mathf.Max(startNumber, 0);

        for (int i = startNumber; i < m_nameList.Count; i++)
        {
            if (m_nameList[i].Contains(m_searchText))
            {
                // �q�b�g������I��
                m_selectNumber = i;
                GUI.FocusControl("");
                Repaint();
                return;
            }
            // �q�b�g���Ȃ��ꍇ��-1
            m_selectNumber = -1;
        }
    }

    /// <summary>
    /// �f�[�^�̒ǉ�����
    /// </summary>
    private void AddData()
    {
        var newLevelData = new StageData();
        // �ǉ�
        m_stageDataBase.stageDataList.Add(newLevelData);
    }

    /// <summary>
    /// �f�[�^�̍폜����
    /// </summary>
    private void DeleteData()
    {
        if (m_selectNumber == -1)
        {
            return;
        }
        // �I���ʒu�̃f�[�^���폜
        m_stageDataBase.stageDataList.Remove(m_stageDataBase.stageDataList[m_selectNumber]);
        // ����
        m_selectNumber -= 1;
        m_selectNumber = Mathf.Max(m_selectNumber, 0);
    }
}
