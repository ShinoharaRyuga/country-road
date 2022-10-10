using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    List<StageInfo> _stageInfos = new List<StageInfo>();
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
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

/// <summary>�X�e�[�W��� </summary>
public struct StageInfo
{
    /// <summary>�X�e�[�W�� </summary>
    public string _stageName;
    /// <summary>�ړI�n </summary>
    public string[] _destinationsName;
    /// <summary>���C�t </summary>
    public int _life;
    /// <summary>�o��X�l�� </summary>
    public int _peopleCount;
    /// <summary>�X�e�[�W�Z���N�g��ʂŕ\������^�C�� </summary>
    public int _stageTile;
    /// <summary>���l������ </summary>
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
