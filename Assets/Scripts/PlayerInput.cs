using UnityEngine;

/// <summary>
/// �v���C���[����̓��͂��Ǘ�����
/// </summary>
public class PlayerInput : MonoBehaviour
{
    /// <summary>�^�C������ւ����I���Ɣ��f���鋗�� </summary>
    const float ARRIVAL_DISTANCE = 0.05f;

    [Tooltip("�l���������قǑ���")]
    [SerializeField, Header("�^�C������ւ����̑��x")] float _tileMoveSpeed = 2f;
    /// <summary>�����������^�C�� </summary>
    TileBase _startMapTile = default;
    /// <summary>������̃^�C�� </summary>
    TileBase _endMapTile = default;
    Vector3 _startPoint = default;
    Vector3 _endPoint = default;
    Transform _centerTransform = default;
    /// <summary>�^�C������ւ������ǂ��� </summary>
    bool _isSwap = false;

    Pathfinding _pathfinding => GetComponent<Pathfinding>();
    StageManager _manager => GetComponent<StageManager>();
    void Update()
    {
        //�}�E�X�ő���
        if (Input.GetButtonDown("Fire1"))   //�����������^�C�������߂�
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
        else if (Input.GetButtonUp("Fire1"))    //������̃^�C�������߂�
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

            if (_startMapTile != null && _endMapTile != null && !_isSwap)   //�^�C�������ւ�
            {
                _manager.AddMoveCount();
                _centerTransform = new GameObject().transform;
                _centerTransform.position = (_startMapTile.transform.position + _endMapTile.transform.position) / 2;
                _centerTransform.forward = _startPoint - _endPoint;
                _isSwap = true;

                //�ʒu�������ւ���
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

    /// <summary>�^�C�������ւ��� </summary>
    void TileSwap()
    {
        var speed = 180 / _tileMoveSpeed;
        var startDistance = Vector3.Distance(_startMapTile.transform.position, _endPoint);
        var endDistance = Vector3.Distance(_endMapTile.transform.position, _startPoint);
        var angleAxis = Quaternion.AngleAxis(speed * Time.deltaTime, _centerTransform.right);
        var startTileFinish = false;
        var endTileFinish = false;

        if (ARRIVAL_DISTANCE <= startDistance)   //�X�^�[�g�^�C��
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

        if (ARRIVAL_DISTANCE <= endDistance)    //�G���h�^�C��
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


        if (startTileFinish && endTileFinish)   //�����̃^�C���̓���ւ���I��
        {
            _isSwap = false;
            _startMapTile = null;
            _endMapTile = null;
            Destroy(_centerTransform.gameObject);
        }
    }
}


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

