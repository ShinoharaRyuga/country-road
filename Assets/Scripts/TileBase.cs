using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイルの基底クラス
/// 　T字路や曲がり角などを派生させて作成する  
/// </summary>
public class TileBase : MonoBehaviour
{
    [SerializeField] TileStatus _currnetStatus = TileStatus.None;

    /// <summary>タイルに乗っている街人のリスト </summary>
    List<HumanMove> _onHumans = new List<HumanMove>();
    /// <summary>街人を誘導するためポイントリスト </summary>
    List<RoadPoint> _roadPoints = new List<RoadPoint>();
    /// <summary>繋がっているタイル </summary>
    Dictionary<PointStatus, TileBase> _connectingTiles = new Dictionary<PointStatus, TileBase>();
    int _row = 0;
    int _col = 0;
    public List<HumanMove> OnHumans { get => _onHumans; set => _onHumans = value; }
    public Dictionary<PointStatus, TileBase> ConnectingTiles { get => _connectingTiles; set => _connectingTiles = value; }
    public TileStatus CurrnetStatus { get => _currnetStatus; set => _currnetStatus = value; }
    public int Col { get => _col; }
    public int Row { get => _row; }

    void Start()
    {
        //各出入りと繋がっているタイルを結びつける
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.TryGetComponent(out ConnectingTiles tiles))
            {
                if (!tiles.IsMiddle)
                {
                    _connectingTiles.Add(tiles.CurrentStatus, null);
                }
            }

            if (child.TryGetComponent(out RoadPoint point))
            {
                _roadPoints.Add(point);
            }
        }
    }

    /// <summary>接続されたタイルを_connectingTilesに追加する </summary>
    public void AddConnectedTile(PointStatus key, TileBase tile)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            _connectingTiles[key] = tile;
        }
    }

    /// <summary>プレイヤーに最も近いポイントを返す</summary>
    public Vector3 GetNextPoint(Transform player, List<RoadPoint> hitPoints)
    {
        var targetPoints = new List<RoadPoint>();
        var nextPoint = _roadPoints[0];

        foreach (var point in _roadPoints)
        {
            if (!hitPoints.Contains(point))
            {
                targetPoints.Add(point);
            }
        }

        nextPoint = targetPoints.OrderBy(p => Vector3.Distance(player.position, p.transform.position)).FirstOrDefault();

        return nextPoint.transform.position;
    }

    /// <summary>ゲーム開始時一番最初に向かうポイントを取得する </summary>
    public Vector3 GetFirstPoint(Transform player)
    {
        var firstPoint = _roadPoints.OrderBy(p => Vector3.Distance(player.position, p.transform.position)).FirstOrDefault();

        return firstPoint.transform.position;
    }

    /// <summary>指定された出口にタイルが繋がっているか調べる </summary>
    /// <param name="key">指定した出口</param>
    /// <returns>繋がっていたらそのタイルを返す</returns>
    public TileBase GetNextTile(PointStatus key)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            return _connectingTiles[key];
        }

        return null;
    }

    public void SetPoint(int r, int c)
    {
        _row = r;
        _col = c;
    }
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

public enum TileStatus
{
    None = 0,
    Straight = 1,
    Side = 2,
    Corner = 3,
    TRoad = 4,
    Cross = 5,
    Start = 6,
    Goal = 7,
}
