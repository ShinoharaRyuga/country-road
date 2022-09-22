using UnityEngine;
using UnityEngine.UI;

/// <summary>リザルト関連の処理を行うクラス </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField] Sprite _starSprite = default;
    [SerializeField] Image[] _resultStarImages = default;


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
}
