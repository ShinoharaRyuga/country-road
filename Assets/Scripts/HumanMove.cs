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
    int _currentIndex = 1;
    bool _isMoving = false;

    Rigidbody _rb => GetComponent<Rigidbody>();
    public MapTile CurrentMapTile { get => _currentMapTile; set => _currentMapTile = value; }
    public MapTile LastMapTile { get => _lastMapTile; set => _lastMapTile = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    void Update()
    {
        var distance = 0f;

        if (Input.GetButtonDown("Jump"))
        {
            SetMovePosition();
            Debug.Log("目的地を設定");
        }

        if (_isMoving)
        {
            distance = Vector3.Distance(transform.position, _currentMapTile.TilePoints[_currentIndex].transform.position);  //ゴールまでの距離を計算する
            _rb.velocity = transform.forward * _moveSpeed;

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
                    _isMoving = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log($"current{_currentMapTile}");
            Debug.Log($"last{_lastMapTile}");
        }
    }

    public void SetNextTile()
    {
        if (_currentMapTile.EndConnectionTile != null)
        {
            _currentMapTile = _currentMapTile.EndConnectionTile;
            _isMoving = true;
            Debug.Log("ロード");
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
