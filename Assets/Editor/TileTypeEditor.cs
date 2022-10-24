using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileController))]
public class TileTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var instance = target as TileController;

        switch (instance.TileType)
        {
            case TileType.None:
                instance.NoneType = (NoneType)EditorGUILayout.EnumPopup("NoneType", instance.NoneType);
                break;
            case TileType.Building:
                instance.BuildingType = (BuildingType)EditorGUILayout.EnumPopup("BuildingType", instance.BuildingType);
                break;
            case TileType.Road:
                instance.RoadType = (RoadType)EditorGUILayout.EnumPopup("RoadType", instance.RoadType);
                break;
        } 
    }
}
