using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>タイルを管理するクラス </summary>
public class MapTile : MonoBehaviour
{
    /// <summary>タイルの中心と出入口の位置 </summary>
    List<RoadPoint> _tilePoints = new List<RoadPoint>();
    /// <summary>スタートポイントと繋がっているタイル </summary>
    MapTile _startConnectionTile = default;
    /// <summary>エンドポイントと繋がっているタイル </summary>
    MapTile _endConnectionTile = default;
    /// <summary>タイルの中心と出入口の位置 </summary>
    public List<RoadPoint> TilePoints { get => _tilePoints; }
    /// <summary>スタートポイントと繋がっているタイル </summary>
    public MapTile StartConnectionTile { get => _startConnectionTile; set => _startConnectionTile = value; }
    /// <summary>エンドポイントと繋がっているタイル </summary>
    public MapTile EndConnectionTile { get => _endConnectionTile; set => _endConnectionTile = value; }

    void Start()
    {
        //for (var i = 0; i < transform.childCount; i++)  //各ポイントを取得し、リストを作成する
        //{
        //    var roadPoint = transform.GetChild(i).gameObject.GetComponent<RoadPoint>();
        //    var nextStatus = i % Enum.GetValues(typeof(PointStatus)).Length;
        //    roadPoint.PointStatus = (PointStatus)nextStatus;
        //    _tilePoints.Add(roadPoint);
        //}
    }

    /// <summary>
    /// 街の人がエンドポイントから進入してきたら
    /// スタートポイントとエンドポイントを入れ替える
    /// </summary>
    public void Swap()
    {
        var tmp = _tilePoints[0];
        _tilePoints[0] = _tilePoints[2];
        _tilePoints[2] = tmp;
    }
}
