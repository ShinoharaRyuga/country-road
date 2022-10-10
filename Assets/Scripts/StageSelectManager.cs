using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    List<StageInfo> _stageInfos = new List<StageInfo>();
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    public List<StageInfo> StageInfos { get => _stageInfos; set => _stageInfos = value; }

    private void Awake()
    {
        GameData.GetTiles();
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
