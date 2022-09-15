using UnityEngine;

/// <summary>タイル同士を接続したり切り離したりするクラス </summary>
public class ConnectingTiles : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;

    public TileBase ParentTile => transform.parent.GetComponent<TileBase>();

    public PointStatus CurrentStatus { get => _currentStatus; }

    public bool IsMiddle => CurrentStatus == PointStatus.Middle;

    /// <summary>
    /// タイル同士を繋げる為の関数
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ConnectingTiles tile))
        {
            ParentTile.AddConnectedTile(_currentStatus, tile.ParentTile);
        }
    }

    /// <summary>
    /// タイルが移動されて他のタイルとの繋がりがなくなった場合の処理 
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ConnectingTiles tile))
        {
            ParentTile.AddConnectedTile(_currentStatus, null);
        }
    }
}
