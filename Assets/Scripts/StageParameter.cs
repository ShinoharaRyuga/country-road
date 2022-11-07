using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>ステージを作成する為のクラス </summary>
[CreateAssetMenu]
public class StageParameter : ScriptableObject
{
    [SerializeField, Header("ステージ番号")]
    int _stageNumber = default;
    [SerializeField, Header("ステージ名")]
    string _stageName = "";
    [SerializeField, Header("目的地")]
    string _destinationName = default;
    [SerializeField, Header("ライフ")]
    int _life = 0;
    [SerializeField, Header("登場人数")]
    int _peopleCount = 0;
    [SerializeField, Header("星獲得移動回数")]
    int _starGetCount = 0;
    [SerializeField, Header("ステージ選択で表示するタイル")]
    TileController _stageSelectTile = default;
    [SerializeField, Header("一つ目の星獲得条件")]
    GetStarCondition _firstCondition = GetStarCondition.StageClear;
    [SerializeField, Header("二つ目の星獲得条件")]
    GetStarCondition _secondCondition = GetStarCondition.Perfect;
    [SerializeField, Header("三つ目の星獲得条件")]
    GetStarCondition _thirdCondition = GetStarCondition.MoveCountLess;
    [SerializeField, Header("読み込むシーンの名前")]
    string _readSceneName;

    public int StageNumber { get => _stageNumber; }
    public string StageName { get => _stageName; }
    /// <summary>目的地の名前</summary>
    public string DestinationName { get => _destinationName; }
    public int Life { get => _life; }
    /// <summary>登場人数</summary>
    public int PeopleCount { get => _peopleCount; set => _peopleCount = value; }
    /// <summary>星獲得移動回数 </summary>
    public int StarGetCount { get => _starGetCount; }
    /// <summary>ステージ選択で表示するタイル</summary>
    public TileController StageSelectTile { get => _stageSelectTile; }
    /// <summary>一つ目の星獲得条件 </summary>
    public GetStarCondition FirstCondition { get => _firstCondition; }
    /// <summary>二つ目の星獲得条件 </summary>
    public GetStarCondition SecondCondition { get => _secondCondition; }
    /// <summary>三つ目の星獲得条件 </summary>
    public GetStarCondition ThirdCondition { get => _thirdCondition; set => _thirdCondition = value; }
    /// <summary>読み込むシーンの名前 </summary>
    public string ReadSceneName { get => _readSceneName; }
}

