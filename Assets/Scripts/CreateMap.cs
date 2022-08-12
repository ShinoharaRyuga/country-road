using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField] int _rows = 3;
    [SerializeField] int _columns = 3;
    [SerializeField] MapTile[] _tiles = default;

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
                var index = Random.Range(0, _tiles.Length);
                var rotationIndex = Random.Range(0, _rotationValues.Length);
                _mapTiles[i, k] = Instantiate(_tiles[index], new Vector3(xPos, 0, zPos), Quaternion.Euler(0, _rotationValues[rotationIndex], 0));
                xPos += 5;
            }
        }
    }
}
