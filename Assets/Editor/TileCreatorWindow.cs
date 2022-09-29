using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class TileCreatorWindow : EditorWindow
{
    GameObject _targetTile = default;
    Vector3 _buildingPosition = default;
    GameObject _test = default;

    [MenuItem("Window/TileCreator")]
    static void Open()
    {
        var window = GetWindow<TileCreatorWindow>();
        window.titleContent = new GUIContent("TileCreatorScene");

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/TileCreatorScene.unity");
        }
    }

    private void OnGUI()
    {
        // ボタン表示
        if (GUILayout.Button("ゴールタイルを生成"))
        {
            var goalTilePrefab = Resources.Load<GameObject>("GoalTile");
            _targetTile = PrefabUtility.InstantiatePrefab(goalTilePrefab) as GameObject;
            _buildingPosition = _targetTile.transform.GetChild(0).position;
        }

        if (GUILayout.Button("生成"))
        {
            _test = new GameObject();
            _test.transform.SetParent(_targetTile.transform);
            _test.transform.position = _buildingPosition;
        }

        if (GUILayout.Button("削除"))
        {
            DestroyImmediate(_test);
        }

        if (GUILayout.Button("Prefabを生成"))
        {
            PrefabUtility.SaveAsPrefabAsset(_targetTile, "Assets/Test.prefab");
        }
    }
}
