using System;
using UnityEngine;

/// <summary>CSVファイルを読みマップデータを作成するクラス</summary>
public class GetMapData : MonoBehaviour
{
    [SerializeField, Header("CSVファイル")] TextAsset _dataTable = default;

    Pathfinding _pathfinding => GetComponent<Pathfinding>();

    private void Awake()
    {
        BuildTileData();
    }

    private void Start()
    {
        _pathfinding.CreateMap();
    }

    void BuildTileData()
    {
        System.IO.StringReader sr = new System.IO.StringReader(_dataTable.text);
        var mapSize = sr.ReadLine().Split(',');
        _pathfinding.Row = int.Parse(mapSize[0]);
        _pathfinding.Col = int.Parse(mapSize[1]);

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

    void CreateTileData(string[] target)
    {
        for (var i = 0; i < target.Length; i++)
        {
            var values = target[i].Split('-');
            var status = (TileStatus)int.Parse(values[0]);
            var tileData = new TileData(status, float.Parse(values[1]));
            _pathfinding.TileDataList.Add(tileData);
        }
    }
}

public struct TileData
{
    public TileStatus TileStatus;
    public float RotationValue;

    public TileData(TileStatus status, float value)
    {
        TileStatus = status;
        RotationValue = value;
    }
}
