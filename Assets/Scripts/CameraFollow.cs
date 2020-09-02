using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset;

    private void Update() {
        transform.position = target.position + offset;
    }
}
