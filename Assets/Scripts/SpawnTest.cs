using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    [SerializeField] MapTile _targetTile = default;
    [SerializeField] HumanMove _humanPrefab = default;
    int[] numbers = new int[2] { 0, 2 };
    HumanMove _human = default;
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_human != null)
            {
                Destroy(_human.gameObject);
            }

            var index = Random.Range(0, 2);
            index = numbers[index];
            _human = Instantiate(_humanPrefab, _targetTile.TilePoints[index].transform.position, Quaternion.identity);
            _human.CurrentMapTile = _targetTile;
        }
    }
}
