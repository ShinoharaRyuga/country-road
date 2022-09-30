using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class TileCreatorWindow : EditorWindow
{
    GameObject _targetTile = default;
    Vector3 _buildingPosition = default;
    GameObject _test = default;
    private Vector2 _dataScrollPosition;
    private Vector2 _parameterScrollPosition;

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
        // �{�^���\��
        if (GUILayout.Button("�S�[���^�C���𐶐�"))
        {
            var goalTilePrefab = Resources.Load<GameObject>("GoalTile");
            _targetTile = PrefabUtility.InstantiatePrefab(goalTilePrefab) as GameObject;
            _buildingPosition = _targetTile.transform.GetChild(0).position;
        }

        if (GUILayout.Button("����"))
        {
            _test = new GameObject();
            _test.transform.SetParent(_targetTile.transform);
            _test.transform.position = _buildingPosition;
        }

        if (GUILayout.Button("�폜"))
        {
            DestroyImmediate(_test);
        }

        if (GUILayout.Button("Prefab�𐶐�"))
        {
            PrefabUtility.SaveAsPrefabAsset(_targetTile, "Assets/Test.prefab");
        }

        if (GUILayout.Button("�I�u�W�F�N�g�ǂݍ���"))
        {
            var filePathArray = Directory.GetFiles("Assets/BuildingPrefabs", "*", SearchOption.AllDirectories);

            foreach (var filePath in filePathArray)
            {
                var Extension = Path.GetExtension(filePath);
                
                if (Extension == ".prefab")
                {
                    var asset = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                    _assetList.Add(asset);
                }
            }

            Debug.Log("�ǂݍ���");

            foreach (var asset in _assetList)
            {
                Debug.Log(asset.name);
            }

            _assetList.Clear();
        }
    }
}

