using UnityEngine;

public class AdventurerController : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    private int _direction = 1;
    private SpriteRenderer _spriteRenderer;

    private void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.position += Vector3.right * (_direction * speed * Time.deltaTime);

        if (transform.position.x < minX) {
            _direction = 1;
        } else if (transform.position.x > maxX) {
            _direction = -1;
        }
        _spriteRenderer.flipX = _direction < 0;
    }

    private void OnDrawGizmosSelected() {
        var position = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, position.y - 5f), new Vector3(minX, position.y + 5f));
        Gizmos.DrawLine(new Vector3(maxX, position.y - 5f), new Vector3(maxX, position.y + 5f));
    }
}
