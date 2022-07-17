using UnityEngine;

public class HumanMove : MonoBehaviour
{
    [SerializeField, Tooltip("ˆÚ“®‘¬“x")] float _move = 1f;
    [SerializeField, Tooltip("ˆÊ’u“ž’B‚ÆŒ©‚È‚·‹——£")] float _goalDistance = 0.6f;
    [SerializeField] MapTile tile;
    Rigidbody _rb => GetComponent<Rigidbody>();
    int _currentIndex = 1;
    
    void Update()
    {
        var distance = 0f;

        if (_currentIndex < tile.TilePoints.Count)
        {
           distance = Vector3.Distance(transform.position, tile.TilePoints[_currentIndex].transform.position);
        }
       
        if (Input.GetButtonDown("Jump"))
        {
            SetStartPosition();
        }

        if (distance <= _goalDistance)
        {
            _rb.velocity = Vector3.zero;
            _currentIndex++;

            if (_currentIndex < tile.TilePoints.Count)
            {
                SetStartPosition();
            }
        }
    }

    void SetStartPosition()
    {
        var dir = tile.TilePoints[_currentIndex].transform.position - transform.position;
        transform.forward = dir;
        var quaternion = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.rotation = quaternion;
        _rb.velocity = transform.forward * _move;
    }
}
