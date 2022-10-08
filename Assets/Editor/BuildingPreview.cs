using UnityEngine;
using UnityEditor;

public class BuildingPreview : Editor
{
    GUIContent _title = new GUIContent("Œš•¨ƒvƒŒƒrƒ…[");
   static GameObject _previewObject = default;

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override GUIContent GetPreviewTitle()
    {
        return _title;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        GUI.DrawTexture(r, AssetPreview.GetAssetPreview(_previewObject));
    }

    public static void GetPreviewObject(GameObject target)
    {
        _previewObject = target;
    }
}
