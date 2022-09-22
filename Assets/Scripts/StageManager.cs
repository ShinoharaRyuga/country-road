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
    [SerializeField] ResultManager _resultManager = default; 
    [SerializeField] HumanMove _humanPrefab = default;
    [SerializeField] TMP_Text _timerText = default;
    Transform _spawnPoint = default;
    TileBase _startTile = default;
    /// <summary>ステージ上にいる街人数 </summary>
    int _currentHumanNumber = 0;
    /// <summary>現在のライフ </summary>
    int _currentLife = 0;
    bool _isGameStart = false;
    bool _isCountDownStart = false;
    /// <summary>ステージクリア </summary>
    bool _isStageClear = false;
    /// <summary>三つ目の星獲得条件</summary>
    bool thirdCondition = false;
    float _timer = 0;



    Pathfinding GetPathfinding => GetComponent<Pathfinding>();

    public int CurrentHumanNumber { get => _currentHumanNumber; set => _currentHumanNumber = value; }

    public bool IsPerfect => _currentLife == _life;

    public bool IsStageClear { get => _isStageClear; }

    void Start()
    {
        _currentHumanNumber = _humanNumber;
        _currentLife = _life;
        _timer = COUNTDOWN_TIME;
    }

    private void Update()
    {
        if (!_isGameStart)
        {
            GameStart();
        }

        if (_isCountDownStart)
        {
            _timer -= Time.deltaTime;
            _timerText.text = _timer.ToString("F0");

            if (_timer < START_TIME)   //ゲームスタート
            {
                _isCountDownStart = false;
                _isGameStart = true;
                _timerText.text = "START!";
                StartCoroutine(HumanGenerator());
                StartCoroutine(ChangeTimerTextActive());
            }
        }
    }

    public void StageClear()
    {
        _isStageClear = true;
        _timerText.text = "CLEAR!!";
        _timerText.gameObject.SetActive(true);
        var resultCanvas =  Instantiate(_resultManager);
        var array = new bool[] { _isStageClear, IsPerfect, thirdCondition };
        resultCanvas.SetResult(array);
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

    void GameStart()
    {
        _startTile = GetPathfinding.StartTile;
        _spawnPoint = _startTile.transform.GetChild(0);
        _isCountDownStart = true;
    }
}
