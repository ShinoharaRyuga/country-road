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

    [SerializeField, Header("�e�X�e�[�W���")] StageParameter[] _stageParameters = default;

    /// <summary>���݂̃X�e�[�W </summary>
    int _currentStageNumber = 0;
    /// <summary>���l�������̃L�����o�X��\�����Ă��邩�ǂ��� </summary>
    bool _isStarCanvas = false;
    /// <summary>���݂̃X�e�[�W��� </summary>
    StageParameter _currentStageParameter = default;
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

        foreach (var stage in _stageParameters) //�X�e�[�W�I��p�^�C���𐶐�
        {
            var targetTile = stage.StageSelectTile;
            var tile = Instantiate(targetTile, Vector3.zero, Quaternion.Euler(0, 180, 0));
            tile.transform.GetChild(1).gameObject.SetActive(false);

            //�ʒu�A��]�A�傫���̒���
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
        _stageNumberText.text = _currentStageParameter.StageNumber.ToString();  //�X�e�[�W��
        _stageNameText.text = _currentStageParameter.StageName;                 //�X�e�[�W��
        _destinationsText.text = _currentStageParameter.DestinationName;        //�ړI�n
        _lifeText.text = $"{_currentStageParameter.Life}��";                //���C�t
        _peopleCountText.text = $"{_currentStageParameter.PeopleCount}�l";  //�o��X�l��
    }

    /// <summary>���X�e�[�W�Ɉړ����� </summary>
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
            _currentStageParameter = _stageParameters[_currentStageNumber];
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
        SceneManager.LoadScene(_currentStageParameter.ReadSceneName);
    }
}
