using System.Collections.Generic;
using UnityEngine;

/// <summary>街の人を動かすクラス</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("現在乗っているタイル")] TileBase _currentTile;
    List<RoadPoint> _hitPoints = new List<RoadPoint>();
    /// <summary>前に乗っていたタイル </summary>
    TileBase _lastTile = default;
    int _hitCount = 0;
    bool _isMoving = false;
    Rigidbody _rb => GetComponent<Rigidbody>();
    Animator _anime => GetComponent<Animator>();
    RouteSearch routeSearch => GetComponent<RouteSearch>();

    public TileBase CurrentTile { get => _currentTile; set => _currentTile = value; }
    public TileBase LastTile { get => _lastTile; set => _lastTile = value; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
          //  routeSearch.CheckRoute(_currentTile);
        }

        if (_isMoving)
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            SetFirstPoint();
        }

        _anime.SetFloat("Speed", _rb.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RoadPoint roadPoint))
        {
            if (roadPoint.CurrentStatus == PointStatus.Goal)    //ゴールに到着したらその場止まる
            {
                Debug.Log("Goal");
                MoveStop();
                return;
            }

            _hitCount++;
            _hitPoints.Add(roadPoint);
         
            if (_hitCount == 1) //次のタイルに進入
            {
                _currentTile.AddHuman(this);
            }

           
            if (_hitCount == 3) //現在のタイルから抜ける
            {
                _hitCount = 0;
                _hitPoints.Clear();

                var nextTile = roadPoint.ParentMapTile.GetNextTile(roadPoint.CurrentStatus);

                if (nextTile != null)   //次のタイルが繋がっていれば進む
                {
                    _currentTile.RemoveHuman(this);
                    _lastTile = _currentTile;
                    _currentTile = nextTile;
                    SetFirstPoint();
                }
                else
                {
                    MoveStop();
                }

                return;
            }

            //次のポイントに進む
            var nextPoint = roadPoint.ParentMapTile.GetNextPoint(transform, _hitPoints);
            SetMoveDirection(nextPoint);
        }
    }

    /// <summary>街人を停止する </summary>
    public void MoveStop()
    {
        _isMoving = false;
        _rb.velocity = Vector3.zero;
    }

    /// <summary>次のポイントの方を向かせる </summary>
    /// <param name="nextPoint">次に進むポイント</param>
    void SetMoveDirection(Vector3 nextPoint)
    {
        var dir = nextPoint - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
    }

    /// <summary>街人の移動を開始させる為の関数 </summary>
    public void SetFirstPoint()
    {
        var nextPoint = _currentTile.GetFirstPoint(transform);
        SetMoveDirection(nextPoint);
        _isMoving = true;
    }
}
