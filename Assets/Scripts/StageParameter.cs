using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>�X�e�[�W���쐬����ׂ̃N���X </summary>
[CreateAssetMenu]
public class StageParameter : ScriptableObject
{
    [SerializeField, Header("�X�e�[�W�ԍ�")]
    int _stageNumber = default;
    [SerializeField, Header("�X�e�[�W��")]
    string _stageName = "";
    [SerializeField, Header("�ړI�n")]
    string _destinationName = default;
    [SerializeField, Header("���C�t")]
    int _life = 0;
    [SerializeField, Header("�o��l��")]
    int _peopleCount = 0;
    [SerializeField, Header("���l���ړ���")]
    int _starGetCount = 0;
    [SerializeField, Header("�X�e�[�W�I���ŕ\������^�C��")]
    TileController _stageSelectTile = default;
    [SerializeField, Header("��ڂ̐��l������")]
    GetStarCondition _firstCondition = GetStarCondition.StageClear;
    [SerializeField, Header("��ڂ̐��l������")]
    GetStarCondition _secondCondition = GetStarCondition.Perfect;
    [SerializeField, Header("�O�ڂ̐��l������")]
    GetStarCondition _thirdCondition = GetStarCondition.MoveCountLess;
    [SerializeField, Header("�ǂݍ��ރV�[���̖��O")]
    string _readSceneName;

    public int StageNumber { get => _stageNumber; }
    public string StageName { get => _stageName; }
    /// <summary>�ړI�n�̖��O</summary>
    public string DestinationName { get => _destinationName; }
    public int Life { get => _life; }
    /// <summary>�o��l��</summary>
    public int PeopleCount { get => _peopleCount; set => _peopleCount = value; }
    /// <summary>���l���ړ��� </summary>
    public int StarGetCount { get => _starGetCount; }
    /// <summary>�X�e�[�W�I���ŕ\������^�C��</summary>
    public TileController StageSelectTile { get => _stageSelectTile; }
    /// <summary>��ڂ̐��l������ </summary>
    public GetStarCondition FirstCondition { get => _firstCondition; }
    /// <summary>��ڂ̐��l������ </summary>
    public GetStarCondition SecondCondition { get => _secondCondition; }
    /// <summary>�O�ڂ̐��l������ </summary>
    public GetStarCondition ThirdCondition { get => _thirdCondition; set => _thirdCondition = value; }
    /// <summary>�ǂݍ��ރV�[���̖��O </summary>
    public string ReadSceneName { get => _readSceneName; }
}

