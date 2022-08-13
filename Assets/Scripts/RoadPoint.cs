using UnityEngine;

/// <summary>タイルへの出入りを管理するクラス </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;
    public PointStatus CurrentStatus { get => _currentStatus; set => _currentStatus = value; }
    /// <summary>親タイル </summary>
    public MapTile ParentMapTile => transform.parent.GetComponent<MapTile>();

    private void OnTriggerEnter(Collider other)
    {
        
    }
}

public enum PointStatus
{
    Start = 0,
    Middle = 1,
    End = 2,
    None = 3,
}
