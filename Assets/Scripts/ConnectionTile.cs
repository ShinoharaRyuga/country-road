using UnityEngine;

/// <summary>�^�C�����m��ڑ�������؂藣�����肷��N���X </summary>
public class ConnectionTile : MonoBehaviour
{
    [SerializeField] ConnectionStatus _currentStatus = ConnectionStatus.None;
    /// <summary>�e�^�C�� </summary>
    public MapTile ParentMapTile => transform.parent.GetComponent<MapTile>();

    /// <summary>
    /// �^�C�����m���q����ׂ̊֐�
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
    /// �^�C�����ړ�����đ��̃^�C���Ƃ̌q���肪�Ȃ��Ȃ����ꍇ�̏��� 
    /// start��end�Ɍq�����Ă����^�C�������폜����
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
