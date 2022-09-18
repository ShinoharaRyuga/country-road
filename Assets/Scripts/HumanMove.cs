using System.Collections.Generic;
using UnityEngine;

/// <summary>�X�̐l�𓮂����N���X</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("���ݏ���Ă���^�C��")] TileBase _currentTile;
    List<RoadPoint> _hitPoints = new List<RoadPoint>();
    /// <summary>�O�ɏ���Ă����^�C�� </summary>
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
            if (roadPoint.CurrentStatus == PointStatus.Goal)    //�S�[���ɓ��������炻�̏�~�܂�
            {
                Debug.Log("Goal");
                MoveStop();
                return;
            }

            _hitCount++;
            _hitPoints.Add(roadPoint);
         
            if (_hitCount == 1) //���̃^�C���ɐi��
            {
                _currentTile.AddHuman(this);
            }

           
            if (_hitCount == 3) //���݂̃^�C�����甲����
            {
                _hitCount = 0;
                _hitPoints.Clear();

                var nextTile = roadPoint.ParentMapTile.GetNextTile(roadPoint.CurrentStatus);

                if (nextTile != null)   //���̃^�C�����q�����Ă���ΐi��
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

            //���̃|�C���g�ɐi��
            var nextPoint = roadPoint.ParentMapTile.GetNextPoint(transform, _hitPoints);
            SetMoveDirection(nextPoint);
        }
    }

    /// <summary>�X�l���~���� </summary>
    public void MoveStop()
    {
        _isMoving = false;
        _rb.velocity = Vector3.zero;
    }

    /// <summary>���̃|�C���g�̕����������� </summary>
    /// <param name="nextPoint">���ɐi�ރ|�C���g</param>
    void SetMoveDirection(Vector3 nextPoint)
    {
        var dir = nextPoint - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
    }

    /// <summary>�X�l�̈ړ����J�n������ׂ̊֐� </summary>
    public void SetFirstPoint()
    {
        var nextPoint = _currentTile.GetFirstPoint(transform);
        SetMoveDirection(nextPoint);
        _isMoving = true;
    }
}
