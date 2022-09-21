using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    const int TILE_SCALE = 5;

    [SerializeField] TileBase[] _tiles;
    int _row = 0;
    int _col = 0;
    List<TileData> _tileDataList = new List<TileData>();
    TileBase[,] _mapTiles = default;
    int[,] _pathData = default;

    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }
    public List<TileData> TileDataList { get => _tileDataList; set => _tileDataList = value; }


    public void CreateMap()
    {
        _pathData = new int[_row, _col];
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
               

                var tileData = _tileDataList[index];
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

                index++;
                xPos += TILE_SCALE;
                _mapTiles[r, c] = tile;
            }
        }
    }
}

