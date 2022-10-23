using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�}�b�v�����֘A�������s���N���X </summary>
public class CreateMap : MonoBehaviour
{
    /// <summary>�^�C���̑傫�� </summary>
    const int TILE_SCALE = 5;
    /// <summary>���^�C���𐶐�����܂Ŏ��� </summary>
    const float NEXT_TILE_WAIT = 0.2f;
    /// <summary>�^�C����������Y�ʒu </summary>
    const float FIRST_POSITION_Y = 30;
    /// <summary>�Q�[���J�n�J�E���g�_�E�����n�܂�܂ł̎��� </summary>
    const float COUNTDOWN_START_TIME = 3f;

    [SerializeField, Header("�����Ń}�b�v�������s��")] bool _autoCreate = false;
    [SerializeField] TileBase[] _tiles;
    [SerializeField] TileBase[] _noneTiles;
    TileBase _startTile = default;
    TileBase _goalTile = default;
    List<TileData> _tileDataList = new List<TileData>();
    int _row = 0;
    int _col = 0;

    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }
    public TileBase StartTile { get => _startTile; set => _startTile = value; }
    public TileBase GoalTile { get => _goalTile; set => _goalTile = value; }
    public List<TileData> TileDataList { get => _tileDataList; set => _tileDataList = value; }


    private void Start()
    {
        if (_autoCreate)
        {
           MapCreate();
        }
    }

    /// <summary>
    /// �}�b�v�z�u�̃��X�g���󂯎��}�b�v�𐶐����� 
    /// �`���[�g���A���}�b�v�̂݃v���C���[�̃^�C�~���O�Ń}�b�v�������J�n����
    /// </summary>
    public void MapCreate()
    {
        var index = 0;
        var xPos = 0;
        var zPos = _col * TILE_SCALE;
        var generateTiles = new TileBase[_tileDataList.Count];

        for (var c = 0; c < _col; c++)
        {
            zPos -= TILE_SCALE;
            xPos = 0;

            for (var r = 0; r < _row; r++)
            {
                var tileData = _tileDataList[index];
                var tileIndex = (int)tileData.TileStatus;
                var tile = _tiles[0];

                switch (tileData.TileStatus)    //�^�C�����쐬
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
            }
        }

        StartCoroutine(FallTileRamdom(generateTiles));
    }
    
    /// <summary>
    /// �^�C���������_���ɗ��Ƃ��Ă���
    /// �X�e�[�W��ʒu�Ɉړ�������
    /// </summary>
    /// <param name="tileBases">�������ꂽ�^�C��</param>
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
    }
}
