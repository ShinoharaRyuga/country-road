using UnityEngine;
using UnityEditor;

/// <summary>StageSelectManager�ɂ��郊�X�g�̗v�f����ύX����</summary>
[CustomPropertyDrawer(typeof(StageNumberAttribute))]
public class StageNumbersDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.PropertyField(position, property, new GUIContent(((StageNumberAttribute)attribute)._stageNumbers[pos]));

        }
        catch
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
