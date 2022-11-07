using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>CSVファイルを読みマップデータを作成するクラス</summary>
public class GetMapData : MonoBehaviour
{
    [SerializeField, Header("CSVファイル")] TextAsset _dataTable = default;
    CreateMap _createMap => GetComponent<CreateMap>();

    private void Awake()
    {
        BuildTileData();
    }

    /// <summary>読み込んだファイルを元にデータを作成する </summary>
    void BuildTileData()
    {
        System.IO.StringReader sr = new System.IO.StringReader(_dataTable.text);
        var mapSize = sr.ReadLine().Split(',');
        _createMap.Row = int.Parse(mapSize[0]);
        _createMap.Col = int.Parse(mapSize[1]);

        while (true)
        {
            var line = sr.ReadLine();
        
            if (string.IsNullOrEmpty(line))
            {
                Debug.Log("finish");
                break;
            }

            var values = line.Split(",");
            CreateTileData(values);
        }
    }

    /// <summary>受け取ったデータを元にTileDataの構造体を作成する </summary>
    void CreateTileData(string[] target)
    {
        //for (var i = 0; i < target.Length; i++)
        //{
        //    var values = target[i].Split('-');
        //    var status = (TileID)int.Parse(values[0]);
        //    var tileData = new TileData(status, float.Parse(values[1]));
        //    _createMap.TileDataList.Add(tileData);
        //}
    }
}

/// <summary>タイル情報</summary>
public struct TileData
{
    /// <summary>タイルの種類</summary>
   // public TileID TileStatus;
    /// <summary>回転値 </summary>
    public float RotationValue;

    //public TileData(TileID status, float value)
    //{
    //  //  TileStatus = status;
    //    RotationValue = value;
    //}
}
