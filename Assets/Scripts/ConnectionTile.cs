using UnityEngine;

/// <summary>タイル同士を接続したり切り離したりするクラス </summary>
public class ConnectionTile : MonoBehaviour
{
    [SerializeField] ConnectionStatus _currentStatus = ConnectionStatus.None;
    /// <summary>親タイル </summary>
    public MapTile ParentMapTile => transform.parent.GetComponent<MapTile>();

    /// <summary>
    /// タイル同士を繋げる為の関数
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Connection"))
        {
            if (_currentStatus == ConnectionStatus.Start)
            {
                ParentMapTile.StartConnectionTile = other.transform.parent.GetComponent<MapTile>();
                ParentMapTile.AgainMove();
            }
            else if(_currentStatus == ConnectionStatus.End)
            {
                ParentMapTile.EndConnectionTile = other.transform.parent.GetComponent<MapTile>();
                ParentMapTile.AgainMove();
            }
        }
    }

    /// <summary>
    /// タイルが移動されて他のタイルとの繋がりがなくなった場合の処理 
    /// startとendに繋がっていたタイル情報を削除する
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Connection"))
        {
            if (_currentStatus == ConnectionStatus.Start)
            {
                ParentMapTile.StartConnectionTile = null;
            }
            else if (_currentStatus == ConnectionStatus.End)
            {
                ParentMapTile.EndConnectionTile = null;
            }
        }
    }
}

public enum ConnectionStatus
{
    None,
    Start,
    End,
}
