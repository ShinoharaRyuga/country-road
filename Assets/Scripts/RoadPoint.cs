using UnityEngine;

/// <summary>街人を誘導するためポイント </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;
    public PointStatus CurrentStatus { get => _currentStatus; set => _currentStatus = value; }

    public bool IsMiddle => CurrentStatus == PointStatus.Middle;
    /// <summary>親タイル </summary>
    public TileController ParentMapTile => transform.parent.GetComponent<TileController>();
}


