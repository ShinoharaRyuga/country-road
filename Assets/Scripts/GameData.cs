using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//�Q�[�����f�[�^���܂Ƃ߂Ă���
//CSV����ݒ肳�����
public class GameData
{
    /// <summary>�^�C���ǂݍ��݂Ɏg�p����p�X </summary>
    const string LOAD_PATH = "Assets/Prefabs/Tiles";
    static Dictionary<StageSelectTiles, GameObject> _stageSelectTiles = new Dictionary<StageSelectTiles, GameObject>();
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    static List<StageInfo> _stageInfos = new List<StageInfo>();

    public static Dictionary<StageSelectTiles, GameObject> StageSelectTiles { get => _stageSelectTiles; }
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    public static List<StageInfo> StageInfos { get => _stageInfos; }

    /// <summary>�^�C�������̃t�H���_����擾���� </summary>
    public static void GetTiles()
    {
        var filePathArray = Directory.GetFiles(LOAD_PATH, "*", SearchOption.AllDirectories);
        var tmpDictionary = new Dictionary<string, GameObject>();

        foreach (var filePath in filePathArray)
        {
            var extension = Path.GetExtension(filePath);

            if (extension == ".prefab")
            {
                var tile = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                tmpDictionary.Add(tile.name, tile);
            }
        }

        SortTiles(tmpDictionary);
        Debug.Log("end");
    }

    /// <summary>StageSelectTiles�̏��Ԃɕ��ׂ�(Dictionary���쐬) </summary>
    static void SortTiles(Dictionary<string, GameObject> dictionary)
    {
        for (var i = 0; i < Enum.GetNames(typeof(StageSelectTiles)).Length; i++)
        {
            var stageSelectTileName = (StageSelectTiles)i;

            foreach (var tilename in dictionary.Keys)
            {
                if (tilename.Contains(stageSelectTileName.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    _stageSelectTiles.Add((StageSelectTiles)i, dictionary[tilename]);
                }
            }
        }
    }
}

/// <summary>�X�e�[�W�Z���N�g��ʂŎg�p����^�C�� </summary>
public enum StageSelectTiles
{
    School = 0,
    House1 = 1,
    /// <summary>���w������ </summary>
    Chemical_laboratory = 2,
    /// <summary>���� </summary>
    Clothing_store = 3,
    /// <summary>�z�R </summary>
    Mine = 4,
    /// <summary>����H�� </summary>
    Pharmaceutical_factory = 5,
}

/// <summary>�X�e�[�W�Ő����l������ׂ̏��� </summary>
public enum GetStarCondition
{
    /// <summary>�X�e�[�W�N���A </summary>
    StageClear = 0,
    /// <summary>�m�[�~�X�N���A</summary>
    Perfect = 1,
    /// <summary>����ȓ��ɃN���A </summary>
    MoveCountLess = 2,
}


