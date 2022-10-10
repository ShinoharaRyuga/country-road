using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField, Tooltip("�`���[�g���A���X�e�[�W")] GameObject _tutorialStageTile = default;
    [SerializeField, Tooltip("�J����")] Transform _cameraTransform = default;
    [SerializeField, Tooltip("���X�e�[�W�Ɉړ�����{�^��")] GameObject _nextButton = default;
    [SerializeField, Tooltip("�O�X�e�[�W�Ɉړ�����{�^��")] GameObject _previousButton = default;
    [SerializeField, Tooltip("���l��������\������{�^���̃e�L�X�g")] TMP_Text _getStarButtonText = default;

    /// <summary>���݂̃X�e�[�W </summary>
    int _currentStageNumber = 0;
    /// <summary>���l�������̃L�����o�X��\�����Ă��邩�ǂ��� </summary>
    bool _isStarCanvas = false;
    /// <summary>���݂̃X�e�[�W�^�C��</summary>
    GameObject _currentStageTile = default;
    /// <summary>�e�X�e�[�W�̃^�C�� </summary>
    List<GameObject> _stageTiles = new List<GameObject>();
    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    List<StageInfo> _stageInfos = new List<StageInfo>();


    /// <summary>�e�X�e�[�W���̃��X�g </summary>
    public List<StageInfo> StageInfos { get => _stageInfos; set => _stageInfos = value; }

    private void Awake()
    {
        GameData.GetTiles();
    }

    public void Start()
    {
        _stageTiles.Add(_tutorialStageTile);
        var tilePosX = TILE_POSITION_X;

        foreach (var stageInfo in _stageInfos)  //�X�e�[�W�I��p�^�C���𐶐�
        {
            var targetTile = (StageSelectTiles)stageInfo._stageTile;
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

        CurrentTileRotate();
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

    /// <summary>���X�e�[�W�Ɉړ����� </summary>
    public void NextStage()
    {
        if (_currentStageNumber < _stageTiles.Count - 1)
        {
            _currentStageNumber++;
            _currentStageTile = _stageTiles[_currentStageNumber];
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
    }

    /// <summary>�O�X�e�[�W�Ɉړ�����</summary>
    public void PreviousStage()
    {
        if (0 < _currentStageNumber)
        {
            _currentStageNumber--;
            _currentStageTile = _stageTiles[_currentStageNumber];
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
