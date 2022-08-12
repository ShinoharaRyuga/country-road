using UnityEngine;

/// <summary>�^�C���ւ̏o������Ǘ�����N���X </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;
    public PointStatus CurrentStatus { get => _currentStatus; set => _currentStatus = value; }
    /// <summary>�e�^�C�� </summary>
    MapTile _parentMapTile => transform.parent.GetComponent<MapTile>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human") && _currentStatus != PointStatus.Middle)
        {
            var human = other.GetComponent<HumanMove>();

            if (_parentMapTile.CheckHumans(human))
            {
                _parentMapTile.Humans.Add(human);
                Debug.Log("�ǉ�");
            }
            else
            {
                human.SetNextTile(_currentStatus);
                _parentMapTile.Humans.Remove(human);
                Debug.Log("���^�C��");
            }
        }
        else
        {
            Debug.Log("hit");
        }
    }
}

public enum PointStatus
{
    Start = 0,
    Middle = 1,
    End = 2,
    None = 3,
}
