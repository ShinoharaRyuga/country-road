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
          //  Debug.DrawRay(ray.origin, ray.direction * 10, Color.green, 5, false);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile") && _startMapTile == null)
                {
                    _startMapTile = hit.collider.gameObject.GetComponent<MapTile>();
                    _startPoint = _startMapTile.transform.position;
                    Debug.Log($"{_startMapTile.name} start {_startMapTile.StartConnectionTile}");
                    Debug.Log($"{_startMapTile.name} end {_startMapTile.EndConnectionTile}");
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
                       // Debug.Log($"end {_endMapTile.name}");
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
    }
}
