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
        private Bounds restrictArea;

        [SerializeField]
        private BoxCollider2D[] layers;

        [SerializeField]
        private bool runInEditMode;

        [SerializeField, UsePropertyInInspector(nameof(UseSpriteMask))]
        private bool useSpriteMask;

        [SerializeField]
        [DisplayAsRange(0, nameof(MaxLayerIndex))]
        private int originLayer;

        private SpriteMask _spriteMask;

        public int MaxLayerIndex => Mathf.Max(layers.Length - 1, 0);
        public GameObject OriginLayerName => layers == null || layers.Length == 0 ? null : layers[originLayer].gameObject;

        public bool UseSpriteMask {
            get => useSpriteMask;
            set {
                useSpriteMask = value;
                UpdateSpriteMask(value);
            }
        }

        private void UpdateSpriteMask(bool spriteMaskEnabled) {
            UpdateLayers();

            foreach (var layer in layers) {
                layer.GetComponentInChildren<SpriteRenderer>().maskInteraction = spriteMaskEnabled ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.None;
            }
            // if (_spriteMask == null) {
            _spriteMask = transform.Find("SpriteMask").GetComponent<SpriteMask>();

            var textureRect = new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height);
            var pivot = Vector2.one * 0.5f;

            Debug.Log(textureRect);
            Debug.Log(pivot);

            _spriteMask.sprite = Sprite.Create(Texture2D.whiteTexture, textureRect, pivot, 4);
            _spriteMask.transform.localScale = layers[0].bounds.extents * 2;
            // }

#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
            EditorUtility.SetDirty(_spriteMask);
#endif
        }

        private Transform _layerContainer;

        private void Awake() {
            UpdateLayers();
        }

        private void UpdateLayers() {
            if (_layerContainer == null) {
                _layerContainer = transform.Find("Layers");
            }
            layers = new BoxCollider2D[_layerContainer.transform.childCount];

            for (var i = 0; i < _layerContainer.transform.childCount; i++) {
                layers[i] = _layerContainer.transform.GetChild(i).GetComponent<BoxCollider2D>();
            }
        }

        private void Update() {
            if (!Application.isPlaying && (!Application.isEditor || !runInEditMode)) {
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

            for (var i = 0; i < layers.Length; i++) {
                var layer = layers[i];
                var position = -cameraPos;

                if (i < originLayer) {
                    // position.x /= (1f / (distancePerLayer * (originLayer - i))) * horizontalSpeedMultiplier;
                    // position.y /= (1f / (distancePerLayer * (originLayer - i))) * verticalSpeedMultiplier;

                    position.x *= distancePerLayer * (originLayer - i) * horizontalSpeedMultiplier;
                    position.y *= distancePerLayer * (originLayer - i) * verticalSpeedMultiplier;
                } else if (i > originLayer) {
                    position.x *= (1f / (distancePerLayer * (i - originLayer))) * horizontalSpeedMultiplier;
                    position.y *= (1f / (distancePerLayer * (i - originLayer))) * verticalSpeedMultiplier;
                }
                layer.transform.position = i == originLayer ? transform.position : position;
            }
        }

        public void ResetPositions() {
            foreach (var layer in layers) {
                layer.transform.position = transform.position;
            }
        }

        public void SortLayerSprites() {
            UpdateSpriteMask(useSpriteMask);
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
            distancePerLayer = Mathf.Max(distancePerLayer, 0.001f);
            originLayer = Mathf.Clamp(originLayer, 0, layers.Length - 1);
        }
    }
}
