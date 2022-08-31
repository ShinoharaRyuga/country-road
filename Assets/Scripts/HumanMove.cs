using System.Collections;
using UnityEngine;

/// <summary>街の人を動かすクラス</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("位置到達と見なす距離")] float _goalDistance = 0.6f;
    [SerializeField, Tooltip("街の人が乗っているタイル")] MapTile _currentMapTile;
    /// <summary>タイル進入時最初に衝突したポイントステータス </summary>
    PointStatus _enterPointStatus = PointStatus.None;
    /// <summary>タイルから退出する時に衝突するポイント </summary>
    PointStatus _exitPointStatus = PointStatus.None;
    /// <summary>タイルのポイント位置</summary>
    int _currentIndex = 0;
    bool _isMoving = false;

    Rigidbody _rb => GetComponent<Rigidbody>();
    public MapTile CurrentMapTile { get => _currentMapTile; set => _currentMapTile = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    void Update()
    {
        if (_isMoving)
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log(CurrentMapTile);
            Debug.Log("スタート" + _currentMapTile.StartConnectionTile);
            Debug.Log("エンド" + _currentMapTile.EndConnectionTile);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("移動");
            ChackTileConnection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RoadPoint roadPoint))
        {
            var hitTile = roadPoint.ParentMapTile;

            if (hitTile == _currentMapTile && _exitPointStatus == roadPoint.CurrentStatus)    //タイルの終了位置に到着
            {
                StartCoroutine(ArrivalEndPoint());
                return;
            }

            if (roadPoint.CurrentStatus != PointStatus.Middle)
            {
                _enterPointStatus = roadPoint.CurrentStatus;
                SetExitPoint();
            }

            StartCoroutine(SetDestination());
        }
    }

    /// <summary>
    /// タイルが繋がっているか調べて
    /// 繋がってたら移動する
    /// </summary>
    public void ChackTileConnection()
    {
        if (_exitPointStatus == PointStatus.Start && _currentMapTile.StartConnectionTile != null)
        {
            _currentMapTile = _currentMapTile.StartConnectionTile;
            SetEnterPoint();
        }
        else if (_exitPointStatus == PointStatus.End && _currentMapTile.EndConnectionTile != null)
        {
            _currentMapTile = _currentMapTile.EndConnectionTile;
            SetEnterPoint();
        }
    }


    /// <summary>タイルから出る時に衝突するポイントステータスを設定する </summary>
    void SetExitPoint()
    {
        if (_enterPointStatus == PointStatus.Start)
        {
            _exitPointStatus = PointStatus.End;
        }
        else
        {
            _exitPointStatus = PointStatus.Start;
            _currentIndex = _currentMapTile.TilePoints.Count - 1;
        }
    }

    /// <summary>目的地を決める</summary>
    IEnumerator SetDestination()
    {
        yield return new WaitForSeconds(0.5f);

        if (_enterPointStatus == PointStatus.Start)
        {
            if (_currentIndex < _currentMapTile.TilePoints.Count - 1)   //startからendポイントに向かう
            {
                _currentIndex++;
                SetMoveDirection();
            }
        }
        else if (_enterPointStatus == PointStatus.End)
        {
            if (0 < _currentIndex)
            {
                _currentIndex--;
                SetMoveDirection();
            }
        }
    }

    /// <summary>タイルから退出する時の処理 </summary>
    IEnumerator ArrivalEndPoint()
    {
        yield return new WaitForSeconds(0.5f);
        _isMoving = false;
        _rb.velocity = Vector3.zero;

        ChackTileConnection();
    }

   
    /// <summary>進入ポイントを決める </summary>
    void SetEnterPoint()
    {
        var point = _currentMapTile.GetNearRoadPoint(transform.position);

        if (point == PointStatus.Start)
        {
            _currentIndex = 0;
            _enterPointStatus = PointStatus.Start;
        }
        else if (point == PointStatus.End)
        {
            _currentIndex = _currentMapTile.TilePoints.Count - 1;
            _enterPointStatus = PointStatus.End;
        }

        //移動処理
        SetExitPoint();
        SetMoveDirection();
    }

    /// <summary>移動処理 </summary>
    void SetMoveDirection()
    {
        var dir = _currentMapTile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _isMoving = true;
    }
}
