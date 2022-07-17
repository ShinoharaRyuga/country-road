using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    int _connectionNumber = 0;
    /// <summary>�^�C���̒��S�Əo�����̈ʒu </summary>
    List<Transform> _tilePoints = new List<Transform>();
    public List<Transform> TilePoints { get => _tilePoints; }

    void Start()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            _tilePoints.Add(transform.GetChild(i));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(_connectionNumber);

            if (_connectionNumber >= 2)
            {
                Debug.Log("�q�����Ă���");
            }
            else
            {
                Debug.Log("�q�����Ă��Ȃ�");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       //Debug.Log("enter");
        if (other.gameObject.CompareTag("Point"))
        {
            _connectionNumber++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");
        if (other.gameObject.CompareTag("Point"))
        {
            _connectionNumber--;
        }
    }
}
