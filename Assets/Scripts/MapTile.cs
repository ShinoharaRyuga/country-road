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

    /// <summary> 最も近いポイントステータスを返す </summary>
    public PointStatus GetNearRoadPoint(Vector3 playerPosition)
    {
        var nearPoint = _tilePoints.OrderBy(x => Vector3.Distance(playerPosition, x.transform.position)).FirstOrDefault();
        return nearPoint.CurrentStatus;
    }

    /// <summary>
    /// タイルに進入して来た人がリストに入っているか調べて
    /// 処理を行う
    /// </summary>
    public void CheckHumans(HumanMove human, PointStatus status)
    {
        var nextTile = _startConnectionTile;

        if (status == PointStatus.End)
        {
            nextTile = _endConnectionTile;
        }

        if (_humans.Contains(human) && nextTile != null)
        {
            _humans.Remove(human);
        }
        else if (!_humans.Contains(human))
        {
            _humans.Add(human);
        }
    }

    /// <summary>タイルでとどまっていた街人達を再び移動させる</summary>
    public void AgainMove()
    {
        //削除時にInvalidOperationExceptionを出さないようにするため
        var moveHumans = new List<HumanMove>();

        foreach (var human in _humans)
        {
         //   human.ChackTileConnection();
            moveHumans.Add(human);
        }

        //移動した街人を削除
        foreach (var target in moveHumans)
        {
            _humans.Remove(target);
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
