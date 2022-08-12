using UnityEngine;

/// <summary>タイルへの出入りを管理するクラス </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;
    public PointStatus CurrentStatus { get => _currentStatus; set => _currentStatus = value; }
    /// <summary>親タイル </summary>
    MapTile _parentMapTile => transform.parent.GetComponent<MapTile>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human") && _currentStatus != PointStatus.Middle)
        {
            var human = other.GetComponent<HumanMove>();

            if (_parentMapTile.CheckHumans(human))
            {
                _parentMapTile.Humans.Add(human);
                Debug.Log("追加");
            }
            else
            {
                human.SetNextTile(_currentStatus);
                _parentMapTile.Humans.Remove(human);
                Debug.Log("次タイル");
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
