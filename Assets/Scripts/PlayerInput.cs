using System;
using UnityEngine;

/// <summary>
/// �v���C���[����̓��͂��Ǘ�����
/// </summary>
public class PlayerInput : MonoBehaviour
{
    /// <summary>�����������^�C�� </summary>
    MapTile _startMapTile = default;
    /// <summary>�����悵�����^�C�� </summary>
    MapTile _endMapTile = default;

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
                    _startMapTile = hit.collider.gameObject.GetComponent<MapTile>();
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
                    if (_startMapTile != hit.collider.gameObject.GetComponent<MapTile>())
                    {
                        _endMapTile = hit.collider.gameObject.GetComponent<MapTile>();
                        _endPoint = _endMapTile.transform.position;
                    }

                }
            }

            if (_startMapTile != null && _endMapTile != null)
            {
                _startMapTile.transform.position = _endPoint;
                _endMapTile.transform.position = _startPoint;
            }

            _startMapTile = null;
            _endMapTile = null;
        }


        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile"))
                {
                    var target = hit.collider.gameObject.GetComponent<MapTile>();
                    Debug.Log($"{target.name}");
                    Debug.Log($"start {target.StartConnectionTile}");
                    Debug.Log($"end {target.EndConnectionTile}");

                    //target.TilePoints.ForEach(p => Debug.Log(p.CurrentStatus));
                }
            }
        }
    }
}
