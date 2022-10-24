using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>経路探索を行うクラス </summary>
public class Pathfinding : MonoBehaviour
{
    int _row = 0;
    int _col = 0;
    int _goalRow = 0;
    int _goalCol = 0;
    int _moveCost = 0;
    TileController[,] _mapTiles = default;
    TileController _startTile = default;
    TileController _goalTile = default;
    List<TileController> _openTiles = new List<TileController>();

    StageManager _manager => GetComponent<StageManager>();  
    public int GoalRow { get => _goalRow; set => _goalRow = value; }
    public int GoalCol { get => _goalCol; set => _goalCol = value; }
    public TileController StartTile { get => _startTile; set => _startTile = value; }
    public TileController[,] MapTiles { get => _mapTiles; set => _mapTiles = value; }


    /// <summary>配列内のタイルを入れ替える </summary>
    public void SwapTile(TileController start, TileController end)
    {
        var startTile = _mapTiles[start.Row, start.Col];
        var endTile = _mapTiles[end.Row, end.Col];
        var tmp = startTile;
        _mapTiles[endTile.Row, endTile.Col] = tmp;
        _mapTiles[startTile.Row, startTile.Col] = endTile;
    }

    void OpenAroundTile(TileController currentTile)
    {
        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.First))
        {
            var firstTile = currentTile.ConnectingTiles[PointStatus.First];

            if (firstTile != null)
            {
                Debug.Log($"first {firstTile.name}");
                OpenTile(firstTile);
            }
        }

        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.Second))
        {
            var secondTile = currentTile.ConnectingTiles[PointStatus.Second];

            if (secondTile != null)
            {
                Debug.Log($"Second {secondTile.name}");
                OpenTile(secondTile);
            }
        }

        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.Third))
        {
            var thirdTile = currentTile.ConnectingTiles[PointStatus.Third];

            if (thirdTile != null)
            {
                Debug.Log($"Third {thirdTile.name}");
                OpenTile(thirdTile);
            }
        }

        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.Fourth))
        {
            var fourthTile = currentTile.ConnectingTiles[PointStatus.Fourth];


            if (fourthTile != null)
            {
                Debug.Log($"fourth {fourthTile}");
                OpenTile(fourthTile);
            }
        }
    }

    void OpenTile(TileController tile)
    {
        if (tile.AstarStatus == AstarStatus.Empty)
        {
            tile.AstarStatus = AstarStatus.Open;
            _openTiles.Add(tile);
            tile.SetCosts(_moveCost, GetGuessCost(tile.Row, tile.Col));
        }
    }

    /// <summary>ゴールタイルまで距離(推定コスト)を計算する </summary>
    /// <returns>推定コスト</returns>
    int GetGuessCost(int r, int c)
    {
        var disR = _goalTile.Row - r;
        var disC = _goalTile.Col - c;

        return disR + disC;
    }
}

