using System.Linq;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR



[CustomPropertyDrawer(typeof(VarRename))]
public class PropertyDataDrawer : PropertyDrawer
{
    protected virtual VarRename Attribute
    {
        get
        {
            return (VarRename)attribute;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private SerializedProperty myIterator;
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        label.text = Attribute.g_VarName;
        if (Attribute.g_StrList.Length > 0)
        {
            try
            {
                int _Pos = int.Parse(property.propertyPath.Split('[').LastOrDefault().TrimEnd(']'));
                EditorGUI.PropertyField(position, property, new GUIContent(ObjectNames.NicifyVariableName(Attribute.g_StrList[_Pos])), property.isExpanded);
            }
            catch
            {
                EditorGUI.PropertyField(position, property, label, property.isExpanded);
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }
    }
}

#endif