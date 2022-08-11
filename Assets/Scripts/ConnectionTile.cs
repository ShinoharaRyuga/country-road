using UnityEngine;

public class ConnectionTile : MonoBehaviour
{
    [SerializeField] ConnectionStatus _currentStatus = ConnectionStatus.None;
    MapTile _parentMapTile => transform.parent.GetComponent<MapTile>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Connection"))
        {
            if (_currentStatus == ConnectionStatus.Start)
            {
                _parentMapTile.StartConnectionTile = other.transform.parent.GetComponent<MapTile>();
            }
            else if(_currentStatus == ConnectionStatus.End)
            {
                _parentMapTile.EndConnectionTile = other.transform.parent.GetComponent<MapTile>();
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
                _parentMapTile.StartConnectionTile = null;
            }
            else if (_currentStatus == ConnectionStatus.End)
            {
                _parentMapTile.EndConnectionTile = null;
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
