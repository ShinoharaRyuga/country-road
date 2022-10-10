using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


/// <summary>タイルを生成するウィンドウのクラス </summary>
public class TileCreatorWindow : EditorWindow
{
    /// <summary>タイルの保存先のパス </summary>
    const string SAVE_FOLDER_PATH = "Assets/Prefabs/Tiles/";
    const string LOAD_FOLDER_PATH = "Assets/BuildingPrefabs/";

    GameObject _targetTile = default;
    GameObject _selectBuilding = default;
    /// <summary>建物オブジェクトの生成位置 </summary>
    Vector3 _buildingPoint = default;
    /// <summary>タイルのtransform </summary>
    Vector3 _buildingPosition = Vector3.zero;
    Vector3 _buildingRotation = Vector3.zero;
    Vector3 _buildingScale = Vector3.zero;

    /// <summary>GUILayout用 </summary>
    Vector2 _dataScrollPosition;
    Vector2 _parameterScrollPosition;
    Vector2 _tileScrollPosition;
   
    /// <summary>読み込んだプレハブのリスト</summary>
    List<GameObject> _prefabList = new List<GameObject>();

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
                        _buildingPoint = _targetTile.transform.GetChild(0).position;
                    }

                    if (GUILayout.Button("Prefabを生成"))
                    {
                        if (_targetTile == null)
                        {
                            Debug.Log("タイルが存在しません\nタイルを生成してください");
                            return;
                        }
                        Debug.Log("プレハブを作成しました");

                        var prefabName = $"{SAVE_FOLDER_PATH}{_selectBuilding}.prefab";
                        PrefabUtility.SaveAsPrefabAsset(_targetTile, prefabName);
                    }

                    if (GUILayout.Button("プレハブ読み込み"))
                    {
                        var filePathArray = Directory.GetFiles(LOAD_FOLDER_PATH, "*", SearchOption.AllDirectories);

                        foreach (var filePath in filePathArray)
                        {
                            var Extension = Path.GetExtension(filePath);

                            if (Extension == ".prefab")
                            {
                                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);

                                if (!_prefabList.Contains(asset))
                                {
                                    _prefabList.Add(asset);
                                }
                            }
                        }

                        Debug.Log("読み込み");
                    }
       
                    GUILayout.EndHorizontal();   
                }

                GUILayout.BeginHorizontal();    //Position
                {
                    _buildingPosition = EditorGUILayout.Vector3Field("Position", _buildingPosition);

                    if (GUILayout.Button("値を適用") && _selectBuilding != null)
                    {
                        _selectBuilding.transform.position = _buildingPosition;
                    }
                   
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();    //Rotation
                {
                    _buildingRotation = EditorGUILayout.Vector3Field("Rotation", _buildingRotation);

                    if (GUILayout.Button("値を適用") && _selectBuilding != null)
                    {
                        _selectBuilding.transform.rotation = Quaternion.Euler(_buildingRotation);
                    }
                    
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();    //Scale
                {
                    _buildingScale = EditorGUILayout.Vector3Field("Scale", _buildingScale);

                    if (GUILayout.Button("値を適用") && _selectBuilding != null)
                    {
                        _selectBuilding.transform.localScale = _buildingScale;
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
            var root = rootVisualElement;
           
            _dataScrollPosition = scroll.scrollPosition;
            GUILayout.Label("建物");

            foreach (var target in _prefabList)
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
        _selectBuilding.transform.position = new Vector3(_buildingPoint.x, 0.5f, _buildingPoint.z);

        //値をセット
        _buildingPosition = _selectBuilding.transform.position;
        _buildingRotation = _selectBuilding.transform.localEulerAngles;
        _buildingRotation = _selectBuilding.transform.localScale;
    }
}

