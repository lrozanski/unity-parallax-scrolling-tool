using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class RoomBasedParallax : MonoBehaviour {

        [SerializeField]
        private new Transform camera;

        [SerializeField]
        private float distancePerLayer = 1;

        [SerializeField]
        private float horizontalSpeedMultiplier = 1f;

        [SerializeField]
        private float verticalSpeedMultiplier = 1f;

        [SerializeField, UsePropertyInInspector(nameof(RoomBounds))]
        private Bounds roomBounds;

        [SerializeField]
        private Bounds restrictArea;

        [SerializeField]
        private ParallaxLayer[] layers;

        [SerializeField]
        private new bool runInEditMode;

        [SerializeField, UsePropertyInInspector(nameof(UseSpriteMask))]
        private bool useSpriteMask;

        [SerializeField]
        [DisplayAsRange(0, nameof(MaxLayerIndex))]
        private int originLayer;

        public Bounds RoomBounds {
            get => roomBounds;
            set {
                roomBounds = value;
                if (useSpriteMask) {
                    UpdateSpriteMaskRectangle();
                }
            }
        }

        public int MaxLayerIndex => Mathf.Max(layers.Length - 1, 0);
        public GameObject OriginLayer => layers == null || layers.Length == 0 ? null : layers[originLayer].gameObject;
        public int OriginLayerIndex {
            get => originLayer;
            set => originLayer = value;
        }

        private Transform layerContainer;
        private SpriteMask spriteMask;

        public bool UseSpriteMask {
            get => useSpriteMask;
            set {
                useSpriteMask = value;
                UpdateSpriteMask(value);
            }
        }

        private void UpdateSpriteMaskRectangle() {
            const int pixelsPerUnit = 4;
            if (spriteMask == null) {
                spriteMask = transform.Find("SpriteMask").GetComponent<SpriteMask>();

                var textureRect = new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height);
                var pivot = Vector2.one * 0.5f;

                spriteMask.sprite = Sprite.Create(Texture2D.whiteTexture, textureRect, pivot, pixelsPerUnit);
            }
            spriteMask.transform.localScale = roomBounds.extents * (pixelsPerUnit / 2f);
        }

        private void UpdateSpriteMask(bool spriteMaskEnabled) {
            UpdateLayers();

            var spriteRenderers = layerContainer.GetComponentsInChildrenRecursive<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers) {
                spriteRenderer.maskInteraction = spriteMaskEnabled
                    ? SpriteMaskInteraction.VisibleInsideMask
                    : SpriteMaskInteraction.None;
            }
            UpdateSpriteMaskRectangle();
        }

        private void Start() {
            UpdateSpriteMask(useSpriteMask);
        }

        private void UpdateLayers() {
            if (layerContainer == null) {
                layerContainer = transform.Find("Layers");
            }
            layers = new ParallaxLayer[layerContainer.transform.childCount];

            for (var i = 0; i < layerContainer.transform.childCount; i++) {
                layers[i] = layerContainer.transform.GetChild(i).GetComponent<ParallaxLayer>();
            }
        }

        private void Update() {
            if (!Application.isPlaying && (!Application.isEditor || !runInEditMode)) {
                return;
            }
            var roomCenter = transform.position;
            var cameraPos = camera.transform.position;
            cameraPos.x = Mathf.Clamp(cameraPos.x, restrictArea.min.x, restrictArea.max.x);
            cameraPos.y = Mathf.Clamp(cameraPos.y, restrictArea.min.y, restrictArea.max.y);

            var totalDistance = 0f;
            for (var i = originLayer - 1; i >= 0; i--) {
                var layer = layers[i];
                var position = roomCenter;
                totalDistance += layer.distanceFromPreviousLayer;

                position.x = roomCenter.x - cameraPos.x * totalDistance * horizontalSpeedMultiplier;
                position.y = roomCenter.y - cameraPos.y * totalDistance * verticalSpeedMultiplier;
                layer.transform.position = position;
            }
            totalDistance = 0f;
            for (var i = originLayer + 1; i < layers.Length; i++) {
                var layer = layers[i];
                var position = roomCenter;
                totalDistance += layer.distanceFromPreviousLayer;

                position.x = roomCenter.x - cameraPos.x / totalDistance * horizontalSpeedMultiplier;
                position.y = roomCenter.y - cameraPos.y / totalDistance * verticalSpeedMultiplier;
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
                var spriteRenderers = layer.transform.GetComponentsInChildrenRecursive<SpriteRenderer>();

                foreach (var spriteRenderer in spriteRenderers) {
                    spriteRenderer.sortingOrder = s++;
                }
            }
        }

        private void OnDrawGizmosSelected() {
            var position = transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(position + roomBounds.center, roomBounds.size);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position + restrictArea.center, restrictArea.size);
        }

        private void OnValidate() {
            roomBounds.size = new Vector3(
                Mathf.Max(roomBounds.size.x, 0f),
                Mathf.Max(roomBounds.size.y, 0f),
                0f
            );
            restrictArea.size = new Vector3(
                Mathf.Clamp(restrictArea.size.x, 0f, roomBounds.size.x),
                Mathf.Clamp(restrictArea.size.y, 0f, roomBounds.size.y),
                0f
            );
            distancePerLayer = Mathf.Max(distancePerLayer, 0.01f);
            originLayer = Mathf.Clamp(originLayer, 0, layers.Length - 1);
        }
    }
}
