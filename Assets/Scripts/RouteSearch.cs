using System.Collections.Generic;
using UnityEngine;

public class RouteSearch : MonoBehaviour
{
    public void Search(TileBase lastTile, TileBase currentTile)
    {
        var routes = new Dictionary<PointStatus, int>();


    }

    public void CheckRoute(TileBase currentTile)
    {
        if (currentTile == null) { return; }




        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.First))
        {
            var nextTile = currentTile.ConnectingTiles[PointStatus.First];
            CheckRoute(nextTile);
        }

        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.Second))
        {
            var nextTile = currentTile.ConnectingTiles[PointStatus.Second];
            CheckRoute(nextTile);
        }

        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.Third))
        {
            var nextTile = currentTile.ConnectingTiles[PointStatus.Third];
            CheckRoute(nextTile);
        }

        if (currentTile.ConnectingTiles.ContainsKey(PointStatus.Fourth))
        {
            var nextTile = currentTile.ConnectingTiles[PointStatus.Fourth];
            CheckRoute(nextTile);
        }
    }
}
