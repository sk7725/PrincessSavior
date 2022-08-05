using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Vector3 playerCameraOffset;
    [SerializeField] private float cameraSpeed = 2f;

    public Vector3 targetPos, currentOffset, targetOffset;

    private void Awake() {
        targetOffset = currentOffset = playerCameraOffset;
    }

    public void Init() {
        targetOffset = currentOffset = playerCameraOffset;
        targetPos = GameControl.main.player.transform.position + playerCameraOffset;
    }

    void Update()
    {
        if (GameControl.main.player.state != PlayerControl.State.none) {
            UpdateTarget();
        }

        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * cameraSpeed);

        if (GameControl.main.scrollCamera) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
            if((transform.position - targetPos).sqrMagnitude < 0.01f) {
                GameControl.main.scrollCamera = false;
            }
        }
        else transform.position = targetPos;
    }

    private void UpdateTarget() {
        if (GameControl.main.player.sword.gameObject.activeInHierarchy) {
            targetPos = GameControl.main.player.sword.transform.position + currentOffset;
            targetOffset = new Vector3(0, 0, playerCameraOffset.z);
        }
        else {
            targetPos = GameControl.main.player.transform.position + currentOffset;
            if (GameControl.main.player.landed) {
                targetOffset = playerCameraOffset;
            }
            else {
                targetOffset = new Vector3(0, 0, playerCameraOffset.z);
            }
        }
    }
}
