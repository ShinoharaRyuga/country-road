using System.Collections.Generic;
using UnityEngine;

public class RouteSearch : MonoBehaviour
{
    [SerializeField] TileBase[] _tiles = default;
    [SerializeField] TileBase _startTile = default;
    [SerializeField] TileBase _goalTile = default;

    int[,] _mapData = new int[,]
    {
        {2, 0, 0 },
        {0, 0, 0 },
        {0, 0, 0 },
        {0, 0, 0 },
        {0, 0, 0 },
        {0, 0, 1 },
    };

    private void Start()
    {
        var xPos = -5;
        var zPos = 0;
        for (var i = 0; i < _mapData.GetLength(0); i++)
        {
            xPos = -5;
            zPos += 5;
            for (var k = 0; k < _mapData.GetLength(1); k++)
            {
                var data = _mapData[i, k];

                switch (data)
                {
                    case 0:
                        var index = Random.Range(0, 5);
                        var tile = Instantiate(_tiles[index], new Vector3(xPos, 0, zPos), Quaternion.identity);
                        tile.gameObject.name = $"{i} {k}";
                        tile.SetPoint(i, k);
                        break;
                        case 1:
                        var startTile = Instantiate(_startTile, new Vector3(xPos, 0, zPos), Quaternion.identity);
                        break;
                    case 2:
                        var goalTile = Instantiate(_goalTile, new Vector3(xPos, 0, zPos), Quaternion.identity);
                        break;
                }

                xPos += 5;
            }
        }

    }

    public void Search(TileBase lastTile, TileBase currentTile)
    {
        var routes = new Dictionary<PointStatus, int>();


    }

    public void CheckRoute(TileBase currentTile)
    {
        foreach (var tile in currentTile.ConnectingTiles)
        {
            Debug.Log(tile.Key);
            Debug.Log(tile.Value);
        }
    }
}
