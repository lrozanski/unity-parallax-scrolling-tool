using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {
    public class ParallaxLayer : MonoBehaviour {

        [SerializeField]
        public float distanceFromPreviousLayer = 1f;

        private void OnValidate() {
            distanceFromPreviousLayer = Mathf.Max(distanceFromPreviousLayer, 0.01f);
        }
    }
}
