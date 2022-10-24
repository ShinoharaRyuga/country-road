using UnityEngine;

/// <summary>�X�l��U�����邽�߃|�C���g </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField] PointStatus _currentStatus = PointStatus.None;
    public PointStatus CurrentStatus { get => _currentStatus; set => _currentStatus = value; }

    public bool IsMiddle => CurrentStatus == PointStatus.Middle;
    /// <summary>�e�^�C�� </summary>
    public TileController ParentMapTile => transform.parent.GetComponent<TileController>();
}


