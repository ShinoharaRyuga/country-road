using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイルの基底クラス
/// 　T字路や曲がり角などを派生させて作成する  
/// </summary>
public class TileBase : MonoBehaviour
{
    /// <summary>タイルに乗っている街人のリスト </summary>
    List<HumanMove> _onHumans = new List<HumanMove>();
    /// <summary>繋がっているタイル </summary>
    Dictionary<ConnectingTiles, TileBase> _connectingTiles = new Dictionary<ConnectingTiles, TileBase>();

    public List<HumanMove> OnHumans { get => _onHumans; set => _onHumans = value; }
    public Dictionary<ConnectingTiles, TileBase> ConnectingTiles { get => _connectingTiles; set => _connectingTiles = value; }

    void Start()
    {
        //各ポイントを取得する
        for (var i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ConnectingTiles tiles))
            {
                if (!tiles.IsMiddle)
                {
                    _connectingTiles.Add(tiles, null);
                }
            }
        }
    }

    /// <summary>接続されたタイルを_connectingTilesに追加する </summary>
    public void AddConnectedTile(ConnectingTiles key, TileBase tile)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            _connectingTiles[key] = tile;
        }
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

    //あとで削除
    Start = 99,
    End = 98,
}
