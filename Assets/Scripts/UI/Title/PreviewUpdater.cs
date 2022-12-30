using System.Collections;
using UnityEngine;

public class PreviewUpdater : MonoBehaviour {
    public GameObject current;
    public GameObject next; //can be null

    [Header("Settings")]
    public float pivotLength = 3f;
    public float modelScale = 5f;
    public float duration = 0.5f;
    
    private void Start() {
        current.transform.position = transform.position;
        current.transform.localScale = Vector3.one * modelScale;
    }

    private Vector3 AngledPos(float degrees) {
        Vector3 o = transform.position + Vector3.forward * pivotLength;

        return o + Quaternion.Euler(0, degrees, 0) * (Vector3.forward * pivotLength * -1);
    }

    public void Animate(Level level, bool fromLeft) {
        if(next != null) {
            StopAllCoroutines();
            EndAnimation();
        }

        float m = fromLeft ? -1 : 1;
        next = Instantiate(level.previewPrefab, AngledPos(-90 * m), Quaternion.identity, transform);
        next.transform.localScale = Vector3.zero;

        StartCoroutine(IAnimation(m));
    }

    private void EndAnimation() {
        Destroy(current);
        current = next;
        next = null;

        current.transform.position = transform.position;
        current.transform.localScale = Vector3.one * modelScale;
    }

    IEnumerator IAnimation(float m) {
        float f = 0;
        while (f < 1f) {
            float ff = -1 * (1 - f) * (1 - f) + 1;
            current.transform.localScale = Vector3.one * (1 - ff) * modelScale;
            next.transform.localScale = Vector3.one * ff * modelScale;

            current.transform.position = AngledPos(m * 90 * ff);
            next.transform.position = AngledPos(m * -90 * (1 - ff));

            f += Time.deltaTime / duration;
            yield return null;
        }

        EndAnimation();
    }
}
