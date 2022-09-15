using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>�^�C�����Ǘ�����N���X </summary>
public class MapTile : MonoBehaviour
{
    /// <summary>�^�C���ɏ���Ă���X�̐l���X�g </summary>
    List<HumanMove> _humans = new List<HumanMove>();
    /// <summary>�^�C���̒��S�Əo�����̈ʒu </summary>
    List<RoadPoint> _tilePoints = new List<RoadPoint>();
    /// <summary>�X�^�[�g�|�C���g�ƌq�����Ă���^�C�� </summary>
    MapTile _startConnectionTile = default;
    /// <summary>�G���h�|�C���g�ƌq�����Ă���^�C�� </summary>
    MapTile _endConnectionTile = default;
    /// <summary>�^�C���̒��S�Əo�����̈ʒu </summary>
    public List<RoadPoint> TilePoints { get => _tilePoints; }
    /// <summary>�X�^�[�g�|�C���g�ƌq�����Ă���^�C�� </summary>
    public MapTile StartConnectionTile { get => _startConnectionTile; set => _startConnectionTile = value; }
    /// <summary>�G���h�|�C���g�ƌq�����Ă���^�C�� </summary>
    public MapTile EndConnectionTile { get => _endConnectionTile; set => _endConnectionTile = value; }
    public List<HumanMove> Humans { get => _humans; set => _humans = value; }

    void Start()
    {
        for (var i = 0; i < transform.childCount; i++)  //�e�|�C���g���擾���A���X�g���쐬����
        {
            if (transform.GetChild(i).TryGetComponent<RoadPoint>(out var roadPoint))
            {
                var nextStatus = i % Enum.GetValues(typeof(PointStatus)).Length;
                roadPoint.CurrentStatus = (PointStatus)nextStatus;
                _tilePoints.Add(roadPoint);
            }
        }
    }

    /// <summary> �ł��߂��|�C���g�X�e�[�^�X��Ԃ� </summary>
    public PointStatus GetNearRoadPoint(Vector3 playerPosition)
    {
        var nearPoint = _tilePoints.OrderBy(x => Vector3.Distance(playerPosition, x.transform.position)).FirstOrDefault();
        return nearPoint.CurrentStatus;
    }

    /// <summary>
    /// �^�C���ɐi�����ė����l�����X�g�ɓ����Ă��邩���ׂ�
    /// �������s��
    /// </summary>
    public void CheckHumans(HumanMove human, PointStatus status)
    {
        var nextTile = _startConnectionTile;

        if (status == PointStatus.End)
        {
            nextTile = _endConnectionTile;
        }

        if (_humans.Contains(human) && nextTile != null)
        {
            _humans.Remove(human);
        }
        else if (!_humans.Contains(human))
        {
            _humans.Add(human);
        }
    }

    /// <summary>�^�C���łƂǂ܂��Ă����X�l�B���Ăшړ�������</summary>
    public void AgainMove()
    {
        //�폜����InvalidOperationException���o���Ȃ��悤�ɂ��邽��
        var moveHumans = new List<HumanMove>();

        foreach (var human in _humans)
        {
         //   human.ChackTileConnection();
            moveHumans.Add(human);
        }

        //�ړ������X�l���폜
        foreach (var target in moveHumans)
        {
            _humans.Remove(target);
        }
    }

    /// <summary>
    /// �X�̐l���G���h�|�C���g����i�����Ă�����
    /// �X�^�[�g�|�C���g�ƃG���h�|�C���g�����ւ���
    /// </summary>
    public void Swap()
    {
        var tmp = _tilePoints[0];
        _tilePoints[0] = _tilePoints[2];
        _tilePoints[2] = tmp;
    }
}
