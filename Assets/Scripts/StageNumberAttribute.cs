using UnityEngine;

/// <summary>
/// StageSelectManager�Ŏg�p
/// ���X�g�̊e�v�f����ێ�����
/// </summary>
public class StageNumberAttribute : PropertyAttribute
{
   public readonly string[] _stageNumbers;

    public StageNumberAttribute(string[] numbers) { this._stageNumbers = numbers; }
}
