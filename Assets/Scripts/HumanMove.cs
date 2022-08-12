using UnityEngine;

/// <summary>街の人を動かすクラス</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("位置到達と見なす距離")] float _goalDistance = 0.6f;
    [SerializeField, Tooltip("街の人が乗っているタイル")] MapTile _currentMapTile;
    /// <summary>最後に乗っていたタイル</summary>
    MapTile _lastMapTile = default;
    /// <summary>タイルのポイント位置</summary>
    int _currentIndex = 0;
    bool _isMoving = false;

    Rigidbody _rb => GetComponent<Rigidbody>();
    public MapTile CurrentMapTile { get => _currentMapTile; set => _currentMapTile = value; }
    public MapTile LastMapTile { get => _lastMapTile; set => _lastMapTile = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    void Update()
    {
        var distance = 0f;

        if (Input.GetKeyDown(KeyCode.B))
        {
            SetMovePosition();
        }

        if (_isMoving)
        {
            if (_currentIndex < _currentMapTile.TilePoints.Count)
            {
                distance = Vector3.Distance(transform.position, _currentMapTile.TilePoints[_currentIndex].transform.position);  //ゴールまでの距離を計算する
            }
           
            if (distance <= _goalDistance)
            {
               
                if (_currentIndex < _currentMapTile.TilePoints.Count - 1)
                {
                    _currentIndex++;
                    SetMovePosition();
                }
                else
                {
                    _currentIndex = 0;
                    _rb.velocity = Vector3.zero;
                }
            }
            else
            {
                _rb.velocity = transform.forward * _moveSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log($"current{_currentMapTile}");
            Debug.Log($"last{_lastMapTile}");
            Debug.Log($"point{_currentMapTile.TilePoints[_currentIndex].name}");
        }
    }

    public void SetNextTile(PointStatus status)
    {

        Debug.Log("呼ばれた");

        if (status == PointStatus.Start)
        {
            if (_currentMapTile.StartConnectionTile != null)
            {
                _currentMapTile = _currentMapTile.StartConnectionTile;
                _isMoving = true;
                Debug.Log("進行");
            }
            else
            {
                _isMoving = false;
                _rb.velocity = Vector3.zero;
                Debug.Log("Start止める");
            }
        }
        else if (status == PointStatus.End)
        {
            if (_currentMapTile.EndConnectionTile != null)
            {
                _currentMapTile = _currentMapTile.EndConnectionTile;
                _isMoving = true;
                Debug.Log("進行");
            }
            else
            {
                _isMoving = false;
                _rb.velocity = Vector3.zero;
                Debug.Log("End止める");
            }
        }
    }

    /// <summary>目的地を決める</summary>
    void SetMovePosition()
    {
        var dir = _currentMapTile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _isMoving = true;
    }
}
