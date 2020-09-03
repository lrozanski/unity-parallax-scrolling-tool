using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [CustomEditor(typeof(RoomBasedParallax))]
    public class RoomBasedParallaxEditor : Editor {

        [MenuItem("GameObject/2D Object/Room Based Parallax")]
        public static void CreateInstance() {
            var prefab = Resources.Load<RoomBasedParallax>(nameof(RoomBasedParallax));
            var instance = Instantiate(prefab);
            instance.name = nameof(RoomBasedParallax);

            Selection.SetActiveObjectWithContext(instance, null);
        }

        public override void OnInspectorGUI() {
            // EditorGUI.BeginChangeCheck();
            // serializedObject.UpdateIfRequiredOrScript();
            // var iterator = serializedObject.GetIterator();
            // for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false) {
            //     if ("originLayer" == iterator.propertyPath) {
            //         continue;
            //     }
            //     using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath)) {
            //         EditorGUILayout.PropertyField(iterator, true);
            //     }
            // }
            // if (EditorGUI.EndChangeCheck()) {
            //     serializedObject.ApplyModifiedProperties();
            // }
            DrawDefaultInspector();

            if (!Selection.activeGameObject.TryGetComponent(out RoomBasedParallax instance)) {
                return;
            }
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(new GUIContent("Origin Layer Name"), instance.OriginLayerName, typeof(GameObject), true);
            EditorGUI.EndDisabledGroup();

            GUILayout.BeginVertical();
            if (GUILayout.Button("Reset Positions")) {
                instance.ResetPositions();
            }
            if (GUILayout.Button("Sort Layer Sprites")) {
                instance.SortLayerSprites();
            }
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
