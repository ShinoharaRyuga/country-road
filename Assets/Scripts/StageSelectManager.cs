using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    /// <summary>各ステージ情報のリスト </summary>
    List<StageInfo> _stageInfos = new List<StageInfo>();
    /// <summary>各ステージ情報のリスト </summary>
    public List<StageInfo> StageInfos { get => _stageInfos; set => _stageInfos = value; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            var stageInfo = StageInfos[0];

            Debug.Log(stageInfo._stageName);
            foreach (var name in stageInfo._destinationsName)
            {
                Debug.Log(name);
            }
            Debug.Log(stageInfo._life);
            Debug.Log(stageInfo._peopleCount);
            Debug.Log(stageInfo._stageTile);

            foreach (var condition in stageInfo.getStarConditions)
            {
                Debug.Log(condition);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            var stageInfo = StageInfos[1];

            Debug.Log(stageInfo._stageName);
            foreach (var name in stageInfo._destinationsName)
            {
                Debug.Log(name);
            }
            Debug.Log(stageInfo._life);
            Debug.Log(stageInfo._peopleCount);
            Debug.Log(stageInfo._stageTile);

            foreach (var condition in stageInfo.getStarConditions)
            {
                Debug.Log(condition);
            }
        }
    }
}

/// <summary>ステージ情報 </summary>
public struct StageInfo
{
    /// <summary>ステージ名 </summary>
    public string _stageName;
    /// <summary>目的地 </summary>
    public string[] _destinationsName;
    /// <summary>ライフ </summary>
    public int _life;
    /// <summary>登場街人数 </summary>
    public int _peopleCount;
    /// <summary>ステージセレクト画面で表示するタイル </summary>
    public int _stageTile;
    /// <summary>星獲得条件 </summary>
    public int[] getStarConditions;

    public StageInfo(string stageName, string[] destinations, int life, int people, int stageTile, int[] conditions)
    {
        _stageName = stageName;
        _destinationsName = destinations;
        _life = life;
        _peopleCount = people;
        _stageTile = stageTile;
        getStarConditions = conditions;
    }
}
