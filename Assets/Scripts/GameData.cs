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
    const string LOAD_PATH = "Assets/Prefabs/BuildingTiles";
    static Dictionary<BuildingType, GameObject> _stageSelectTiles = new Dictionary<BuildingType, GameObject>();
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    static List<StageInfo> _stageInfos = new List<StageInfo>();

    public static Dictionary<BuildingType, GameObject> StageSelectTiles { get => _stageSelectTiles; }
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
        for (var i = 0; i < Enum.GetNames(typeof(BuildingType)).Length; i++)
        {
            var stageSelectTileName = (BuildingType)i;

            foreach (var tilename in dictionary.Keys)
            {
                if (tilename.Contains(stageSelectTileName.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    _stageSelectTiles.Add((BuildingType)i, dictionary[tilename]);
                }
            }
        }
    }
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


public enum PointStatus
{
    First = 0,
    Second = 1,
    Third = 2,
    Fourth = 3,
    None = 4,
    Middle = 5,
    Goal = 6,
    Start = 7,
}

/// <summary>�X�^�[�g�^�C���܂��̓S�[���^�C���ł��邩�ǂ��� </summary>
public enum TilePoint
{
    /// <summary> </summary>
    None = 0,
    Start = 1,
    Goal = 2,
}

/// <summary>��܂��ȃ^�C���̎�� </summary>
public enum TileType
{
    /// <summary>�� </summary>
    Road = 0,
    /// <summary>���� </summary>
    Building = 1,
    /// <summary>���� </summary>
    None = 2,
}

/// <summary>���̎�� </summary>
public enum RoadType
{
    /// <summary>�^������ </summary>
    Straight = 0,
    /// <summary>���� </summary>
    Side = 1,
    /// <summary>�Ȃ���p </summary>
    Corner = 2,
    /// <summary>T���H </summary>
    TRoad = 3,
    /// <summary>�\���H </summary>
    Cross = 4,
}

/// <summary>�����̎�� </summary>
public enum BuildingType
{
    /// <summary>�ƂP </summary>
    House_1 = 0,
    /// <summary>���w������ </summary>
    Chemical_laboratory = 1,
    /// <summary>���� </summary>
    Clothing_store = 2,
    /// <summary>�z�R </summary>
    Mine = 3,
    /// <summary>����H�� </summary>
    Pharmaceutical_factory = 4,
    /// <summary>�w�Z </summary>
    School = 5,
}

/// <summary>�����^�C���̎�� </summary>
public enum NoneType
{
    /// <summary>�x���`�i�P�́j </summary>
    Bench = 0,
    /// <summary>�x���`�i�����j </summary>
    Benches = 1,
    /// <summary>�؁X </summary>
    Trees = 2,
}

/// <summary>�^�C����ID(���) </summary>
public enum TileID
{
    /// <summary>���Ȃ��@�^�C�� </summary>
    None = 0,
    /// <summary>�^������ </summary>
    Straight = 1,
    /// <summary>���� </summary>
    Side = 2,
    /// <summary>�Ȃ���p </summary>
    Corner = 3,
    /// <summary>T���H </summary>
    TRoad = 4,
    /// <summary>�\���H </summary>
    Cross = 5,
    /// <summary>�w�Z </summary>
    School = 6,
    /// <summary>�ƂP </summary>
    House1 = 7,
    /// <summary>���w������ </summary>
    Chemical_laboratory = 8,
    /// <summary>���� </summary>
    Clothing_store = 9,
    /// <summary>�z�R </summary>
    Mine = 10,
    /// <summary>����H�� </summary>
    Pharmaceutical_factory = 11,
}

