using UnityEngine;

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
    [SerializeField, Header("ステージ選択で表示するタイル")]
    TileController _stageSelectTile = default;
    [SerializeField, Header("スタートタイル")]
    TileController[] _startTiles = default;
    [SerializeField, Header("ゴールタイル")]
    TileController[] _goalTiles = default;
    [SerializeField, Header("一つ目の星獲得条件")]
    GetStarCondition _firstCondition = GetStarCondition.StageClear;
    [SerializeField, Header("二つ目の星獲得条件")]
    GetStarCondition _secondCondition = GetStarCondition.Perfect;
    [SerializeField, Header("三つ目の星獲得条件")]
    GetStarCondition _thirdCondition = GetStarCondition.MoveCountLess;

    public int StageNumber { get => _stageNumber; }
    public string StageName { get => _stageName; }
    /// <summary>目的地の名前</summary>
    public string DestinationName { get => _destinationName; }
    public int Life { get => _life; }
    /// <summary>登場人数</summary>
    public int PeopleCount { get => _peopleCount; set => _peopleCount = value; }
    /// <summary>ステージ選択で表示するタイル</summary>
    public TileController StageSelectTile { get => _stageSelectTile; }
    public TileController[] StartTiles { get => _startTiles; }
    public TileController[] GoalTiles { get => _goalTiles; }
    /// <summary>一つ目の星獲得条件 </summary>
    public GetStarCondition FirstCondition { get => _firstCondition; }
    /// <summary>二つ目の星獲得条件 </summary>
    public GetStarCondition SecondCondition { get => _secondCondition; }
    /// <summary>三つ目の星獲得条件 </summary>
    public GetStarCondition ThirdCondition { get => _thirdCondition; set => _thirdCondition = value; }
}

