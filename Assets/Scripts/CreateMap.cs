using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>マップ生成関連処理を行うクラス </summary>
public class CreateMap : MonoBehaviour
{
    /// <summary>タイルの大きさ </summary>
    const int TILE_SCALE = 5;
    /// <summary>次タイルを生成するまで時間 </summary>
    const float NEXT_TILE_WAIT = 0.2f;
    /// <summary>タイル生成時のY位置 </summary>
    const float FIRST_POSITION_Y = 30;
    /// <summary>ゲーム開始カウントダウンが始まるまでの時間 </summary>
    const float COUNTDOWN_START_TIME = 3f;

    [SerializeField, Header("自動でマップ生成を行う")] bool _autoCreate = false;
    [SerializeField] TileController[] _tiles;
    [SerializeField] TileController[] _startTiles = default;
    [SerializeField] TileController[] _goalTiles = default;
    TileController _startTile = default;
    TileController _goalTile = default;
    List<TileData> _tileDataList = new List<TileData>();
    int _row = 0;
    int _col = 0;

    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }
    public TileController StartTile { get => _startTile; set => _startTile = value; }
    public TileController GoalTile { get => _goalTile; set => _goalTile = value; }
    public List<TileData> TileDataList { get => _tileDataList; set => _tileDataList = value; }

    private void Start()
    {
        if (_autoCreate)
        {
            MapCreate();
        }
    }

    /// <summary>
    /// マップ配置のリストを受け取りマップを生成する 
    /// チュートリアルマップのみプレイヤーのタイミングでマップ生成を開始する
    /// </summary>
    public void MapCreate()
    {
        var index = 0;
        var xPos = 0;
        var zPos = _col * TILE_SCALE;
        var generateTiles = new TileController[_tileDataList.Count];

        for (var c = 0; c < _col; c++)
        {
            zPos -= TILE_SCALE;
            xPos = 0;

            for (var r = 0; r < _row; r++)
            {
                var tileData = _tileDataList[index];
                var tileIndex = (int)tileData.TileStatus;
                var tile = _tiles[0];


                switch (tileData.TileStatus)
                {
                    case TileID.StartTile:
                        tile = Instantiate(_startTiles[0], new Vector3(xPos, FIRST_POSITION_Y, zPos), Quaternion.Euler(0, tileData.RotationValue, 0));
                        _startTile = tile;
                        break;
                    case TileID.GoalTile:
                        tile = Instantiate(_goalTiles[0], new Vector3(xPos, FIRST_POSITION_Y, zPos), Quaternion.Euler(0, tileData.RotationValue, 0));
                        break;
                        default:
                        tile = Instantiate(_tiles[tileIndex], new Vector3(xPos, FIRST_POSITION_Y, zPos), Quaternion.Euler(0, tileData.RotationValue, 0));
                        break;
                }

                generateTiles[index] = tile;
                tile.SetPoint(r, c);
                index++;
                xPos += TILE_SCALE;
            }
        }

        StartCoroutine(FallTileRamdom(generateTiles));
    }

    /// <summary>
    /// タイルをランダムに落としていく
    /// ステージ定位置に移動させる
    /// </summary>
    /// <param name="tileBases">生成されたタイル</param>
    IEnumerator FallTileRamdom(TileController[] tileBases)
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
    }
}
