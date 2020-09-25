using System;
using Packages.Rider.Editor.UnitTesting;
using UnityEditor;
using UnityEngine;

// [ExecuteAlways]
public class FpsTest : MonoBehaviour {

    private void OnEnable() {
        // Bullet hell editor?
        EditorApplication.update += Tick;
    }

    private void OnDisable() {
        EditorApplication.update = null;
    }

    private void Tick() {
    }
}