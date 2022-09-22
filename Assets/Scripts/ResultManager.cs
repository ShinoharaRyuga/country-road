using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>リザルト関連の処理を行うクラス </summary>
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

    /// <summary>表示する星獲得条件を変更する </summary>
    public void ChangeThirdText(int count)
    {
        _thirdConditionText.text = $"{count}手以内にクリア";
    }
}
