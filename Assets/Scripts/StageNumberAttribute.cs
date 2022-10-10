using UnityEngine;

/// <summary>
/// StageSelectManagerで使用
/// リストの各要素名を保持する
/// </summary>
public class StageNumberAttribute : PropertyAttribute
{
   public readonly string[] _stageNumbers;

    public StageNumberAttribute(string[] numbers) { this._stageNumbers = numbers; }
}
