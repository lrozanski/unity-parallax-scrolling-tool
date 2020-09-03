using System.Reflection;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [CustomPropertyDrawer(typeof(DisplayAsRange))]
    public class DisplayAsRangeDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldAttribute = (DisplayAsRange) attribute;
            var minValue = GetFieldValue(property, fieldAttribute.minValue, fieldAttribute.minField);
            var maxValue = GetFieldValue(property, fieldAttribute.maxValue, fieldAttribute.maxField);

            EditorGUI.BeginChangeCheck();
            EditorGUI.IntSlider(position, property, minValue, maxValue);

            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private static int GetFieldValue(SerializedProperty serializedProperty, int attributeValue, string attributeField) {
            if (attributeField != null) {
                var field = serializedProperty.serializedObject.targetObject.GetType().GetField(attributeField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null) {
                    return (int) field.GetValue(serializedProperty.serializedObject.targetObject);
                }
                var property = serializedProperty.serializedObject.targetObject.GetType().GetProperty(attributeField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null) {
                    return (int) property.GetValue(serializedProperty.serializedObject.targetObject);
                }
            }
            return attributeValue;
        }
    }
}
