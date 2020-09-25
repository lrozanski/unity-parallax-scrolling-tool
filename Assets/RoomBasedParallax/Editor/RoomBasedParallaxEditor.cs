using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [CustomEditor(typeof(RoomBasedParallax))]
    public class RoomBasedParallaxEditor : Editor {

        private const int ActionBoxPadding = 5;

        private bool layersFoldout;
        private GUIStyle actionBoxStyle;

        [MenuItem("GameObject/2D Object/Room Based Parallax")]
        public static void CreateInstance() {
            var prefab = Resources.Load<RoomBasedParallax>(nameof(RoomBasedParallax));
            var instance = Instantiate(prefab);
            instance.name = nameof(RoomBasedParallax);

            Selection.SetActiveObjectWithContext(instance, null);
        }

        public override void OnInspectorGUI() {
            if (actionBoxStyle == null) {
                actionBoxStyle = new GUIStyle(GUI.skin.box) {
                    padding = new RectOffset(ActionBoxPadding, ActionBoxPadding, ActionBoxPadding, ActionBoxPadding)
                };
            }

            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            var iterator = serializedObject.GetIterator();
            for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false) {
                switch (iterator.propertyPath) {
                    case "runInEditMode":
                        continue;
                    case "useSpriteMask":
                        continue;
                    case "layers":
                        DrawLayersPanel();
                        continue;
                }
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath)) {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }

            if (!Selection.activeGameObject.TryGetComponent(out RoomBasedParallax instance)) {
                return;
            }
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(new GUIContent("Origin Layer"), instance.OriginLayer, typeof(GameObject), true);
            EditorGUI.EndDisabledGroup();

            CreateOptionsBox();
            CreateActionsBox(instance);
        }

        private void DrawLayersPanel() {
            var layersProperty = serializedObject.FindProperty("layers");
            var enumerator = layersProperty.GetEnumerator();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            layersFoldout = EditorGUILayout.Foldout(layersFoldout, layersProperty.displayName, true);
            if (layersFoldout) {
                EditorGUI.indentLevel = 1;
                layersProperty.arraySize = EditorGUILayout.DelayedIntField("Size", layersProperty.arraySize);

                var index = 0;
                var originLayerIndex = ((RoomBasedParallax) serializedObject.targetObject).OriginLayerIndex;
                while (enumerator.MoveNext()) {
                    var layer = (SerializedProperty) enumerator.Current;
                    DrawLayer(layer);
                    index++;
                }
            }
            EditorGUILayout.EndVertical();
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawLayer(SerializedProperty layerProperty) {
            var layer = (ParallaxLayer) layerProperty.objectReferenceValue;

            EditorGUILayout.BeginHorizontal();

            layer.distanceFromPreviousLayer = EditorGUILayout.DelayedFloatField("Distance", layer.distanceFromPreviousLayer);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(layer, typeof(ParallaxLayer), true);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        private void CreateOptionsBox() {
            GUILayout.Space(8f);
            GUILayout.BeginVertical(actionBoxStyle);
            GUILayout.Label("Options", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("runInEditMode"), GUILayout.Height(8f));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useSpriteMask"), GUILayout.Height(8f));
            GUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void CreateActionsBox(RoomBasedParallax instance) {
            GUILayout.Space(4f);
            GUILayout.BeginVertical(actionBoxStyle);
            GUILayout.Label("Actions", EditorStyles.boldLabel);

            if (GUILayout.Button("Reset Positions")) {
                instance.ResetPositions();
            }
            if (GUILayout.Button("Sort Layer Sprites")) {
                instance.SortLayerSprites();
            }
            GUILayout.EndVertical();
        }
    }
}
