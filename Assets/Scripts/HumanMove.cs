using UnityEngine;

/// <summary>�X�̐l�𓮂����N���X</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("�ʒu���B�ƌ��Ȃ�����")] float _goalDistance = 0.6f;
    [SerializeField, Tooltip("�X�̐l������Ă���^�C��")] MapTile _currentMapTile;
    /// <summary>�Ō�ɏ���Ă����^�C��</summary>
    MapTile _lastMapTile = default;
    /// <summary>�^�C���̃|�C���g�ʒu</summary>
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
            Debug.Log("�ړI�n��ݒ�");
        }

        if (_isMoving)
        {
            distance = Vector3.Distance(transform.position, _currentMapTile.TilePoints[_currentIndex].transform.position);  //�S�[���܂ł̋������v�Z����
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
                    Debug.Log("���B");
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
            Debug.Log("���[�h");
        }
    }

    /// <summary>�ړI�n�����߂�</summary>
    void SetMovePosition()
    {
        var dir = _currentMapTile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _isMoving = true;
    }
}
