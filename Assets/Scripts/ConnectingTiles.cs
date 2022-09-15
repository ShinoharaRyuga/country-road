using UnityEngine;

/// <summary>�^�C�����m��ڑ�������؂藣�����肷��N���X </summary>
public class ConnectingTiles : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;

    public TileBase ParentTile => transform.parent.GetComponent<TileBase>();

    public PointStatus CurrentStatus { get => _currentStatus; }

    public bool IsMiddle => CurrentStatus == PointStatus.Middle;

    /// <summary>
    /// �^�C�����m���q����ׂ̊֐�
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ConnectingTiles tile))
        {
            ParentTile.AddConnectedTile(_currentStatus, tile.ParentTile);
        }
    }

    /// <summary>
    /// �^�C�����ړ�����đ��̃^�C���Ƃ̌q���肪�Ȃ��Ȃ����ꍇ�̏��� 
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ConnectingTiles tile))
        {
            ParentTile.AddConnectedTile(_currentStatus, null);
        }
    }
}
