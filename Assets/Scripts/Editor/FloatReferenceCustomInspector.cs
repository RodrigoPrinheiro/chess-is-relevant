using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using System;
using ModularVariables;

namespace CustomInspectors
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceCustomInspector : PropertyDrawer
    {
        enum FloatChoice{ UseConstant, UseObject}
        private FloatChoice choice = FloatChoice.UseConstant;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var useConstant = property.FindPropertyRelative("_useConstant");
            
            EditorGUI.BeginProperty(position, label, property);
            // Draw field label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Update choice with the value in our object
            choice = useConstant.boolValue ? FloatChoice.UseConstant : FloatChoice.UseObject;
            
            // Enum choice draw rect
            Rect foldoutPos = position; 
            foldoutPos.x -= 30;
            foldoutPos.width = 25;
            foldoutPos.height = 25;
            // Draw enum choice
            choice = (FloatChoice)EditorGUI.EnumPopup(foldoutPos, choice);
            position.height = 20;

            // Draw respective property
            switch(choice)
            {
                case FloatChoice.UseConstant:
                    var constantValue = property.FindPropertyRelative("_constantValue");
                    // Draw Float field
                    EditorGUI.PropertyField(position, constantValue, GUIContent.none);
                    useConstant.boolValue = true;
                    break;
                case FloatChoice.UseObject:
                    var floatReference = property.FindPropertyRelative("_modularValue");
                    // Draw Object Field
                    EditorGUI.PropertyField(position, floatReference, GUIContent.none);
                    useConstant.boolValue = false;
                    break;
            }
            // Reset indent level
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 2;
        }
    }
}