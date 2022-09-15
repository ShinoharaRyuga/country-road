using System.Collections.Generic;
using UnityEngine;

/// <summary>�X�̐l�𓮂����N���X</summary>
public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")] float _moveSpeed = 1f;
    [SerializeField, Tooltip("���ݏ���Ă���^�C��")] TileBase _currentTile;
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

            ////���݂̃^�C�����甲����
            if (_hitCount == 3)
            {
                _hitCount = 0;
                _hitPoints.Clear();

                var nextTile = roadPoint.ParentMapTile.GetNextTile(roadPoint.CurrentStatus);

                if (nextTile != null)   //���̃^�C�����q�����Ă���ΐi��
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

            //���̃|�C���g�ɐi��
            var nextPoint = roadPoint.ParentMapTile.GetNextPoint(transform, _hitPoints);
            SetMoveDirection(nextPoint);
        }
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

    /// <summary>�Q�[���J�n����ԍŏ��Ɍ������|�C���g���擾���� </summary>
    void SetFirstPoint()
    {
        var nextPoint = _currentTile.GetFirstPoint(transform);
        SetMoveDirection(nextPoint);
        _isMoving = true;
    }
}
