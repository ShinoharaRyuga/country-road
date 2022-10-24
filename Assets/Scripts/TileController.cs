using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C���̊��N���X
/// �@T���H��Ȃ���p�Ȃǂ�h�������č쐬����  
/// </summary>
public class TileController : MonoBehaviour
{
    /// <summary>�}�b�v�쐬���̈ړ����x </summary>
    const float FIRST_MOVE_SPEED = 70f;

    [SerializeField, Header("�^�C���̈ʒu���")] TilePoint _tilePoint = TilePoint.None;
    [SerializeField, Header("�^�C���̎��")] TileType _tileType = TileType.None;
    [SerializeField, HideInInspector ,Tooltip("���̎��")] RoadType _roadType = RoadType.Straight;
    [SerializeField, HideInInspector ,Tooltip("�����̎��")] BuildingType _buildingType = BuildingType.House1;
    [SerializeField, HideInInspector ,Tooltip("�����^�C���̎��")] NoneType _noneType = NoneType.Bench;
   
    AstarStatus _astarStatus = AstarStatus.Empty;
    /// <summary>�^�C���ɏ���Ă���X�l�̃��X�g </summary>
    List<HumanMove> _onHumans = new List<HumanMove>();
    /// <summary>�X�l��U�����邽�߃|�C���g���X�g </summary>
    List<RoadPoint> _roadPoints = new List<RoadPoint>();
    /// <summary>�q�����Ă���^�C�� </summary>
    Dictionary<PointStatus, TileController> _connectingTiles = new Dictionary<PointStatus, TileController>();

    Pathfinding _pathfinding;
    int _row = 0;
    int _col = 0;
    int _realCost = 0;
    int _guessCost = 0;
    int _score = 0;
    public List<HumanMove> OnHumans { get => _onHumans; set => _onHumans = value; }
    public Dictionary<PointStatus, TileController> ConnectingTiles { get => _connectingTiles; set => _connectingTiles = value; }
    public TileType TileType { get => _tileType; }
    public int Col { get => _col; }
    public int Row { get => _row; }
    public int RealCost { get => _realCost; set => _realCost = value; }
    public int GuessCost { get => _guessCost; set => _guessCost = value; }
    public int Score { get => _score; set => _score = value; }
    public AstarStatus AstarStatus { get => _astarStatus; set => _astarStatus = value; }
    public Pathfinding PathfindingClass { get => _pathfinding; set => _pathfinding = value; }
    public RoadType RoadType { get => _roadType; set => _roadType = value; }
    public BuildingType BuildingType { get => _buildingType; set => _buildingType = value; }
    public NoneType NoneType { get => _noneType; set => _noneType = value; }

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
    public void AddConnectedTile(PointStatus key, TileController tile)
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

    public Vector3 GetNextPoint(TileController nextTile, List<RoadPoint> hitPoints)
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

        nextPoint = targetPoints.OrderBy(p => Vector3.Distance(nextTile.transform.position, p.transform.position)).FirstOrDefault();

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
    public TileController GetNextTile(PointStatus key)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            return _connectingTiles[key];
        }

        return null;
    }

    public TileController GetNextTile(TileController lastTile)
    {                                       
        var nextTile = _connectingTiles[0]; 
        var minGuessCost = 99;              
    
        foreach (var tile in _connectingTiles)
        {
            if (tile.Value != null && tile.Value != lastTile)
            {
                var targetTile = tile.Value;
                var targetGuessCost = targetTile.GetGuessCost(_pathfinding.GoalRow, _pathfinding.GoalCol);

                if (targetGuessCost < minGuessCost)
                {
                    nextTile = targetTile;
                    minGuessCost = targetGuessCost;
                }
            }
        }

        return nextTile;
    }

    int GetGuessCost(int goalRow, int goalCol)
    {
        var disR = goalRow - _row;
        var disC = goalCol - _col;

        return disR + disC;
    }

    /// <summary>�ʒu����ݒ肷�� </summary>
    public void SetPoint(int r, int c)
    {
        _row = r;
        _col = c;
    }

    /// <summary>�o�H�T���Ŏg�p����R�X�g���v�Z���� </summary>
    /// <param name="realCost">���R�X�g</param>
    /// <param name="guessCost">����R�X�g</param>
    public void SetCosts(int realCost, int guessCost)
    {
        _realCost = realCost;
        _guessCost = guessCost;
        _score = _realCost + guessCost;
    }

    /// <summary>�X�l���i�����Ă����烊�X�g�ɒǉ����� </summary>
    public void AddHuman(HumanMove human)
    {
        if (!_onHumans.Contains(human))
        {
            _onHumans.Add(human);
        }
    }

    /// <summary>�X�l���������烊�X�g����폜���� </summary>
    public void RemoveHuman(HumanMove human)
    {
        if (_onHumans.Contains(human))
        {
            _onHumans.Remove(human);
        }
    }

    /// <summary>�e�R���C�_�[���A�N�e�B�u��Ԃɂ���</summary>
    public void ActiveCollider()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.gameObject.TryGetComponent(out Collider collider))
            {
                collider.enabled = true;
            }
        }
    }

    /// <summary>
    /// �^�C���ɏ���Ă��Ĉړ������Ă��Ȃ��X�l��
    /// �Ăшړ�������ׂ̊֐� 
    /// </summary>
    public void StartHumanMove()
    {
        var movedHumans = new List<HumanMove>();

        foreach (var human in _onHumans)
        {
            human.SetFirstPoint();
            movedHumans.Add(human);
        }

        //�ړ������X�l�����X�g����폜����
        foreach (var movedHuman in movedHumans)
        {
            _onHumans.Remove(movedHuman);
        }
    }

    /// <summary>�}�b�v�쐬���̈ړ����� </summary>
    /// <returns></returns>
    public IEnumerator StartMove()
    {
        var startPosition = transform.position;
        var endPosition = new Vector3(transform.position.x, 0, transform.position.z);
        var distance = Vector3.Distance(startPosition, endPosition);
        var time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            var present_Location = (time * FIRST_MOVE_SPEED) / distance;

            transform.position = Vector3.Lerp(startPosition, endPosition, present_Location);

            if (transform.position == endPosition)
            {
                break;
            }

            yield return null;
        }
    }
}


