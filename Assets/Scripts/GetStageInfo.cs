using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StageSelectManager))]
public class GetStageInfo : MonoBehaviour
{
    [StageNumberAttribute(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" })]
    [SerializeField, Tooltip("�e�X�e�[�W���̃��X�g")] List<TextAsset> _stageInfoList = new List<TextAsset>();

    private void Awake()
    {
        BuildStageInfo();
    }

    /// <summary>�X�e�[�W���̃��X�g���쐬���� </summary>
    void BuildStageInfo()
    {
        foreach (var stageData in _stageInfoList)
        {
            System.IO.StringReader sr = new System.IO.StringReader(stageData.text);

            var name = sr.ReadLine().Split(',');    //�X�e�[�W���擾
            var stageName = name[0];

            var destinations = GetDestinations(sr.ReadLine().Split(','));   //�ړI�n�擾

            var lifeData = sr.ReadLine().Split(',');    //���C�t�擾
            var life = int.Parse(lifeData[0]);

            var peopleData = sr.ReadLine().Split(',');  //�o��X�l���擾
            var peopleCount = int.Parse(peopleData[0]);

            var stageTileData = sr.ReadLine().Split(',');   //�X�e�[�W�Z���N�g��ʂŕ\������^�C�����擾
            var stageTileNumber = int.Parse(stageTileData[0]);

            var conditionsData = sr.ReadLine().Split(','); //���l���������擾
            var conditions = GetConditions(conditionsData);

            //�擾�����f�[�^�����ɃX�e�[�W�����쐬�����X�g�ɒǉ�����
            var stageInfo = new StageInfo(stageName, destinations, life, peopleCount, stageTileNumber, conditions); 
            GameData.StageInfos.Add(stageInfo);
        }

        Debug.Log("finish");
    }

    /// <summary>�ړI�n���擾 </summary>
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

    /// <summary>���l���������擾 </summary>
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
