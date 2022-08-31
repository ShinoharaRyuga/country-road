using System.Collections;
using UnityEngine;

/// <summary>�X�̐l�𓮂����N���X</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("�ʒu���B�ƌ��Ȃ�����")] float _goalDistance = 0.6f;
    [SerializeField, Tooltip("�X�̐l������Ă���^�C��")] MapTile _currentMapTile;
    /// <summary>�^�C���i�����ŏ��ɏՓ˂����|�C���g�X�e�[�^�X </summary>
    PointStatus _enterPointStatus = PointStatus.None;
    /// <summary>�^�C������ޏo���鎞�ɏՓ˂���|�C���g </summary>
    PointStatus _exitPointStatus = PointStatus.None;
    /// <summary>�^�C���̃|�C���g�ʒu</summary>
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
            Debug.Log("�X�^�[�g" + _currentMapTile.StartConnectionTile);
            Debug.Log("�G���h" + _currentMapTile.EndConnectionTile);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("�ړ�");
            ChackTileConnection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RoadPoint roadPoint))
        {
            var hitTile = roadPoint.ParentMapTile;

            if (hitTile == _currentMapTile && _exitPointStatus == roadPoint.CurrentStatus)    //�^�C���̏I���ʒu�ɓ���
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
    /// �^�C�����q�����Ă��邩���ׂ�
    /// �q�����Ă���ړ�����
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


    /// <summary>�^�C������o�鎞�ɏՓ˂���|�C���g�X�e�[�^�X��ݒ肷�� </summary>
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

    /// <summary>�ړI�n�����߂�</summary>
    IEnumerator SetDestination()
    {
        yield return new WaitForSeconds(0.5f);

        if (_enterPointStatus == PointStatus.Start)
        {
            if (_currentIndex < _currentMapTile.TilePoints.Count - 1)   //start����end�|�C���g�Ɍ�����
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

    /// <summary>�^�C������ޏo���鎞�̏��� </summary>
    IEnumerator ArrivalEndPoint()
    {
        yield return new WaitForSeconds(0.5f);
        _isMoving = false;
        _rb.velocity = Vector3.zero;

        ChackTileConnection();
    }

   
    /// <summary>�i���|�C���g�����߂� </summary>
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

        //�ړ�����
        SetExitPoint();
        SetMoveDirection();
    }

    /// <summary>�ړ����� </summary>
    void SetMoveDirection()
    {
        var dir = _currentMapTile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _isMoving = true;
    }
}
