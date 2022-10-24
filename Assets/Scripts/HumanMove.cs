using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�X�̐l�𓮂����N���X</summary>
public class HumanMove : MonoBehaviour
{
    /// <summary>��ԍŏ��ɓ����o���܂ł̎��� </summary>
    const float FIRST_WAIT_TIME = 0.5f;

    [SerializeField, Tooltip("�ړ����x")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("���ݏ���Ă���^�C��")] TileController _currentTile;
    [SerializeField, Tooltip("�ړI�n(�S�[��)")] Destination _currentDestination = Destination.None;
    List<RoadPoint> _hitPoints = new List<RoadPoint>();
    /// <summary>�O�ɏ���Ă����^�C�� </summary>
    TileController _lastTile = default;
    int _hitCount = 0;
    bool _isMoving = false;
    TileController _startTile = default;
    TileController _goalTile = default;
    Rigidbody _rb => GetComponent<Rigidbody>();
    Animator _anime => GetComponent<Animator>();
    public TileController CurrentTile { get => _currentTile; set => _currentTile = value; }
    /// <summary>�ړI�n </summary>
    public Destination CurrentDestination { get => _currentDestination; set => _currentDestination = value; }
    public TileController StartTile { get => _startTile; set => _startTile = value; }
    public TileController GoalTile { get => _goalTile; set => _goalTile = value; }

    private void Start()
    {
        StartCoroutine(FirstMove());
    }

    private void Update()
    {
        if (_isMoving)
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }

        _anime.SetFloat("Speed", _rb.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RoadPoint roadPoint))
        {
            //if (roadPoint.ParentMapTile.CurrnetStatus == TileID.Start) { return; }

            _hitCount++;
            _hitPoints.Add(roadPoint);

            if (_hitCount == 1) //���̃^�C���ɐi��
            {
                _currentTile = roadPoint.ParentMapTile;
                _currentTile.AddHuman(this);
            }

            if (_hitCount == 2)
            {
                if (_currentTile.ConnectingTiles.Count <= 2)
                {
                    var nextPoint = _currentTile.GetNextPoint(transform, _hitPoints);
                    SetMoveDirection(nextPoint);
                }
                else
                {
                    var nextTile = _currentTile.GetNextTile(_lastTile);
                    var nextPoint = _currentTile.GetNextPoint(nextTile, _hitPoints);
                    SetMoveDirection(nextPoint);
                }
            }


            if (_hitCount == 3) //���݂̃^�C�����甲����
            {
                _hitCount = 0;
                _hitPoints.Clear();
               
                var nextTile = roadPoint.ParentMapTile.GetNextTile(roadPoint.CurrentStatus);
                
                if (nextTile != null)   //���̃^�C�����q�����Ă���ΐi��
                {
                    var nextPoint = nextTile.GetFirstPoint(transform);
                    SetMoveDirection(nextPoint);
                    _currentTile.RemoveHuman(this);
                    _lastTile = _currentTile;
                }
                else
                {
                    MoveStop();
                }

                return;
            }

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
        var nextTile = _currentTile.GetNextTile(_lastTile);
        var nextPoint = nextTile.GetFirstPoint(transform);
        SetMoveDirection(nextPoint);
        _isMoving = true;
    }

    /// <summary>��ԍŏ��ɓ�����������֐� </summary>
    IEnumerator FirstMove()
    {
        yield return new WaitForSeconds(FIRST_WAIT_TIME);
        SetFirstPoint();
    }
}

public enum Destination
{
    None,
    School,
    Library,
}
