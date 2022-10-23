using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StageSelectManager))]
public class GetStageInfo : MonoBehaviour
{
    [StageNumberAttribute(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" })]
    [SerializeField, Tooltip("各ステージ情報のリスト")] List<TextAsset> _stageInfoList = new List<TextAsset>();

    private void Awake()
    {
        BuildStageInfo();
    }

    /// <summary>ステージ情報のリストを作成する </summary>
    void BuildStageInfo()
    {
        foreach (var stageData in _stageInfoList)
        {
            System.IO.StringReader sr = new System.IO.StringReader(stageData.text);

            var name = sr.ReadLine().Split(',');    //ステージ名取得
            var stageName = name[0];

            var destinations = GetDestinations(sr.ReadLine().Split(','));   //目的地取得

            var lifeData = sr.ReadLine().Split(',');    //ライフ取得
            var life = int.Parse(lifeData[0]);

            var peopleData = sr.ReadLine().Split(',');  //登場街人数取得
            var peopleCount = int.Parse(peopleData[0]);

            var stageTileData = sr.ReadLine().Split(',');   //ステージセレクト画面で表示するタイルを取得
            var stageTileNumber = int.Parse(stageTileData[0]);

            var conditionsData = sr.ReadLine().Split(','); //星獲得条件を取得
            var conditions = GetConditions(conditionsData);

            //取得したデータを元にステージ情報を作成しリストに追加する
            var stageInfo = new StageInfo(stageName, destinations, life, peopleCount, stageTileNumber, conditions); 
            GameData.StageInfos.Add(stageInfo);
        }

        Debug.Log("finish");
    }

    /// <summary>目的地を取得 </summary>
    string[] GetDestinations(string[] destinationData)
    {
        var destinations = new List<string>();

        for (var i = 0; i < destinationData.Length; i++)
        {
            if (!string.IsNullOrEmpty(destinationData[i]))
            {
                destinations.Add(destinationData[i]);
            }  
        }

        return destinations.ToArray();
    }

    /// <summary>星獲得条件を取得 </summary>
    int[] GetConditions(string[] conditionsData)
    {
        var conditions = new int[conditionsData.Length];

        for (var i = 0; i < conditions.Length; i++)
        {
            conditions[i] = int.Parse(conditionsData[i]);
        }

        return conditions;
    }
}
