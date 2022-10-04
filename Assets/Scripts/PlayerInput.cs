using DG.Tweening;
using UnityEngine;

/// <summary>
/// �v���C���[����̓��͂��Ǘ�����
/// </summary>
public class PlayerInput : MonoBehaviour
{
    [SerializeField, Header("����ւ����̍���")] float _swapHeight = 4f;
    [SerializeField, Header("����ւ��ɂ����鎞��")] float _swapTime = 0.7f;
    /// <summary>�����������^�C�� </summary>
    TileBase _startMapTile = default;
    /// <summary>������̃^�C�� </summary>
    TileBase _endMapTile = default;
    Vector3 _startPoint = default;
    Vector3 _endPoint = default;
    Pathfinding _pathfinding => GetComponent<Pathfinding>();
    StageManager _manager => GetComponent<StageManager>();
    /// <summary>�^�C������ւ������Ă��邩�ǂ��� </summary>
    bool _isSwap = false;

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
                if (hit.collider.gameObject.CompareTag("Tile") && !_isSwap)
                {
                    if (_startMapTile != hit.collider.gameObject.GetComponent<TileBase>())
                    {
                        _endMapTile = hit.collider.gameObject.GetComponent<TileBase>();
                        _endPoint = _endMapTile.transform.position;
                    }
                }
            }

            if (_startMapTile != null && _endMapTile != null && !_isSwap)   //�^�C�������ւ�
            {
                _manager.AddMoveCount();
               
                //�ʒu�������ւ���
                _pathfinding.SwapTile(_startMapTile, _endMapTile);
                var tmpStartRow = _startMapTile.Row;
                var tmpStartCol = _startMapTile.Col;
                _startMapTile.SetPoint(_endMapTile.Row, _endMapTile.Col);
                _endMapTile.SetPoint(tmpStartRow, tmpStartCol);

                TileSwap();
            }
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
        _isSwap = true;
        _startMapTile.transform.DOJump(_endPoint, jumpPower: _swapHeight, numJumps: 1, duration: _swapTime);
        _endMapTile.transform.DOJump(_startPoint, jumpPower: -_swapHeight, numJumps: 1, duration: _swapTime)
            .OnComplete(() => SwapFinish());
    }

    /// <summary>�^�C������ւ��������̏��� </summary>
    void SwapFinish()
    {
        _startMapTile = null;
        _endMapTile = null;
        _isSwap = false;
    }
}


