using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//ゲーム内データをまとめておく
//CSVから設定させる為
public class GameData
{
    /// <summary>タイル読み込みに使用するパス </summary>
    const string LOAD_PATH = "Assets/Prefabs/BuildingTiles";
   
    /// <summary>タイルを特定のフォルダから取得する </summary>
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

/// <summary>ステージで星を獲得する為の条件 </summary>
public enum GetStarCondition
{
    /// <summary>ステージクリア </summary>
    StageClear = 0,
    /// <summary>ノーミスクリア</summary>
    Perfect = 1,
    /// <summary>○手以内にクリア </summary>
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

/// <summary>スタートタイルまたはゴールタイルであるかどうか </summary>
public enum TilePoint
{
    /// <summary> </summary>
    None = 0,
    Start = 1,
    Goal = 2,
}

/// <summary>道を構成する為に必要タイルのID </summary>
public enum TileID
{
    ///=====道タイル=====
    /// <summary>真っ直ぐ </summary>
    Straight = 0,
    /// <summary>曲がり角 </summary>
    Corner = 1,
    /// <summary>T字路 </summary>
    TRoad = 2,
    /// <summary>十字路 </summary>
    Cross = 3,

    ///=====道なしタイル=====
    /// <summary>空タイル </summary>
    None = 4,
    /// <summary>木タイル </summary>
    NoneTrees = 5,
    /// <summary>ベンチタイル(複数) </summary>
    NoneBenches = 6,
    /// <summary>ベンチタイル(単体) </summary>
    NoneBench = 7,

    ///=====スタート・ゴールタイル=====
    StartTile = 8,
    GoalTile = 9,
}

