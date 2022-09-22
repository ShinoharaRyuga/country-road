using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�}�b�v�쐬�ƌo�H�T�����s���N���X </summary>
public class Pathfinding : MonoBehaviour
{
    /// <summary>�^�C���̑傫�� </summary>
    const int TILE_SCALE = 5;
    /// <summary>���^�C���𐶐�����܂Ŏ��� </summary>
    const float NEXT_TILE_WAIT = 0.3f;

    const float COUNTDOWN_START_TIME = 3f;

    [SerializeField] TileBase[] _tiles;
    int _row = 0;
    int _col = 0;
    int _goalRow = 0;
    int _goalCol = 0;
    int _moveCost = 0;
    TileBase[,] _mapTiles = default;
    TileBase _startTile = default;
    TileBase _goalTile = default;
    List<TileBase> _openTiles = new List<TileBase>();

    StageManager _manager => GetComponent<StageManager>();  
    public int GoalRow { get => _goalRow; }
    public int GoalCol { get => _goalCol; }
    public TileBase StartTile { get => _startTile; set => _startTile = value; }

    /// <summary>�}�b�v�̑傫����ݒ肷�� </summary>
    public void SetMapSize(int row, int col)
    {
        _row = row;
        _col = col;
    }

    /// <summary>�z����̃^�C�������ւ��� </summary>
    public void SwapTile(TileBase start, TileBase end)
    {
        var startTile = _mapTiles[start.Row, start.Col];
        var endTile = _mapTiles[end.Row, end.Col];
        var tmp = startTile;
        _mapTiles[endTile.Row, endTile.Col] = tmp;
        _mapTiles[startTile.Row, startTile.Col] = endTile;
    }


    /// <summary>�}�b�v���쐬���� </summary>
    public void CreateMap(List<TileData> tileDataList)
    {
        StartCoroutine(Create(tileDataList));
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

    /// <summary>�S�[���^�C���܂ŋ���(����R�X�g)���v�Z���� </summary>
    /// <returns>����R�X�g</returns>
    int GetGuessCost(int r, int c)
    {
        var disR = _goalTile.Row - r;
        var disC = _goalTile.Col - c;

        return disR + disC;
    }

    /// <summary>���Ԋu�Ń^�C�����w��̈ʒu�ɐ������� </summary>
    /// <param name="tileDataList">�}�b�v�z�u�̃f�[�^</param>
    IEnumerator Create(List<TileData> tileDataList)
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
                    tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, 30, zPos), Quaternion.Euler(0, tileData.RotationValue, 0));
                }
                else
                {
                    tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, 30, zPos), Quaternion.Euler(0, 90, 0));
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

                yield return new WaitForSeconds(NEXT_TILE_WAIT);
            }
        }

        yield return new WaitForSeconds(COUNTDOWN_START_TIME);
        _manager.GameStart();
    }
}

