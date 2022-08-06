using UnityEngine;

/// <summary>�X�̐l�𓮂����N���X</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")] float _move = 1f;
    [SerializeField, Tooltip("�ʒu���B�ƌ��Ȃ�����")] float _goalDistance = 0.6f;
    [SerializeField, Tooltip("�X�̐l������Ă���^�C��")] MapTile _currentMapTile;
    /// <summary>�Ō�ɐG�ꂽ�|�C���g </summary>
    RoadPoint _lastRoadPoint = default;
    /// <summary>�^�C���̃|�C���g�ʒu</summary>
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
                distance = Vector3.Distance(transform.position, _currentMapTile.TilePoints[_currentIndex].transform.position);  //�S�[���܂ł̋������v�Z����
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
                    Debug.Log("���B");
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

    /// <summary>�ݒ肵���ʒu�Ɉړ�����</summary>
    void SetMovePosition()
    {
        var dir = _currentMapTile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _rb.velocity = transform.forward * _move;
    }
}
