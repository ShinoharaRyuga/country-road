using System;
using UnityEngine;

/// <summary>
/// プレイヤーからの入力を管理する
/// </summary>
public class PlayerInput : MonoBehaviour
{
    /// <summary>交換したいタイル </summary>
    [SerializeField] TileBase _startMapTile = default;
    /// <summary>交換先のタイル </summary>
    [SerializeField] TileBase _endMapTile = default;
    Vector3 _startPoint = default;
    Vector3 _endPoint = default;

    // 位置座標
    private Vector3 position;

    GameObject go;
    Vector3 _point;
    void Update()
    {
        //マウスで操作
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile") && _startMapTile == null)
                {
                    _startMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                   // _startPoint = _startMapTile.transform.position;
                }
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile"))
                {
                    if (_startMapTile != hit.collider.gameObject.GetComponent<TileBase>())
                    {
                        _endMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                       // _endPoint = _endMapTile.transform.position;
                    }
                }
            }



            if (_startMapTile != null && _endMapTile != null)
            {
                //タイルを入れ替える
                //_startMapTile.transform.position = _endPoint;
                //  _endMapTile.transform.position = _startPoint;

                //位置情報を入れ替える
                var tmpStartRow = _startMapTile.Row;
                var tmpStartCol = _startMapTile.Col;
                _startMapTile.SetPoint(_endMapTile.Row, _endMapTile.Col);
                _endMapTile.SetPoint(tmpStartRow, tmpStartCol);
            }

            _startMapTile = null;
            _endMapTile = null;
        }



        if (go == null)
        {
            go = Instantiate(new GameObject(), (_startMapTile.transform.position + _endMapTile.transform.position) / 2, Quaternion.identity);
            go.transform.forward = _startMapTile.transform.position - _endMapTile.transform.position;
            _point = go.transform.position;
            _startPoint = _startMapTile.transform.position;
            _endPoint = _endMapTile.transform.position;
        }
        else
        {
            var distance = Vector3.Distance(_startMapTile.transform.position, _endPoint);
            Debug.Log(distance);
            if (0.05f <= distance)
            {
                var tr = _startMapTile.transform;
                // 回転のクォータニオン作成
                var angleAxis = Quaternion.AngleAxis(360 / 2 * Time.deltaTime, go.transform.right);
                // 円運動の位置計算
                var pos = tr.position;

                pos -= _point;
                pos = angleAxis * pos;
                pos += _point;

                tr.position = pos;


            }

            var distance1 = Vector3.Distance(_endMapTile.transform.position, _startPoint);
            Debug.Log(distance1);
            if (0.03f <= distance1)
            {
                var tr1 = _endMapTile.transform;
                // 回転のクォータニオン作成
                var angleAxis1 = Quaternion.AngleAxis(360 / 2 * Time.deltaTime, go.transform.right);
                // 円運動の位置計算
                var pos1 = tr1.position;

                pos1 -= _point;
                pos1 = angleAxis1 * pos1;
                pos1 += _point;

                tr1.position = pos1;
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
                    //Debug.Log($"realCost {target.RealCost}");
                    //Debug.Log($"GuessCost {target.GuessCost}");
                    //Debug.Log($"Score {target.Score}");


                    //foreach (var tile in target.ConnectingTiles)
                    //{
                    //    Debug.Log($"{tile.Key} {tile.Value}");
                    //}

                    Debug.Log(target.OnHumans.Count);
                }
            }
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

