using UnityEngine;

/// <summary>
/// プレイヤーからの入力を管理する
/// </summary>
public class PlayerInput : MonoBehaviour
{
    /// <summary>タイル入れ替えが終了と判断する距離 </summary>
    const float ARRIVAL_DISTANCE = 0.05f;

    [Tooltip("値が小さいほど速い")]
    [SerializeField, Header("タイル入れ替え時の速度")] float _tileMoveSpeed = 2f;
    /// <summary>交換したいタイル </summary>
    TileBase _startMapTile = default;
    /// <summary>交換先のタイル </summary>
    TileBase _endMapTile = default;
    Vector3 _startPoint = default;
    Vector3 _endPoint = default;
    Transform _centerTransform = default;
    /// <summary>タイル入れ替え中かどうか </summary>
    bool _isSwap = false;

    Pathfinding _pathfinding => GetComponent<Pathfinding>();
    StageManager _manager => GetComponent<StageManager>();
    void Update()
    {
        //マウスで操作
        if (Input.GetButtonDown("Fire1"))   //交換したいタイルを決める
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile") && _startMapTile == null && !_isSwap)
                {
                    _startMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                    _startPoint = _startMapTile.transform.position;
                }
            }
        }
        else if (Input.GetButtonUp("Fire1"))    //交換先のタイルを決める
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile"))
                {
                    if (_startMapTile != hit.collider.gameObject.GetComponent<TileBase>() && !_isSwap)
                    {
                        _endMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                        _endPoint = _endMapTile.transform.position;
                    }
                }
            }

            if (_startMapTile != null && _endMapTile != null && !_isSwap)   //タイルを入れ替え
            {
                _manager.AddMoveCount();
                _centerTransform = new GameObject().transform;
                _centerTransform.position = (_startMapTile.transform.position + _endMapTile.transform.position) / 2;
                _centerTransform.forward = _startPoint - _endPoint;
                _isSwap = true;

                //位置情報を入れ替える
                _pathfinding.SwapTile(_startMapTile, _endMapTile);
                var tmpStartRow = _startMapTile.Row;
                var tmpStartCol = _startMapTile.Col;
                _startMapTile.SetPoint(_endMapTile.Row, _endMapTile.Col);
                _endMapTile.SetPoint(tmpStartRow, tmpStartCol);
              
            }
        }

        if (_isSwap)
        {
            TileSwap();
        }

        //デバッグ用
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile"))
                {
                    var target = hit.collider.gameObject.GetComponent<TileBase>();

                    Debug.Log(target.name);
                    //Debug.Log($"realCost {target.RealCost}");
                    //Debug.Log($"GuessCost {target.GuessCost}");
                    //Debug.Log($"Score {target.Score}");
                    //Debug.Log(target.OnHumans.Count);
                    //Debug.Log($"Row {target.Row}");
                    //Debug.Log($"Col {target.Col}");
                    foreach (var tile in target.ConnectingTiles)
                    {
                        Debug.Log($"{tile.Key} {tile.Value}");
                    }

               
                }
            }
        }
    }

    /// <summary>タイルを入れ替える </summary>
    void TileSwap()
    {
        var speed = 180 / _tileMoveSpeed;
        var startDistance = Vector3.Distance(_startMapTile.transform.position, _endPoint);
        var endDistance = Vector3.Distance(_endMapTile.transform.position, _startPoint);
        var angleAxis = Quaternion.AngleAxis(speed * Time.deltaTime, _centerTransform.right);
        var startTileFinish = false;
        var endTileFinish = false;

        if (ARRIVAL_DISTANCE <= startDistance)   //スタートタイル
        {
            var pos = _startMapTile.transform.position;
            pos -= _centerTransform.position;
            pos = angleAxis * pos;
            pos += _centerTransform.position;
            _startMapTile.transform.position = pos;
        }
        else
        {
            startTileFinish = true;
        }

        if (ARRIVAL_DISTANCE <= endDistance)    //エンドタイル
        {
            var pos = _endMapTile.transform.position;
            pos -= _centerTransform.position;
            pos = angleAxis * pos;
            pos += _centerTransform.position;
            _endMapTile.transform.position = pos;
        }
        else
        {
            endTileFinish = true;
        }


        if (startTileFinish && endTileFinish)   //両方のタイルの入れ替える終了
        {
            _isSwap = false;
            _startMapTile = null;
            _endMapTile = null;
            Destroy(_centerTransform.gameObject);
        }
    }
}


//foreach (var touch in Input.touches)    //スワイプ入力
//{
//    if (touch.phase == TouchPhase.Began)
//    {
//        Ray ray = Camera.main.ScreenPointToRay(touch.position);
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit))
//        {
//            if (hit.collider.gameObject.CompareTag("Tile"))
//            {
//                _startMapTile = hit.collider.gameObject.GetComponent<MapTile>();
//                _startPoint = _startMapTile.transform.position;
//                Debug.Log($"start {_startMapTile.name}");
//            }
//        }
//    }
//    else if (touch.phase == TouchPhase.Ended)
//    {
//        Ray ray = Camera.main.ScreenPointToRay(touch.position);
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit))
//        {
//            if (hit.collider.gameObject.CompareTag("Tile"))
//            {
//                if (_startMapTile != hit.collider.gameObject.GetComponent<MapTile>())
//                {
//                    _endMapTile = hit.collider.gameObject.GetComponent<MapTile>();
//                }

//            }
//        }

//if (_startMapTile != null && _endMapTile != null)
//{
//    _startMapTile.transform.position = _endPoint;
//    _endMapTile.transform.position = _startPoint;


//}

//_startMapTile = null;
//_endMapTile = null;
//    }
//}

