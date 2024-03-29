using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイルの基底クラス
/// 　T字路や曲がり角などを派生させて作成する  
/// </summary>
public class TileController : MonoBehaviour
{
    /// <summary>マップ作成時の移動速度 </summary>
    const float FIRST_MOVE_SPEED = 70f;

    [SerializeField, Header("タイルの位置情報")] TilePoint _tilePoint = TilePoint.None;
   
    AstarStatus _astarStatus = AstarStatus.Empty;
    /// <summary>タイルに乗っている街人のリスト </summary>
    List<HumanMove> _onHumans = new List<HumanMove>();
    /// <summary>街人を誘導するためポイントリスト </summary>
    List<RoadPoint> _roadPoints = new List<RoadPoint>();
    /// <summary>繋がっているタイル </summary>
    Dictionary<PointStatus, TileController> _connectingTiles = new Dictionary<PointStatus, TileController>();

    Pathfinding _pathfinding;
    int _row = 0;
    int _col = 0;
    int _realCost = 0;
    int _guessCost = 0;
    int _score = 0;
    public List<HumanMove> OnHumans { get => _onHumans; set => _onHumans = value; }
    public Dictionary<PointStatus, TileController> ConnectingTiles { get => _connectingTiles; set => _connectingTiles = value; }
    public int Col { get => _col; }
    public int Row { get => _row; }
    public int RealCost { get => _realCost; set => _realCost = value; }
    public int GuessCost { get => _guessCost; set => _guessCost = value; }
    public int Score { get => _score; set => _score = value; }
    public AstarStatus AstarStatus { get => _astarStatus; set => _astarStatus = value; }
    public Pathfinding PathfindingClass { get => _pathfinding; set => _pathfinding = value; }
  
    void Start()
    {
        //各出入りと繋がっているタイルを結びつける
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

    private void OnValidate()
    {
        ChangeTag();
    }

    /// <summary>タイルの位置情報に応じてタグを変更する </summary>
    void ChangeTag()
    {
        switch (_tilePoint)
        {
            case TilePoint.Goal:
                gameObject.tag = "GoalTile";
                break;
            case TilePoint.Start:
                gameObject.tag = "StartTile";
                break;
            case TilePoint.None:
                gameObject.tag = "Tile";
                break;
        }
    }

    /// <summary>接続されたタイルを_connectingTilesに追加する </summary>
    public void AddConnectedTile(PointStatus key, TileController tile)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            _connectingTiles[key] = tile;
        }
    }

    /// <summary>プレイヤーに最も近いポイントを返す</summary>
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


    /// <summary>ゲーム開始時一番最初に向かうポイントを取得する </summary>
    public Vector3 GetFirstPoint(Transform player)
    {
        var firstPoint = _roadPoints.OrderBy(p => Vector3.Distance(player.position, p.transform.position)).FirstOrDefault();

        return firstPoint.transform.position;
    }

    /// <summary>指定された出口にタイルが繋がっているか調べる </summary>
    /// <param name="key">指定した出口</param>
    /// <returns>繋がっていたらそのタイルを返す</returns>
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

    /// <summary>位置情報を設定する </summary>
    public void SetPoint(int r, int c)
    {
        _row = r;
        _col = c;
    }

    /// <summary>経路探索で使用するコストを計算する </summary>
    /// <param name="realCost">実コスト</param>
    /// <param name="guessCost">推定コスト</param>
    public void SetCosts(int realCost, int guessCost)
    {
        _realCost = realCost;
        _guessCost = guessCost;
        _score = _realCost + guessCost;
    }

    /// <summary>街人が進入してきたらリストに追加する </summary>
    public void AddHuman(HumanMove human)
    {
        if (!_onHumans.Contains(human))
        {
            _onHumans.Add(human);
        }
    }

    /// <summary>街人が抜けたらリストから削除する </summary>
    public void RemoveHuman(HumanMove human)
    {
        if (_onHumans.Contains(human))
        {
            _onHumans.Remove(human);
        }
    }

    /// <summary>各コライダーをアクティブ状態にする</summary>
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
    /// タイルに乗っていて移動をしていない街人を
    /// 再び移動させる為の関数 
    /// </summary>
    public void StartHumanMove()
    {
        var movedHumans = new List<HumanMove>();

        foreach (var human in _onHumans)
        {
            human.SetFirstPoint();
            movedHumans.Add(human);
        }

        //移動した街人をリストから削除する
        foreach (var movedHuman in movedHumans)
        {
            _onHumans.Remove(movedHuman);
        }
    }

    /// <summary>マップ作成時の移動処理 </summary>
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


