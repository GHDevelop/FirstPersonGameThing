using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Solution used for Visible Only Properties found at https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html in post from user FuzzyLogic
[CustomPropertyDrawer(typeof(VisibleOnlyAttribute))]
public class VisibleOnlyDrawer : UnityEditor.PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (EditorGUI.DisabledGroupScope scope = new EditorGUI.DisabledGroupScope(true))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}

[CustomPropertyDrawer(typeof(BeginVisibleOnlyGroupAttribute))]
public class BeginVisibleOnlyGroupDrawer : UnityEditor.DecoratorDrawer
{
    public override float GetHeight()
    {
        return 0;
    }

    public override void OnGUI(Rect position)
    {
        EditorGUI.BeginDisabledGroup(true);
    }
}

[CustomPropertyDrawer(typeof(EndVisibleOnlyGroupAttribute))]
public class EndVisibleOnlyGroupDrawer : UnityEditor.DecoratorDrawer
{
    public override float GetHeight()
    {
        return 0;
    }

    public override void OnGUI(Rect position)
    {
        EditorGUI.EndDisabledGroup();
    }
}
