using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�^�C�����Ǘ�����N���X </summary>
public class MapTile : MonoBehaviour
{
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

    void Start()
    {
        //for (var i = 0; i < transform.childCount; i++)  //�e�|�C���g���擾���A���X�g���쐬����
        //{
        //    var roadPoint = transform.GetChild(i).gameObject.GetComponent<RoadPoint>();
        //    var nextStatus = i % Enum.GetValues(typeof(PointStatus)).Length;
        //    roadPoint.PointStatus = (PointStatus)nextStatus;
        //    _tilePoints.Add(roadPoint);
        //}
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
