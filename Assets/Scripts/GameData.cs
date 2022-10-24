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
    static Dictionary<BuildingType, GameObject> _stageSelectTiles = new Dictionary<BuildingType, GameObject>();
    /// <summary>各ステージ情報のリスト </summary>
    static List<StageInfo> _stageInfos = new List<StageInfo>();

    public static Dictionary<BuildingType, GameObject> StageSelectTiles { get => _stageSelectTiles; }
    /// <summary>各ステージ情報のリスト </summary>
    public static List<StageInfo> StageInfos { get => _stageInfos; }

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

        SortTiles(tmpDictionary);
        Debug.Log("end");
    }

    /// <summary>StageSelectTilesの順番に並べる(Dictionaryを作成) </summary>
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

/// <summary>大まかなタイルの種類 </summary>
public enum TileType
{
    /// <summary>道 </summary>
    Road = 0,
    /// <summary>建物 </summary>
    Building = 1,
    /// <summary>無し </summary>
    None = 2,
}

/// <summary>道の種類 </summary>
public enum RoadType
{
    /// <summary>真っ直ぐ </summary>
    Straight = 0,
    /// <summary>横道 </summary>
    Side = 1,
    /// <summary>曲がり角 </summary>
    Corner = 2,
    /// <summary>T字路 </summary>
    TRoad = 3,
    /// <summary>十字路 </summary>
    Cross = 4,
}

/// <summary>建物の種類 </summary>
public enum BuildingType
{
    /// <summary>家１ </summary>
    House_1 = 0,
    /// <summary>化学研究所 </summary>
    Chemical_laboratory = 1,
    /// <summary>服屋 </summary>
    Clothing_store = 2,
    /// <summary>鉱山 </summary>
    Mine = 3,
    /// <summary>製薬工場 </summary>
    Pharmaceutical_factory = 4,
    /// <summary>学校 </summary>
    School = 5,
}

/// <summary>無しタイルの種類 </summary>
public enum NoneType
{
    /// <summary>ベンチ（単体） </summary>
    Bench = 0,
    /// <summary>ベンチ（複数） </summary>
    Benches = 1,
    /// <summary>木々 </summary>
    Trees = 2,
}

/// <summary>タイルのID(種類) </summary>
public enum TileID
{
    /// <summary>道なし　タイル </summary>
    None = 0,
    /// <summary>真っ直ぐ </summary>
    Straight = 1,
    /// <summary>横道 </summary>
    Side = 2,
    /// <summary>曲がり角 </summary>
    Corner = 3,
    /// <summary>T字路 </summary>
    TRoad = 4,
    /// <summary>十字路 </summary>
    Cross = 5,
    /// <summary>学校 </summary>
    School = 6,
    /// <summary>家１ </summary>
    House1 = 7,
    /// <summary>化学研究所 </summary>
    Chemical_laboratory = 8,
    /// <summary>服屋 </summary>
    Clothing_store = 9,
    /// <summary>鉱山 </summary>
    Mine = 10,
    /// <summary>製薬工場 </summary>
    Pharmaceutical_factory = 11,
}

