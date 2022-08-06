using UnityEngine;

/// <summary>街の人を動かすクラス</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")] float _move = 1f;
    [SerializeField, Tooltip("位置到達と見なす距離")] float _goalDistance = 0.6f;
    [SerializeField, Tooltip("街の人が乗っているタイル")] MapTile _currentMapTile;
    /// <summary>最後に触れたポイント </summary>
    RoadPoint _lastRoadPoint = default;
    /// <summary>タイルのポイント位置</summary>
    int _currentIndex = 1;
    bool _isMoving = true;

    Rigidbody _rb => GetComponent<Rigidbody>();
    public MapTile CurrentMapTile { get => _currentMapTile; set => _currentMapTile = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }
    public RoadPoint LastRoadPoint { get => _lastRoadPoint; set => _lastRoadPoint = value; }

    void Update()
    {
        var distance = 0f;

        if (_isMoving && _currentMapTile)
        {
             if (_currentIndex < _currentMapTile.TilePoints.Count)
            {
                distance = Vector3.Distance(transform.position, _currentMapTile.TilePoints[_currentIndex].transform.position);  //ゴールまでの距離を計算する
            }

            if (Input.GetButtonDown("Jump"))
            {
                SetMovePosition();
            }

            if (distance <= _goalDistance)
            {
                _rb.velocity = Vector3.zero;
                _currentIndex++;

                if (_currentIndex < _currentMapTile.TilePoints.Count)
                {
                    SetMovePosition();
                }
                else
                {
                    Debug.Log("到達");
                    _currentIndex = 0;
                    _currentMapTile = _currentMapTile.EndConnectionTile;
                    Debug.Log(_currentMapTile);
                    //  SetMovePosition();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(_currentMapTile);
        }
    }

    public bool CheckPoints(RoadPoint targetPoint)
    {
        foreach (var point in _currentMapTile.TilePoints)
        {
            if (targetPoint == point)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>設定した位置に移動する</summary>
    void SetMovePosition()
    {
        var dir = _currentMapTile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _rb.velocity = transform.forward * _move;
    }
}
