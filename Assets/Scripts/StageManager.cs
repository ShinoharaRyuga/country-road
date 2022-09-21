using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>一ステージを管理するクラス </summary>
public class StageManager : MonoBehaviour
{
    const float COUNTDOWN_TIME = 3;
    const float TIMER_TEXT_ACTIVE_TIME = 1;
    const float START_TIME = 0.5f;

    [SerializeField, Header("登場人数")] int _humanNumber = 0;
    [SerializeField, Header("ライフ")] int _life = 0;
    [SerializeField, Header("生成間隔")] float _generateInterval = 0;
    [SerializeField] TileBase _startTile = default;
    [SerializeField] HumanMove _humanPrefab = default;
    [SerializeField] TMP_Text _timerText = default;
    Transform _spawnPoint = default;
    bool _isGameStart = false;
    bool _isCountDownStart = false;
    float _timer = 0;

    Pathfinding GetPathfinding => GetComponent<Pathfinding>();
    void Start()
    {
       
        _timer = COUNTDOWN_TIME;
    }

    private void Update()
    {
        if (_isCountDownStart)
        {
            _timer -= Time.deltaTime;
            _timerText.text = _timer.ToString("F0");

            if (_timer < START_TIME)   //ゲームスタート
            {

                Debug.Log("Start");
                _isCountDownStart = false;
                _isGameStart = true;
                _timerText.text = "Start!";
                StartCoroutine(HumanGenerator());
                StartCoroutine(ChangeTimerTextActive());
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _startTile = GetPathfinding.StartTile;
            _spawnPoint = _startTile.transform.GetChild(0);
            _isCountDownStart = true;
        }
    }

    /// <summary>一定時間ごとに街人を生成する </summary>
    IEnumerator HumanGenerator()
    {
        var count = 1;

        while (true)
        {
            var human = Instantiate(_humanPrefab, _spawnPoint.position, Quaternion.identity);
            human.CurrentTile = _startTile;
            human.transform.forward = _spawnPoint.forward;

            if (_humanNumber <= count)
            {
                break;
            }

            count++;
            yield return new WaitForSeconds(_generateInterval);
        }
    }

    /// <summary>タイマーテキストを非表示にする </summary>
    IEnumerator ChangeTimerTextActive()
    {
        yield return new WaitForSeconds(TIMER_TEXT_ACTIVE_TIME);
        _timerText.gameObject.SetActive(false);
    }
}
