using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class TileCreatorWindow : EditorWindow
{
    GameObject _targetTile = default;
    GameObject _selectBuilding = default;

    Vector2 _dataScrollPosition;
    Vector2 _parameterScrollPosition;
    Vector2 _tileScrollPosition;
    Vector3 _buildingPosition = default;

    string _targerPath = "Assets/BuildingPrefabs";
    List<GameObject> _assetList = new List<GameObject>();


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
        CreateLayout();
    }

    /// <summary>ウィンドウ内のレイアウトを作成する </summary>
    void CreateLayout()
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        {
            GUILayout.BeginVertical(GUI.skin.box);  //左側
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (GUILayout.Button("タイルを生成"))
                    {
                        var tileBasePrefab = Resources.Load<GameObject>("TileBase");
                        _targetTile = PrefabUtility.InstantiatePrefab(tileBasePrefab) as GameObject;
                        _buildingPosition = _targetTile.transform.GetChild(0).position;
                    }

                    if (GUILayout.Button("Prefabを生成"))
                    {
                        if (_targetTile == null)
                        {
                            Debug.Log("タイルが存在しません\nタイルを生成してください");
                            return;
                        }
                        Debug.Log(_selectBuilding.name);

                        // PrefabUtility.SaveAsPrefabAsset(_targetTile, "Assets/Test.prefab");
                    }

                    if (GUILayout.Button("オブジェクト読み込み"))
                    {
                        var filePathArray = Directory.GetFiles("Assets/BuildingPrefabs", "*", SearchOption.AllDirectories);

                        foreach (var filePath in filePathArray)
                        {
                            var Extension = Path.GetExtension(filePath);

                            if (Extension == ".prefab")
                            {
                                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);

                                if (!_assetList.Contains(asset))
                                {
                                    _assetList.Add(asset);
                                }
                            }
                        }

                        Debug.Log("読み込み");

                        foreach (var asset in _assetList)
                        {
                            Debug.Log(asset.name);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical(GUI.skin.box);  //右側
            {
                SetupAssets();
                GUILayout.EndVertical();
            }
           GUILayout.EndHorizontal();
        }
    }

    /// <summary>読み込んだアセットをウィンドウに表示する </summary>
    void SetupAssets()
    {
        using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(_dataScrollPosition, EditorStyles.helpBox, GUILayout.Width(250)))
        {
            _dataScrollPosition = scroll.scrollPosition;
            GUILayout.Label("建物");

            foreach (var target in _assetList)
            {
                if (GUILayout.Button(target.name))
                {
                    CreateBuilding(target);
                }
            }
        }
    }

    /// <summary>指定されたオブジェクトを生成する</summary>
    /// <param name="target"></param>
    void CreateBuilding(GameObject target)
    {
        if (_targetTile == null)
        {
            Debug.Log("先にタイルを生成してください");
            return;
        }

        if (_selectBuilding != null)
        {
            DestroyImmediate(_selectBuilding);
        }

        _selectBuilding = PrefabUtility.InstantiatePrefab(target) as GameObject;
        _selectBuilding.transform.SetParent(_targetTile.transform);
        _selectBuilding.transform.position = new Vector3(_buildingPosition.x, 0.5f, _buildingPosition.z);
    }
}

