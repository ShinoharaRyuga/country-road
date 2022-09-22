using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>���U���g�֘A�̏������s���N���X </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField] Sprite _starSprite = default;
    [SerializeField] Image[] _resultStarImages = default;
    [SerializeField] TMP_Text _thirdConditionText = default;


    public void SetResult(bool[] array)
    {
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i])
            {
                _resultStarImages[i].sprite = _starSprite;
            }
        }
    }

    /// <summary>�\�����鐯�l��������ύX���� </summary>
    public void ChangeThirdText(int count)
    {
        _thirdConditionText.text = $"{count}��ȓ��ɃN���A";
    }
}
