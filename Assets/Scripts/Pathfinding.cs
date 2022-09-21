using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    const int TILE_SCALE = 5;

    [SerializeField] TileBase[] _tiles;
    int _row = 0;
    int _col = 0;
    int _goalRow = 0;
    int _goalCol = 0;
    int _moveCost = 0;
    bool _isFirst = true;

    TileBase[,] _mapTiles = default;
    TileBase _startTile = default;
    TileBase _goalTile = default;
    List<TileBase> _openTiles = new List<TileBase>();

    public int GoalRow { get => _goalRow; }
    public int GoalCol { get => _goalCol; }
    public TileBase StartTile { get => _startTile; set => _startTile = value; }

    private void Update()
    {
        if (_isFirst)
        {
            foreach (var tile in _mapTiles)
            {
                tile.ActiveCollider();
            }

            _isFirst = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            for (var c = 0; c < _col; c++)
            {
                var str = "";
                for (var r = 0; r < _row; r++)
                {
                    var status = (int)_mapTiles[r, c].CurrnetStatus;
                    str += status.ToString();
                }

                Debug.Log(str);
            }
        }
    }

    /// <summary>マップの大きさを設定する </summary>
    public void SetMapSize(int row, int col)
    {
        _row = row;
        _col = col;
    }

    /// <summary>配列内のタイルを入れ替える </summary>
    public void SwapTile(TileBase start, TileBase end)
    {
        var startTile = _mapTiles[start.Row, start.Col];
        var endTile = _mapTiles[end.Row, end.Col];
        var tmp = startTile;
        _mapTiles[endTile.Row, endTile.Col] = tmp;
        _mapTiles[startTile.Row, startTile.Col] = endTile;
    }


    /// <summary>マップを作成する </summary>
    public void CreateMap(List<TileData> tileDataList)
    {
        _mapTiles = new TileBase[_row, _col];

        var index = 0;
        var xPos = 0;
        var zPos = _col * TILE_SCALE;

        for (var c = 0; c < _col; c++)
        {
            zPos -= TILE_SCALE;
            xPos = 0;

            for (var r = 0; r < _row; r++)
            {
                var tileData = tileDataList[index];
                var tileIndex = (int)tileData.TileStatus;
                var tile = _tiles[0];

                if (tileData.TileStatus != TileStatus.Side)
                {
                    tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, 0, zPos), Quaternion.Euler(0, tileData.RotationValue, 0));
                }
                else
                {
                    tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, 0, zPos), Quaternion.Euler(0, 90, 0));
                }

                if (tileData.TileStatus == TileStatus.Start)
                {
                    _startTile = tile;
                }
                else if (tileData.TileStatus == TileStatus.Goal)
                {
                    _goalTile = tile;
                    _goalCol = c;
                    _goalRow = r;
                }

                tile.PathfindingClass = this;
                tile.SetPoint(r, c);
                index++;
                xPos += TILE_SCALE;
                _mapTiles[r, c] = tile;
            }
        }
    }

    void OpenAroundTile(TileBase currentTile)
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

    void OpenTile(TileBase tile)
    {
        if (tile.AstarStatus == AstarStatus.Empty)
        {
            tile.AstarStatus = AstarStatus.Open;
            _openTiles.Add(tile);
            tile.SetCosts(_moveCost, GetGuessCost(tile.Row, tile.Col));
        }
    }

    int GetGuessCost(int r, int c)
    {
        var disR = _goalTile.Row - r;
        var disC = _goalTile.Col - c;

        return disR + disC;
    }
}

