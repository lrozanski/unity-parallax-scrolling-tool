using UnityEngine;

public class PingPongHorizontally : MonoBehaviour {

    [SerializeField]
    private float limit;

    [SerializeField]
    private float speed;

    private int _direction = 1;
    private Vector2 _origin;

    private void Start() {
        _origin = transform.position;
    }

    private void Update() {
        transform.position += Vector3.right * (_direction * speed * Time.deltaTime);

        if (transform.position.x < -limit) {
            _direction = 1;
        } else if (transform.position.x > limit) {
            _direction = -1;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(_origin - Vector2.left * limit, Vector3.one * 0.25f);
        Gizmos.DrawCube(_origin + Vector2.left * limit, Vector3.one * 0.25f);
    }
}
