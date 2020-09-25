using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    // [CustomPropertyDrawer(typeof(ParallaxLayer))]
    public class ParallaxLayerPropertyDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginChangeCheck();

            var labelRect = new Rect(position.x, position.y, 64f, position.height);
            var distanceRect = new Rect(labelRect.xMax, position.y, 64f, position.height);
            var objectRect = new Rect(distanceRect.xMax, position.y, position.width - 128f, position.height);

            var layer = (ParallaxLayer) property.objectReferenceValue;

            EditorGUI.LabelField(labelRect, "Distance");
            layer.distanceFromPreviousLayer = EditorGUI.FloatField(distanceRect, layer.distanceFromPreviousLayer);
            EditorGUI.PropertyField(objectRect, property);

            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
