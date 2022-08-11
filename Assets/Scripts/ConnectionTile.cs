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
    /// �^�C�����ړ�����đ��̃^�C���Ƃ̌q���肪�Ȃ��Ȃ����ꍇ�̏��� 
    /// start��end�Ɍq�����Ă����^�C�������폜����
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
