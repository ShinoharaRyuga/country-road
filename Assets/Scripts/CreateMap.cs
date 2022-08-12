using System.Collections;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField] int _rows = 3;
    [SerializeField] int _columns = 3;
    [SerializeField] MapTile[] _tiles = default;
    [SerializeField] HumanMove _humanPrefab = default;

    HumanMove _human = default;
    int[] _rotationValues = new int[4] { 0, 90, 180, 270 };
    MapTile[,] _mapTiles = default;
    bool _isFirst = true;
    private void Start()
    {
        _mapTiles = new MapTile[_rows, _columns];
        Create();
        _isFirst = false;
       
    }

    public void Create()
    {
        if (!_isFirst)
        {
            for (var i = 0; i < _rows; i++)
            {
                for (var k = 0; k < _columns; k++)
                {
                    Destroy(_mapTiles[i, k].gameObject);
                }
            }
        }

        var xPos = -5;
        var zPos = 0;
        for (var i = 0; i < _rows; i++)
        {
            xPos = -5;
            zPos += 5;
            for (var k = 0; k < _columns; k++)
            {
                if (i == 0 && k == 1)
                {
                    _mapTiles[i, k] = Instantiate(_tiles[1], new Vector3(xPos, 0, zPos), Quaternion.Euler(0, _rotationValues[0], 0));
                    _mapTiles[i, k].gameObject.name = $"{i} {k}";
                    xPos += 5;
                    continue;
                }


                var index = Random.Range(0, _tiles.Length);
                var rotationIndex = Random.Range(0, _rotationValues.Length);
                _mapTiles[i, k] = Instantiate(_tiles[index], new Vector3(xPos, 0, zPos), Quaternion.Euler(0, _rotationValues[rotationIndex], 0));
                _mapTiles[i, k].gameObject.name = $"{i} {k}";
                xPos += 5;
            }
        }

        StartCoroutine(HumanGenerator());
    }

    IEnumerator HumanGenerator()
    {
        yield return new WaitForSeconds(0.1f);

        if (_human != null)
        {
            Destroy(_human.gameObject);
        }

        var point = _mapTiles[0, 1].TilePoints[0];
        var pos = new Vector3(point.transform.position.x, point.transform.position.y + 0.1f, point.transform.position.z);
        _human = Instantiate(_humanPrefab, pos, Quaternion.identity);
        _human.CurrentMapTile = _mapTiles[0, 1];
    }
}
