using System.Collections.Generic;
using UnityEngine;

/// <summary>街の人を動かすクラス</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("現在乗っているタイル")] TileBase _currentTile;
    List<RoadPoint> _hitPoints = new List<RoadPoint>();
    int _hitCount = 0;
    bool _isMoving = false;
    Rigidbody _rb => GetComponent<Rigidbody>();

    private void Update()
    {
        if (_isMoving)
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            SetFirstPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RoadPoint roadPoint))
        {
            _hitCount++;
            _hitPoints.Add(roadPoint);

            ////現在のタイルから抜ける
            if (_hitCount == 3)
            {
                _hitCount = 0;
                _hitPoints.Clear();

                var nextTile = roadPoint.ParentMapTile.GetNextTile(roadPoint.CurrentStatus);

                if (nextTile != null)   //次のタイルが繋がっていれば進む
                {
                    _currentTile = nextTile;
                    SetFirstPoint();
                }
                else
                {
                    _isMoving = false;
                    _rb.velocity = Vector3.zero;
                }

                return;
            }

            //次のポイントに進む
            var nextPoint = roadPoint.ParentMapTile.GetNextPoint(transform, _hitPoints);
            SetMoveDirection(nextPoint);
        }
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

    /// <summary>ゲーム開始時一番最初に向かうポイントを取得する </summary>
    void SetFirstPoint()
    {
        var nextPoint = _currentTile.GetFirstPoint(transform);
        SetMoveDirection(nextPoint);
        _isMoving = true;
    }
}
