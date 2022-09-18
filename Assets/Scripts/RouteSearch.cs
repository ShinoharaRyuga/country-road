using System.Collections.Generic;
using UnityEngine;

public class RouteSearch : MonoBehaviour
{
    [SerializeField] TileBase[] _tiles = default;
    [SerializeField] TileBase _startTile = default;
    [SerializeField] TileBase _goalTile = default;
    [SerializeField] TileBase[] _stageTiles = default;

    int _goalRow = 0;
    int _goalCol = 0;
    int _moveCost = 0;
    bool _isGoal = false;
    bool _isFirst = true;

    List<TileBase> _openTiles = new List<TileBase>();
    List<TileBase> _goalWay = new List<TileBase>();

    TileBase _currentTile;
    TileBase _lastTile;

    TileBase _test;

    TileBase[,] _tileBase;

    private void Start()
    {
        _tileBase = new TileBase[6, 3];
        var index = 0;

        for (var i = 0; i < 6; i++)
        {
            for (var k = 0; k < 3; k++)
            {
                _tileBase[i, k] = _stageTiles[index];
                index++;
            }
        }

        _currentTile = _stageTiles[0];
    }

    private void Update()
    {
        if (_isFirst)
        {
            foreach (var tile in _stageTiles)
            {
                tile.ActiveCollider();
            }

            _isFirst = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _goalWay.Add(_currentTile);
            OpenAroundTile(_currentTile, null);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(_openTiles.Count);
            _openTiles.ForEach(t => Debug.Log(t));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(_test.name);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _goalWay.ForEach(t => Debug.Log(t.name));
        }
    }

    void OpenAroundTile(TileBase tile, TileBase lastTile)
    {
        if(_isGoal) { return; }

        _moveCost++;

        if (tile.ConnectingTiles.ContainsKey(PointStatus.First))
        {
            var firstTile = tile.ConnectingTiles[PointStatus.First];

            if (firstTile != null)
            {
                Debug.Log($"first {firstTile.name}");
                OpenTile(firstTile);
            }
        }

        if (tile.ConnectingTiles.ContainsKey(PointStatus.Second))
        {
            var secondTile = tile.ConnectingTiles[PointStatus.Second];

            if (secondTile != null)
            {
                Debug.Log($"Second {secondTile.name}");
                OpenTile(secondTile);
            }
        }

        if (tile.ConnectingTiles.ContainsKey(PointStatus.Third))
        {
            var thirdTile = tile.ConnectingTiles[PointStatus.Third];

            if (thirdTile != null)
            {
                Debug.Log($"Third {thirdTile.name}");
                OpenTile(thirdTile);
            }
        }

        if (tile.ConnectingTiles.ContainsKey(PointStatus.Fourth))
        {
            var fourthTile = tile.ConnectingTiles[PointStatus.Fourth];
          

            if (fourthTile != null)
            {
                Debug.Log($"fourth {fourthTile}");
                OpenTile(fourthTile);
            }
        }

        tile.AstarStatus = AstarStatus.Close;
        OpenNextTile();
        _lastTile = tile;
    }

    void OpenTile(TileBase tile)
    {
        if (tile.AstarStatus == AstarStatus.Empty)
        {
            tile.AstarStatus = AstarStatus.Open;
            _openTiles.Add(tile);
            tile.SetCosts(_moveCost, GetGuessCost(tile.Row, tile.Col));
        }

        if (tile.CurrnetStatus == TileStatus.Goal)
        {
            _goalWay.Add(tile);
            _openTiles.Clear();
            Debug.Log("Goal");
        }
    }

    void OpenNextTile()
    {

        //TODO実コストと推定コストで調べた方がよいかも
        if (_openTiles.Count <= 0) { return; }

        var nextTile = _openTiles[0];
        var minScore = nextTile.Score;

        foreach (var tile in _openTiles)
        {
            if (tile.Score < minScore)
            {
                nextTile = tile;
                minScore = tile.Score;
            }

            if (tile.Score == minScore && tile.RealCost < nextTile.RealCost)
            {
                nextTile = tile;
                minScore = tile.Score;
            }
        }

        _openTiles.Clear();

        if (nextTile.CurrnetStatus == TileStatus.Goal)
        {
            Debug.Log("Goal");
            return;
        }

        _test = nextTile;

        _goalWay.Add(nextTile);
        OpenAroundTile(nextTile, _lastTile);
    }

    int GetGuessCost(int r, int c)
    {
        var disR = _goalRow - r;
        var disC = _goalCol - c;

        return disR + disC;
    }
}

public enum AstarStatus
{
    Empty = 0,
    Open = 1,
    Close = 2,
}
