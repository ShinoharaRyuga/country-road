//ゲーム内データをまとめておく
//CSVから設定させる為

/// <summary>ステージセレクト画面で使用するタイル </summary>
public enum StageSelectTiles
{
    School = 0,
}

/// <summary>ステージで星を獲得する為の条件 </summary>
public enum GetStarCondition
{
    /// <summary>ステージクリア </summary>
    StageClear = 0,
    /// <summary>ノーミスクリア</summary>
    Perfect = 1,
    /// <summary>○手以内にクリア </summary>
    MoveCountLess = 2,
}


