using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

/// <summary>ステージ選択画面を管理するクラス </summary>
public class StageSelectManager : MonoBehaviour
{
    /// <summary>ステージの間隔 </summary>
    const float TILE_POSITION_X = 7;
    /// <summary>カメラ移動にかかる時間 </summary>
    const float CAMERA_MOVE_TIME = 0.3f;
    /// <summary>タイルの回転が始まるまでの時間 </summary>
    const float ROTATE_WAIT_TIME = 1f;
    /// <summary>タイルの回転速度 </summary>
    const float ROTATE_SPEED = 3f;

    [SerializeField, Tooltip("カメラ")] Transform _cameraTransform = default;
    [SerializeField, Tooltip("次ステージに移動するボタン")] GameObject _nextButton = default;
    [SerializeField, Tooltip("前ステージに移動するボタン")] GameObject _previousButton = default;
    //===UI更新用===
    [SerializeField, Tooltip("星獲得条件を表示するボタンのテキスト")] TMP_Text _getStarButtonText = default;
    [SerializeField, Tooltip("現在のステージ番号を表示するテキスト")] TMP_Text _stageNumberText = default;
    [SerializeField, Tooltip("ステージ名表示するテキスト")] TMP_Text _stageNameText = default;
    [SerializeField, Tooltip("目的地を表示するテキスト")] TMP_Text _destinationsText = default;
    [SerializeField, Tooltip("ライフを表示するテキスト")] TMP_Text _lifeText = default;
    [SerializeField, Tooltip("登場街人数を表示するテキスト")] TMP_Text _peopleCountText = default;
    //===★獲得条件更新===
    [SerializeField, Tooltip("一つ目")] TMP_Text _firstConditionText = default;
    [SerializeField, Tooltip("二つ目")] TMP_Text _secondConditionText = default;
    [SerializeField, Tooltip("三つ目")] TMP_Text _thirdConditionText = default;

    [SerializeField, Header("各ステージ情報")] StageParameter[] _stageParameters = default;

    /// <summary>現在のステージ </summary>
    int _currentStageNumber = 0;
    /// <summary>星獲得条件のキャンバスを表示しているかどうか </summary>
    bool _isStarCanvas = false;
    /// <summary>現在のステージ情報 </summary>
    StageParameter _currentStageParameter = default;
    /// <summary>現在のステージタイル</summary>
    GameObject _currentStageTile = default;
    /// <summary>各ステージのタイル </summary>
    List<GameObject> _stageTiles = new List<GameObject>();

    private void Awake()
    {
        GameData.GetTiles();
    }

    public void Start()
    {
        var tilePosX = 0f;

        foreach (var stage in _stageParameters) //ステージ選択用タイルを生成
        {
            var targetTile = stage.StageSelectTile;
            var tile = Instantiate(targetTile, Vector3.zero, Quaternion.Euler(0, 180, 0));
            tile.transform.GetChild(1).gameObject.SetActive(false);

            //位置、回転、大きさの調整
            tile.transform.position = new Vector3(tilePosX, 0, 1);
            tile.transform.rotation = Quaternion.Euler(0, 180, 0);
            tile.transform.localScale = new Vector3(3, 0.6f, 3);
            tilePosX += TILE_POSITION_X;
            _stageTiles.Add(targetTile.gameObject);
        }

        _currentStageTile = _stageTiles[0];
        _currentStageParameter = _stageParameters[0];
        CurrentTileRotate();
        UIUpdate();
    }

    /// <summary>選択されているタイルを回転させる </summary>
    void CurrentTileRotate()
    {
        _currentStageTile.transform.DORotate(new Vector3(0, 360, 0), ROTATE_SPEED)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetDelay(ROTATE_WAIT_TIME)
            .SetLoops(-1);
    }

    /// <summary>UIを更新する </summary>
    void UIUpdate()
    {
        _stageNumberText.text = _currentStageParameter.StageNumber.ToString();  //ステージ数
        _stageNameText.text = _currentStageParameter.StageName;                 //ステージ名
        _destinationsText.text = _currentStageParameter.DestinationName;        //目的地
        _lifeText.text = $"{_currentStageParameter.Life}個";                //ライフ
        _peopleCountText.text = $"{_currentStageParameter.PeopleCount}人";  //登場街人数
    }

    /// <summary>次ステージに移動する </summary>
    public void NextStage()
    {
        if (_currentStageNumber < _stageTiles.Count - 1)
        {
            _currentStageNumber++;
            _currentStageTile = _stageTiles[_currentStageNumber];
            _currentStageParameter = _stageParameters[_currentStageNumber];
            var cameraPosX = _cameraTransform.position.x + TILE_POSITION_X;
            _cameraTransform.DOMoveX(cameraPosX, CAMERA_MOVE_TIME);
        }

        //ボタン表示・非表示の処理
        if (_currentStageNumber == _stageTiles.Count - 1)
        {
            _nextButton.SetActive(false);
        }

        if (!_previousButton.activeSelf)
        {
            _previousButton.SetActive(true);
        }

        CurrentTileRotate();
        UIUpdate();
    }

    /// <summary>前ステージに移動する</summary>
    public void PreviousStage()
    {
        if (0 < _currentStageNumber)
        {
            _currentStageNumber--;
            _currentStageTile = _stageTiles[_currentStageNumber];
            _currentStageParameter = _stageParameters[_currentStageNumber];
            var cameraPosX = _cameraTransform.position.x - TILE_POSITION_X;
            _cameraTransform.DOMoveX(cameraPosX, CAMERA_MOVE_TIME);
        }

        //ボタン表示・非表示の処理
        if (0 == _currentStageNumber)
        {
            _previousButton.SetActive(false);
        }

        if (!_nextButton.activeSelf)
        {
            _nextButton.SetActive(true);
        }

        CurrentTileRotate();
        UIUpdate();
    }

    /// <summary>★獲得条件を表示するボタンのテキストを変更する </summary>
    public void ChangeGetStarText(GameObject starCanvas)
    {
        if (_isStarCanvas)
        {
            _getStarButtonText.text = "★獲得条件";

        }
        else
        {
            _getStarButtonText.text = "閉じる";
        }

        _isStarCanvas = !_isStarCanvas;
        starCanvas.SetActive(_isStarCanvas);
    }

    /// <summary>ゲームシーンに遷移する </summary>
    public void TransitionGameScene()
    {
        SceneManager.LoadScene(_currentStageParameter.ReadSceneName);
    }
}
