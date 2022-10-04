using DG.Tweening;
using UnityEngine;

/// <summary>
/// プレイヤーからの入力を管理する
/// </summary>
public class PlayerInput : MonoBehaviour
{
    [SerializeField, Header("入れ替え時の高さ")] float _swapHeight = 4f;
    [SerializeField, Header("入れ替えにかかる時間")] float _swapTime = 0.7f;
    /// <summary>交換したいタイル </summary>
    TileBase _startMapTile = default;
    /// <summary>交換先のタイル </summary>
    TileBase _endMapTile = default;
    Vector3 _startPoint = default;
    Vector3 _endPoint = default;
    Pathfinding _pathfinding => GetComponent<Pathfinding>();
    StageManager _manager => GetComponent<StageManager>();
    /// <summary>タイル入れ替えをしているかどうか </summary>
    bool _isSwap = false;

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
                if (hit.collider.gameObject.CompareTag("Tile") && !_isSwap)
                {
                    if (_startMapTile != hit.collider.gameObject.GetComponent<TileBase>())
                    {
                        _endMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                        _endPoint = _endMapTile.transform.position;
                    }
                }
            }

            if (_startMapTile != null && _endMapTile != null && !_isSwap)   //タイルを入れ替え
            {
                _manager.AddMoveCount();
               
                //位置情報を入れ替える
                _pathfinding.SwapTile(_startMapTile, _endMapTile);
                var tmpStartRow = _startMapTile.Row;
                var tmpStartCol = _startMapTile.Col;
                _startMapTile.SetPoint(_endMapTile.Row, _endMapTile.Col);
                _endMapTile.SetPoint(tmpStartRow, tmpStartCol);

                TileSwap();
            }
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
        _isSwap = true;
        _startMapTile.transform.DOJump(_endPoint, jumpPower: _swapHeight, numJumps: 1, duration: _swapTime);
        _endMapTile.transform.DOJump(_startPoint, jumpPower: -_swapHeight, numJumps: 1, duration: _swapTime)
            .OnComplete(() => SwapFinish());
    }

    /// <summary>タイル入れ替え完了時の処理 </summary>
    void SwapFinish()
    {
        _startMapTile = null;
        _endMapTile = null;
        _isSwap = false;
    }
}


