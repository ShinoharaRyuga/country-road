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
    const string LOAD_PATH = "Assets/Prefabs/Tiles";
    static Dictionary<StageSelectTiles, GameObject> _stageSelectTiles = new Dictionary<StageSelectTiles, GameObject>();
    /// <summary>各ステージ情報のリスト </summary>
    static List<StageInfo> _stageInfos = new List<StageInfo>();

    public static Dictionary<StageSelectTiles, GameObject> StageSelectTiles { get => _stageSelectTiles; }
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

/// <summary>ステージセレクト画面で使用するタイル </summary>
public enum StageSelectTiles
{
    School = 0,
    House1 = 1,
    /// <summary>化学研究所 </summary>
    Chemical_laboratory = 2,
    /// <summary>服屋 </summary>
    Clothing_store = 3,
    /// <summary>鉱山 </summary>
    Mine = 4,
    /// <summary>製薬工場 </summary>
    Pharmaceutical_factory = 5,
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


