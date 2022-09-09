using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C���̊��N���X
/// �@T���H��Ȃ���p�Ȃǂ�h�������č쐬����  
/// </summary>
public class TileBase : MonoBehaviour
{
    /// <summary>�^�C���ɏ���Ă���X�l�̃��X�g </summary>
    List<HumanMove> _onHumans = new List<HumanMove>();
    /// <summary>�q�����Ă���^�C�� </summary>
    Dictionary<ConnectingTiles, TileBase> _connectingTiles = new Dictionary<ConnectingTiles, TileBase>();

    public List<HumanMove> OnHumans { get => _onHumans; set => _onHumans = value; }
    public Dictionary<ConnectingTiles, TileBase> ConnectingTiles { get => _connectingTiles; set => _connectingTiles = value; }

    void Start()
    {
        //�e�|�C���g���擾����
        for (var i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ConnectingTiles tiles))
            {
                if (!tiles.IsMiddle)
                {
                    _connectingTiles.Add(tiles, null);
                }
            }
        }
    }

    /// <summary>�ڑ����ꂽ�^�C����_connectingTiles�ɒǉ����� </summary>
    public void AddConnectedTile(ConnectingTiles key, TileBase tile)
    {
        if (_connectingTiles.ContainsKey(key))
        {
            _connectingTiles[key] = tile;
        }
    }
}

public enum PointStatus
{
    First = 0,
    Second = 1,
    Third = 2,
    Fourth = 3,
    None = 4,
    Middle = 5,

    //���Ƃō폜
    Start = 99,
    End = 98,
}
