using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>��X�e�[�W���Ǘ�����N���X </summary>
public class StageManager : MonoBehaviour
{
    const float COUNTDOWN_TIME = 3;
    const float TIMER_TEXT_ACTIVE_TIME = 1;
    const float START_TIME = 0.5f;

    [SerializeField, Header("�X�e�[�W���")] StageParameter _stageParameter = default;
    [SerializeField, Header("�����Ԋu")] float _generateInterval = 0;
    [SerializeField] ResultManager _resultManager = default; 
    [SerializeField] HumanMove _humanPrefab = default;
    [SerializeField] TMP_Text _timerText = default;
    [SerializeField] TMP_Text _lifeText = default;
    [SerializeField] TMP_Text _moveCountText = default;
    Transform _spawnPoint = default;
    TileController _startTile = default;
    /// <summary>�X�e�[�W��ɂ���X�l�� </summary>
    int _currentHumanNumber = 0;
    /// <summary>���݂̃��C�t </summary>
    int _currentLife = 0;
    /// <summary>�^�C���𓮂������� </summary>
    int _moveCount = 0;
    bool _isGameStart = false;
    bool _isCountDownStart = false;
    /// <summary>�X�e�[�W�N���A </summary>
    bool _isStageClear = false;
    /// <summary>�O�ڂ̐��l������</summary>
    bool _thirdCondition = false;
    float _timer = 0;

    CreateMap _createMap => GetComponent<CreateMap>();
    public int CurrentHumanNumber { get => _currentHumanNumber; set => _currentHumanNumber = value; }
    public bool IsPerfect => _currentLife == _stageParameter.Life;
    public bool IsStageClear { get => _isStageClear; }
    public int MoveCount { get => _moveCount; set => _moveCount = value; }

    void Start()
    {
        _currentHumanNumber = _stageParameter.PeopleCount;
        _currentLife = _stageParameter.Life;
        _timer = COUNTDOWN_TIME;
        _lifeText.text = _currentLife.ToString();
    }

    private void Update()
    {
        if (_isCountDownStart)
        {
            _timer -= Time.deltaTime;
            _timerText.text = _timer.ToString("F0");

            if (_timer < START_TIME)   //�Q�[���X�^�[�g
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
        Debug.Log($"�� {_moveCount}");
        if (_moveCount <= _stageParameter.StarGetCount)
        {
            _thirdCondition = true;
        }

        _isStageClear = true;
        _timerText.text = "CLEAR!!";
        _timerText.gameObject.SetActive(true);
        var resultCanvas =  Instantiate(_resultManager);
        var array = new bool[] { _isStageClear, IsPerfect, _thirdCondition };
        resultCanvas.SetResult(array);
        resultCanvas.ChangeThirdText(_stageParameter.StarGetCount);
    }

    /// <summary>��莞�Ԃ��ƂɊX�l�𐶐����� </summary>
    IEnumerator HumanGenerator()
    {
        var count = 1;

        while (true)
        {
            var human = Instantiate(_humanPrefab, _spawnPoint.position, Quaternion.identity);
            human.CurrentTile = _startTile;
            human.transform.forward = _spawnPoint.forward;

            if (_stageParameter.PeopleCount <= count)
            {
                break;
            }

            count++;
            yield return new WaitForSeconds(_generateInterval);
        }
    }

    /// <summary>�^�C�}�[�e�L�X�g���\���ɂ��� </summary>
    IEnumerator ChangeTimerTextActive()
    {
        yield return new WaitForSeconds(TIMER_TEXT_ACTIVE_TIME);
        _timerText.gameObject.SetActive(false);
    }

    /// <summary>�Q�[�����X�^�[�g������</summary>
    public void GameStart()
    {
        _startTile = _createMap.StartTile;
        _spawnPoint = _startTile.transform.GetChild(0);
        _isCountDownStart = true;
    }

    public void AddMoveCount()
    {
        _moveCount++;
        _moveCountText.text = _moveCount.ToString();
    }

    public void Damage()
    {
        _currentLife--;
        _lifeText.text = _currentLife.ToString();
    }
}
