using System;
using UnityEngine;

/// <summary>
/// �v���C���[����̓��͂��Ǘ�����
/// </summary>
public class PlayerInput : MonoBehaviour
{
    /// <summary>�����������^�C�� </summary>
    TileBase _startMapTile = default;
    /// <summary>�����悵�����^�C�� </summary>
    TileBase _endMapTile = default;

    Vector3 _startPoint = default;
    Vector3 _endPoint = default;
    void Update()
    {
        //foreach (var touch in Input.touches)    //�X���C�v����
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

        //�}�E�X�ő���
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile") && _startMapTile == null)
                {
                    _startMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                    _startPoint = _startMapTile.transform.position;
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
                        _endPoint = _endMapTile.transform.position;
                    }

                }
            }


            if (_startMapTile != null && _endMapTile != null)   
            {
                //�^�C�������ւ���
                _startMapTile.transform.position = _endPoint;
                _endMapTile.transform.position = _startPoint;
                
                //�ʒu�������ւ���
                var tmpStartRow = _startMapTile.Row;
                var tmpStartCol = _startMapTile.Col;
                _startMapTile.SetPoint(_endMapTile.Row, _endMapTile.Col);
                _endMapTile.SetPoint(tmpStartRow, tmpStartCol);
            }

            _startMapTile = null;
            _endMapTile = null;
        }

        //�f�o�b�O�p
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


                    foreach (var tile in target.ConnectingTiles)
                    {
                        Debug.Log($"{tile.Key} {tile.Value}");
                    }
                }
            }
        }
    }
}
