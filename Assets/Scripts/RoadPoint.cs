using UnityEngine;

/// <summary>タイルの子オブジェクトのポイントを管理するクラス </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField, Tooltip("ポイントの状態")] PointStatus _pointStatus = PointStatus.None;
    /// <summary>親タイル </summary>
    MapTile MapTile => transform.parent.GetComponent<MapTile>();
    public PointStatus PointStatus { get => _pointStatus; set => _pointStatus = value; }

    private void OnTriggerEnter(Collider other)
    {
        //タイルが移動されて他のタイルの繋がりが出来た時の処理 繋がったタイル情報を親タイルで保持をする
        if (other.gameObject.CompareTag("Connection"))
        {
            if (_pointStatus == PointStatus.Start)  //スタートポイントに繋がりが出来た時
            {
                MapTile.StartConnectionTile = other.transform.parent.GetComponent<MapTile>();
            }
            else if (_pointStatus == PointStatus.End)  //エンドポイントに繋がりが出来た時
            {
                MapTile.EndConnectionTile = other.transform.parent.GetComponent<MapTile>();
            }
        }
    }

    /// <summary>
    /// タイルが移動されて他のタイルとの繋がりがなくなった場合の処理 
    /// startとendに繋がっていたタイル情報を削除する
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Point"))
        {
            if (_pointStatus == PointStatus.Start)
            {
                MapTile.StartConnectionTile = null;
            }
            else if (_pointStatus == PointStatus.End)
            {
                MapTile.EndConnectionTile = null;
            }
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
