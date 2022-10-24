using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

/// <summary>�X�e�[�W�I����ʂ��Ǘ�����N���X </summary>
public class StageSelectManager : MonoBehaviour
{
    /// <summary>�X�e�[�W�̊Ԋu </summary>
    const float TILE_POSITION_X = 7;
    /// <summary>�J�����ړ��ɂ����鎞�� </summary>
    const float CAMERA_MOVE_TIME = 0.3f;
    /// <summary>�^�C���̉�]���n�܂�܂ł̎��� </summary>
    const float ROTATE_WAIT_TIME = 1f;
    /// <summary>�^�C���̉�]���x </summary>
    const float ROTATE_SPEED = 3f;

    [SerializeField, Tooltip("�J����")] Transform _cameraTransform = default;
    [SerializeField, Tooltip("���X�e�[�W�Ɉړ�����{�^��")] GameObject _nextButton = default;
    [SerializeField, Tooltip("�O�X�e�[�W�Ɉړ�����{�^��")] GameObject _previousButton = default;
    //===UI�X�V�p===
    [SerializeField, Tooltip("���l��������\������{�^���̃e�L�X�g")] TMP_Text _getStarButtonText = default;
    [SerializeField, Tooltip("���݂̃X�e�[�W�ԍ���\������e�L�X�g")] TMP_Text _stageNumberText = default;
    [SerializeField, Tooltip("�X�e�[�W���\������e�L�X�g")] TMP_Text _stageNameText = default;
    [SerializeField, Tooltip("�ړI�n��\������e�L�X�g")] TMP_Text _destinationsText = default;
    [SerializeField, Tooltip("���C�t��\������e�L�X�g")] TMP_Text _lifeText = default;
    [SerializeField, Tooltip("�o��X�l����\������e�L�X�g")] TMP_Text _peopleCountText = default;
    //===���l�������X�V===
    [SerializeField, Tooltip("���")] TMP_Text _firstConditionText = default;
    [SerializeField, Tooltip("���")] TMP_Text _secondConditionText = default;
    [SerializeField, Tooltip("�O��")] TMP_Text _thirdConditionText = default;

    /// <summary>���݂̃X�e�[�W </summary>
    int _currentStageNumber = 0;
    /// <summary>���l�������̃L�����o�X��\�����Ă��邩�ǂ��� </summary>
    bool _isStarCanvas = false;
    /// <summary>���݂̃X�e�[�W��� </summary>
    StageInfo _currentStageInfo = default;
    /// <summary>���݂̃X�e�[�W�^�C��</summary>
    GameObject _currentStageTile = default;
    /// <summary>�e�X�e�[�W�̃^�C�� </summary>
    List<GameObject> _stageTiles = new List<GameObject>();

    private void Awake()
    {
        GameData.GetTiles();
    }

    public void Start()
    {
        var tilePosX = 0f;

        foreach (var stageInfo in GameData.StageInfos)  //�X�e�[�W�I��p�^�C���𐶐�
        {
            var targetTile = (BuildingType)stageInfo._stageTile;
            var tile = Instantiate(GameData.StageSelectTiles[targetTile], Vector3.zero, Quaternion.Euler(0, 180, 0));
            tile.transform.GetChild(1).gameObject.SetActive(false);

            //�ʒu�A��]�A�傫���̒���
            tile.transform.position = new Vector3(tilePosX, 0, 1);
            tile.transform.rotation = Quaternion.Euler(0, 180, 0);
            tile.transform.localScale = new Vector3(3, 0.6f, 3);
            tilePosX += TILE_POSITION_X;
            _stageTiles.Add(tile);
        }

        _currentStageTile = _stageTiles[0];
        _currentStageInfo = GameData.StageInfos[0];
        CurrentTileRotate();
        UIUpdate();
    }

    /// <summary>�I������Ă���^�C������]������ </summary>
    void CurrentTileRotate()
    {
        _currentStageTile.transform.DORotate(new Vector3(0, 360, 0), ROTATE_SPEED)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetDelay(ROTATE_WAIT_TIME)
            .SetLoops(-1);
    }

    /// <summary>UI���X�V���� </summary>
    void UIUpdate()
    {
        var destinationsText = "";
        Array.ForEach(_currentStageInfo._destinationsName, s => destinationsText += $"{s} ");

        _stageNumberText.text = _currentStageNumber.ToString();     //�X�e�[�W��
        _stageNameText.text = _currentStageInfo._stageName;         //�X�e�[�W��
        _destinationsText.text = destinationsText;                  //�ړI�n
        _lifeText.text = _currentStageInfo._life.ToString();        //���C�t
        _peopleCountText.text = _currentStageInfo._peopleCount.ToString();  //�o��X�l��
    }

    /// <summary>���X�e�[�W�Ɉړ����� </summary>
    public void NextStage()
    {
        if (_currentStageNumber < _stageTiles.Count - 1)
        {
            _currentStageNumber++;
            _currentStageTile = _stageTiles[_currentStageNumber];
            _currentStageInfo = GameData.StageInfos[_currentStageNumber];
            var cameraPosX = _cameraTransform.position.x + TILE_POSITION_X;
            _cameraTransform.DOMoveX(cameraPosX, CAMERA_MOVE_TIME);
        }

        //�{�^���\���E��\���̏���
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

    /// <summary>�O�X�e�[�W�Ɉړ�����</summary>
    public void PreviousStage()
    {
        if (0 < _currentStageNumber)
        {
            _currentStageNumber--;
            _currentStageTile = _stageTiles[_currentStageNumber];
            _currentStageInfo = GameData.StageInfos[_currentStageNumber];
            var cameraPosX = _cameraTransform.position.x - TILE_POSITION_X;
            _cameraTransform.DOMoveX(cameraPosX, CAMERA_MOVE_TIME);
        }

        //�{�^���\���E��\���̏���
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

    /// <summary>���l��������\������{�^���̃e�L�X�g��ύX���� </summary>
    public void ChangeGetStarText(GameObject starCanvas)
    {
        if (_isStarCanvas)
        {
            _getStarButtonText.text = "���l������";

        }
        else
        {
            _getStarButtonText.text = "����";
        }

        _isStarCanvas = !_isStarCanvas;
        starCanvas.SetActive(_isStarCanvas);
    }

    /// <summary>�Q�[���V�[���ɑJ�ڂ��� </summary>
    public void TransitionGameScene()
    {
        SceneManager.LoadScene("DemoScene");
    }
}

/// <summary>�X�e�[�W��� </summary>
public struct StageInfo
{
    /// <summary>�X�e�[�W�� </summary>
    public string _stageName;
    /// <summary>�ړI�n </summary>
    public string[] _destinationsName;
    /// <summary>���C�t </summary>
    public int _life;
    /// <summary>�o��X�l�� </summary>
    public int _peopleCount;
    /// <summary>�X�e�[�W�Z���N�g��ʂŕ\������^�C�� </summary>
    public int _stageTile;
    /// <summary>���l������ </summary>
    public int[] getStarConditions;

    public StageInfo(string stageName, string[] destinations, int life, int people, int stageTile, int[] conditions)
    {
        _stageName = stageName;
        _destinationsName = destinations;
        _life = life;
        _peopleCount = people;
        _stageTile = stageTile;
        getStarConditions = conditions;
    }
}
