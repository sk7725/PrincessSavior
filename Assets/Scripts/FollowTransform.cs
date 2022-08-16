using UnityEngine;

public class FollowTransform : MonoBehaviour {
    [SerializeField] private Transform target;

    public void Update() {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
