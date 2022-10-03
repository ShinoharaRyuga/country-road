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

    /// <summary>�E�B���h�E���̃��C�A�E�g���쐬���� </summary>
    void CreateLayout()
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        {
            GUILayout.BeginVertical(GUI.skin.box);  //����
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (GUILayout.Button("�^�C���𐶐�"))
                    {
                        var tileBasePrefab = Resources.Load<GameObject>("TileBase");
                        _targetTile = PrefabUtility.InstantiatePrefab(tileBasePrefab) as GameObject;
                        _buildingPosition = _targetTile.transform.GetChild(0).position;
                    }

                    if (GUILayout.Button("Prefab�𐶐�"))
                    {
                        if (_targetTile == null)
                        {
                            Debug.Log("�^�C�������݂��܂���\n�^�C���𐶐����Ă�������");
                            return;
                        }
                        Debug.Log(_selectBuilding.name);

                        // PrefabUtility.SaveAsPrefabAsset(_targetTile, "Assets/Test.prefab");
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

                                if (!_assetList.Contains(asset))
                                {
                                    _assetList.Add(asset);
                                }
                            }
                        }

                        Debug.Log("�ǂݍ���");

                        foreach (var asset in _assetList)
                        {
                            Debug.Log(asset.name);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical(GUI.skin.box);  //�E��
            {
                SetupAssets();
                GUILayout.EndVertical();
            }
           GUILayout.EndHorizontal();
        }
    }

    /// <summary>�ǂݍ��񂾃A�Z�b�g���E�B���h�E�ɕ\������ </summary>
    void SetupAssets()
    {
        using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(_dataScrollPosition, EditorStyles.helpBox, GUILayout.Width(250)))
        {
            _dataScrollPosition = scroll.scrollPosition;
            GUILayout.Label("����");

            foreach (var target in _assetList)
            {
                if (GUILayout.Button(target.name))
                {
                    CreateBuilding(target);
                }
            }
        }
    }

    /// <summary>�w�肳�ꂽ�I�u�W�F�N�g�𐶐�����</summary>
    /// <param name="target"></param>
    void CreateBuilding(GameObject target)
    {
        if (_targetTile == null)
        {
            Debug.Log("��Ƀ^�C���𐶐����Ă�������");
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

