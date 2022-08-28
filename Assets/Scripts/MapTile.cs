using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>タイルを管理するクラス </summary>
public class MapTile : MonoBehaviour
{
    /// <summary>タイルに乗っている街の人リスト </summary>
    List<HumanMove> _humans = new List<HumanMove>();
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
    public List<HumanMove> Humans { get => _humans; set => _humans = value; }

    void Start()
    {
        for (var i = 0; i < transform.childCount; i++)  //各ポイントを取得し、リストを作成する
        {
            if (transform.GetChild(i).TryGetComponent<RoadPoint>(out var roadPoint))
            {
                var nextStatus = i % Enum.GetValues(typeof(PointStatus)).Length;
                roadPoint.CurrentStatus = (PointStatus)nextStatus;
                _tilePoints.Add(roadPoint);
            }
        }
    }

    /// <summary> 最も近いポイントのステータスを返す </summary>
    public PointStatus GetNearRoadPoint(Vector3 playerPosition)
    {
        var nearPoint = _tilePoints.OrderBy(x => Vector3.Distance(playerPosition, x.transform.position)).FirstOrDefault();
        return nearPoint.CurrentStatus;
    }

    /// <summary>
    /// タイルに進入して来た人がリストに入っているか調べて
    /// 処理を行う
    /// </summary>
    public void CheckHumans(HumanMove human)
    {
        if (_humans.Contains(human))
        {
            _humans.Remove(human);
            Debug.Log("削除");
        }
        else
        {
            _humans.Add(human);
            Debug.Log("追加");
        }
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
