using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>CSV�t�@�C����ǂ݃}�b�v�f�[�^���쐬����N���X</summary>
public class GetMapData : MonoBehaviour
{
    [SerializeField, Header("CSV�t�@�C��")] TextAsset _dataTable = default;
    CreateMap _createMap => GetComponent<CreateMap>();

    private void Awake()
    {
        BuildTileData();
    }

    /// <summary>�ǂݍ��񂾃t�@�C�������Ƀf�[�^���쐬���� </summary>
    void BuildTileData()
    {
        System.IO.StringReader sr = new System.IO.StringReader(_dataTable.text);
        var mapSize = sr.ReadLine().Split(',');
        _createMap.Row = int.Parse(mapSize[0]);
        _createMap.Col = int.Parse(mapSize[1]);

        while (true)
        {
            var line = sr.ReadLine();
        
            if (string.IsNullOrEmpty(line))
            {
                Debug.Log("finish");
                break;
            }

            var values = line.Split(",");
            CreateTileData(values);
        }
    }

    /// <summary>�󂯎�����f�[�^������TileData�̍\���̂��쐬���� </summary>
    void CreateTileData(string[] target)
    {
        //for (var i = 0; i < target.Length; i++)
        //{
        //    var values = target[i].Split('-');
        //    var status = (TileID)int.Parse(values[0]);
        //    var tileData = new TileData(status, float.Parse(values[1]));
        //    _createMap.TileDataList.Add(tileData);
        //}
    }
}

/// <summary>�^�C�����</summary>
public struct TileData
{
    /// <summary>�^�C���̎��</summary>
   // public TileID TileStatus;
    /// <summary>��]�l </summary>
    public float RotationValue;

    //public TileData(TileID status, float value)
    //{
    //  //  TileStatus = status;
    //    RotationValue = value;
    //}
}
