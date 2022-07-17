using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CreateLine : MonoBehaviour
{
    [SerializeField] Transform _player = default;
    LineRenderer _lr => GetComponent<LineRenderer>();

    void Start()
    {
        var points = new Vector3[3]
        {
            new Vector3(0, 2, 1),
            new Vector3(0, 2, 5),
            new Vector3(0, 2, 10),
        };

        _lr.positionCount = points.Length;
        _lr.SetPositions(points);
    }

    void Update()
    {
        
    }
}
