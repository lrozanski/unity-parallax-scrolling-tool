using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class RoomBasedParallax : MonoBehaviour {

        [SerializeField]
        private new Camera camera;

        [SerializeField]
        private float distancePerLayer;

        [SerializeField]
        private float horizontalSpeedMultiplier = 1f;

        [SerializeField]
        private float verticalSpeedMultiplier = 1f;

        [SerializeField]
        private bool restrictToBounds;

        [SerializeField]
        private Bounds restrictArea;

        [SerializeField]
        private BoxCollider2D[] layers;

        [SerializeField]
        private bool runInEditMode;

        [SerializeField, UsePropertyInInspector(nameof(UseSpriteMask))]
        private bool useSpriteMask;

        public bool UseSpriteMask {
            get => useSpriteMask;
            set {
                useSpriteMask = value;
                UpdateSpriteMask(value);
            }
        }

        private void UpdateSpriteMask(bool spriteMaskEnabled) {
            foreach (var layer in layers) {
                layer.GetComponentInChildren<SpriteRenderer>().maskInteraction = spriteMaskEnabled ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.None;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        private Transform _layerContainer;

        private void Start() {
            _layerContainer = transform.Find("Layers");
            UpdateLayers();
        }

        private void UpdateLayers() {
            layers = new BoxCollider2D[_layerContainer.childCount];

            for (var i = 0; i < _layerContainer.childCount; i++) {
                layers[i] = _layerContainer.GetChild(i).GetComponent<BoxCollider2D>();
            }
        }

        private void Update() {
            if (Application.isEditor && !runInEditMode) {
                return;
            }
            // var smallestBounds = layers[0].GetComponent<SpriteRenderer>().sprite.bounds;
            var cameraPos = camera.transform.position;
            cameraPos.z = transform.position.z;

            // if (!smallestBounds.Contains(cameraPos)) {
            // return;
            // }
            var restrictBounds = restrictArea;
            cameraPos.x = Mathf.Clamp(cameraPos.x, restrictBounds.min.x, restrictBounds.max.x);
            cameraPos.y = Mathf.Clamp(cameraPos.y, restrictBounds.min.y, restrictBounds.max.y);

            for (var i = 1; i < layers.Length; i++) {
                var layer = layers[i];
                var position = -cameraPos;
                position.x *= (1f / (distancePerLayer * i)) * horizontalSpeedMultiplier;
                position.y *= (1f / (distancePerLayer * i)) * verticalSpeedMultiplier;

                layer.transform.position = position;
            }
        }

        public void ResetPositions() {
            foreach (var layer in layers) {
                layer.transform.position = transform.position;
            }
        }

        public void SortLayerSprites() {
            UpdateLayers();
            
            var s = 0;
            for (var i = layers.Length - 1; i >= 0; i--) {
                var layer = layers[i];
                var spriteRenderers = layer.GetComponentsInChildren<SpriteRenderer>();

                foreach (var spriteRenderer in spriteRenderers) {
                    spriteRenderer.sortingOrder = s++;
                }
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(restrictArea.center, restrictArea.size);
        }

        private void OnValidate() {
            if (distancePerLayer <= 0f) {
                distancePerLayer = 0.0001f;
            }
        }
    }
}
