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

/// <summary>�����\������ׂɕK�v�^�C����ID </summary>
public enum TileID
{
    ///=====���^�C��=====
    /// <summary>�^������ </summary>
    Straight = 0,
    /// <summary>�Ȃ���p </summary>
    Corner = 1,
    /// <summary>T���H </summary>
    TRoad = 2,
    /// <summary>�\���H </summary>
    Cross = 3,

    ///=====���Ȃ��^�C��=====
    /// <summary>��^�C�� </summary>
    None = 4,
    /// <summary>�؃^�C�� </summary>
    NoneTrees = 5,
    /// <summary>�x���`�^�C��(����) </summary>
    NoneBenches = 6,
    /// <summary>�x���`�^�C��(�P��) </summary>
    NoneBench = 7,

    ///=====�X�^�[�g�E�S�[���^�C��=====
    StartTile = 8,
    GoalTile = 9,
}

