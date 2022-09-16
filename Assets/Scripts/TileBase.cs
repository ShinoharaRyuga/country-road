using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C���̊��N���X
/// �@T���H��Ȃ���p�Ȃǂ�h�������č쐬����  
/// </summary>
public class TileBase : MonoBehaviour
{
    [SerializeField] TileStatus _currnetStatus = TileStatus.None;

    /// <summary>�^�C���ɏ���Ă���X�l�̃��X�g </summary>
    List<HumanMove> _onHumans = new List<HumanMove>();
    /// <summary>�X�l��U�����邽�߃|�C���g���X�g </summary>
    List<RoadPoint> _roadPoints = new List<RoadPoint>();
    /// <summary>�q�����Ă���^�C�� </summary>
    Dictionary<PointStatus, TileBase> _connectingTiles = new Dictionary<PointStatus, TileBase>();
    int _row = 0;
    int _col = 0;
    public List<HumanMove> OnHumans { get => _onHumans; set => _onHumans = value; }
    public Dictionary<PointStatus, TileBase> ConnectingTiles { get => _connectingTiles; set => _connectingTiles = value; }
    public TileStatus CurrnetStatus { get => _currnetStatus; set => _currnetStatus = value; }
    public int Col { get => _col; }
    public int Row { get => _row; }

    void Start()
    {
        //�e�o����ƌq�����Ă���^�C�������т���
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.TryGetComponent(out ConnectingTiles tiles))
            {
                if (!tiles.IsMiddle)
                {
                    _connectingTiles.Add(tiles.CurrentStatus, null);
                }
            }

            if (child.TryGetComponent(out RoadPoint point))
            {
                _roadPoints.Add(point);
            }
        }
    }

    /// <summary>�ڑ����ꂽ�^�C����_connectingTiles�ɒǉ����� </summary>
    public void AddConnectedTile(PointStatus key, TileBase tile)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            _connectingTiles[key] = tile;
        }
    }

    /// <summary>�v���C���[�ɍł��߂��|�C���g��Ԃ�</summary>
    public Vector3 GetNextPoint(Transform player, List<RoadPoint> hitPoints)
    {
        var targetPoints = new List<RoadPoint>();
        var nextPoint = _roadPoints[0];

        foreach (var point in _roadPoints)
        {
            if (!hitPoints.Contains(point))
            {
                targetPoints.Add(point);
            }
        }

        nextPoint = targetPoints.OrderBy(p => Vector3.Distance(player.position, p.transform.position)).FirstOrDefault();

        return nextPoint.transform.position;
    }

    /// <summary>�Q�[���J�n����ԍŏ��Ɍ������|�C���g���擾���� </summary>
    public Vector3 GetFirstPoint(Transform player)
    {
        var firstPoint = _roadPoints.OrderBy(p => Vector3.Distance(player.position, p.transform.position)).FirstOrDefault();

        return firstPoint.transform.position;
    }

    /// <summary>�w�肳�ꂽ�o���Ƀ^�C�����q�����Ă��邩���ׂ� </summary>
    /// <param name="key">�w�肵���o��</param>
    /// <returns>�q�����Ă����炻�̃^�C����Ԃ�</returns>
    public TileBase GetNextTile(PointStatus key)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            return _connectingTiles[key];
        }

        return null;
    }

    public void SetPoint(int r, int c)
    {
        _row = r;
        _col = c;
    }
}

public enum PointStatus
{
    First = 0,
    Second = 1,
    Third = 2,
    Fourth = 3,
    None = 4,
    Middle = 5,
    Goal = 6,
    Start = 7,
}

public enum TileStatus
{
    None = 0,
    Straight = 1,
    Side = 2,
    Corner = 3,
    TRoad = 4,
    Cross = 5,
    Start = 6,
    Goal = 7,
}
