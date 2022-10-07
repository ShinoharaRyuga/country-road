using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>マップ生成関連処理を行うクラス </summary>
public class CreateMap : MonoBehaviour
{
    /// <summary>タイルの大きさ </summary>
    const int TILE_SCALE = 5;
    /// <summary>次タイルを生成するまで時間 </summary>
    const float NEXT_TILE_WAIT = 0.3f;
    /// <summary>タイル生成時のY位置 </summary>
    const float FIRST_POSITION_Y = 30;
    /// <summary>ゲーム開始カウントダウンが始まるまでの時間 </summary>
    const float COUNTDOWN_START_TIME = 3f;
   
    [SerializeField] TileBase[] _tiles;
    [SerializeField] TileBase[] _noneTiles;
    TileBase[,] _mapTiles = default;
    TileBase _startTile = default;
    TileBase _goalTile = default;
    int _row = 0;
    int _col = 0;

    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }

    StageManager _manager => GetComponent<StageManager>();

    /// <summary>マップ配置のリストを受け取りマップを生成する </summary>
    /// <param name="tileDataList">マップ配置リスト</param>
    public void MapCreate(List<TileData> tileDataList)
    {
        _mapTiles = new TileBase[_row, _col];
        
        var index = 0;
        var xPos = 0;
        var zPos = _col * TILE_SCALE;
        var generateTiles = new TileBase[tileDataList.Count];

        for (var c = 0; c < _col; c++)
        {
            zPos -= TILE_SCALE;
            xPos = 0;

            for (var r = 0; r < _row; r++)
            {
                var tileData = tileDataList[index];
                var tileIndex = (int)tileData.TileStatus;
                var tile = _tiles[0];

                switch (tileData.TileStatus)    //タイルを作成
                {
                    case TileStatus.None:
                        var rand = Random.Range(0, _noneTiles.Length);
                        tile = Instantiate(_noneTiles[rand], new Vector3(xPos, FIRST_POSITION_Y, zPos), Quaternion.identity);
                        break;
                    case TileStatus.Side:
                        tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, FIRST_POSITION_Y, zPos), Quaternion.Euler(0, 90, 0));
                        break;
                    default:
                        tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, FIRST_POSITION_Y, zPos), Quaternion.Euler(0, tileData.RotationValue, 0));
                        break;
                }

                if (tileData.TileStatus == TileStatus.Start)
                {
                    tile.transform.position = new Vector3(xPos, 0, zPos);
                    _startTile = tile;
                }
                else if (tileData.TileStatus == TileStatus.Goal)
                {
                    tile.transform.position = new Vector3(xPos, 0, zPos);
                    _goalTile = tile;
                }

                generateTiles[index] = tile;
                tile.SetPoint(r, c);
                index++;
                xPos += TILE_SCALE;
                _mapTiles[r, c] = tile;
            }
        }

        StartCoroutine(FallTileRamdom(generateTiles));
    }

    /// <summary>
    /// タイルをランダムに落としていく
    /// ステージ定位置に移動させる
    /// </summary>
    /// <param name="tileBases">生成されたタイル</param>
    IEnumerator FallTileRamdom(TileBase[] tileBases)
    {
        var selectedNumbers = new List<int>();
        for (var i = 0; i < tileBases.Length;)
        {
            var rand = Random.Range(0, tileBases.Length);

            if (!selectedNumbers.Contains(rand))
            {
                StartCoroutine(tileBases[rand].StartMove());
                selectedNumbers.Add(rand);
                i++;
                yield return new WaitForSeconds(NEXT_TILE_WAIT);
            }
        }

        yield return new WaitForSeconds(COUNTDOWN_START_TIME);
        _manager.GameStart();
    }
}
