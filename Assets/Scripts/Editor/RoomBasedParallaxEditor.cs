using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [CustomEditor(typeof(RoomBasedParallax))]
    public class RoomBasedParallaxEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            if (!Selection.activeGameObject.TryGetComponent(out RoomBasedParallax instance)) {
                return;
            }
            GUILayout.BeginVertical();
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
