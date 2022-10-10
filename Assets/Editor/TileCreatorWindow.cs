using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


/// <summary>�^�C���𐶐�����E�B���h�E�̃N���X </summary>
public class TileCreatorWindow : EditorWindow
{
    /// <summary>�^�C���̕ۑ���̃p�X </summary>
    const string SAVE_FOLDER_PATH = "Assets/Prefabs/Tiles/";
    const string LOAD_FOLDER_PATH = "Assets/BuildingPrefabs/";

    GameObject _targetTile = default;
    GameObject _selectBuilding = default;
    /// <summary>�����I�u�W�F�N�g�̐����ʒu </summary>
    Vector3 _buildingPoint = default;
    /// <summary>�^�C����transform </summary>
    Vector3 _buildingPosition = Vector3.zero;
    Vector3 _buildingRotation = Vector3.zero;
    Vector3 _buildingScale = Vector3.zero;

    /// <summary>GUILayout�p </summary>
    Vector2 _dataScrollPosition;
    Vector2 _parameterScrollPosition;
    Vector2 _tileScrollPosition;
   
    /// <summary>�ǂݍ��񂾃v���n�u�̃��X�g</summary>
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
                        _buildingPoint = _targetTile.transform.GetChild(0).position;
                    }

                    if (GUILayout.Button("Prefab�𐶐�"))
                    {
                        if (_targetTile == null)
                        {
                            Debug.Log("�^�C�������݂��܂���\n�^�C���𐶐����Ă�������");
                            return;
                        }
                        Debug.Log("�v���n�u���쐬���܂���");

                        var prefabName = $"{SAVE_FOLDER_PATH}{_selectBuilding}.prefab";
                        PrefabUtility.SaveAsPrefabAsset(_targetTile, prefabName);
                    }

                    if (GUILayout.Button("�v���n�u�ǂݍ���"))
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

                        Debug.Log("�ǂݍ���");
                    }
       
                    GUILayout.EndHorizontal();   
                }

                GUILayout.BeginHorizontal();    //Position
                {
                    _buildingPosition = EditorGUILayout.Vector3Field("Position", _buildingPosition);

                    if (GUILayout.Button("�l��K�p") && _selectBuilding != null)
                    {
                        _selectBuilding.transform.position = _buildingPosition;
                    }
                   
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();    //Rotation
                {
                    _buildingRotation = EditorGUILayout.Vector3Field("Rotation", _buildingRotation);

                    if (GUILayout.Button("�l��K�p") && _selectBuilding != null)
                    {
                        _selectBuilding.transform.rotation = Quaternion.Euler(_buildingRotation);
                    }
                    
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();    //Scale
                {
                    _buildingScale = EditorGUILayout.Vector3Field("Scale", _buildingScale);

                    if (GUILayout.Button("�l��K�p") && _selectBuilding != null)
                    {
                        _selectBuilding.transform.localScale = _buildingScale;
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
            var root = rootVisualElement;
           
            _dataScrollPosition = scroll.scrollPosition;
            GUILayout.Label("����");

            foreach (var target in _prefabList)
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
        _selectBuilding.transform.position = new Vector3(_buildingPoint.x, 0.5f, _buildingPoint.z);

        //�l���Z�b�g
        _buildingPosition = _selectBuilding.transform.position;
        _buildingRotation = _selectBuilding.transform.localEulerAngles;
        _buildingRotation = _selectBuilding.transform.localScale;
    }
}

