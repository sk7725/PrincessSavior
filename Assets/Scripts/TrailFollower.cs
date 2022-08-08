using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollower : MonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected GameObject secondaryTarget;
    public TrailRenderer trail;

    private bool shown = false;
    private float f = 1;

    void Start()
    {
        shown = target.activeInHierarchy;
        trail.enabled = false;
    }

    void LateUpdate()
    {
        transform.position = GetTarget().transform.position;
        transform.rotation = GetTarget().transform.rotation;
        if(target.activeInHierarchy) trail.enabled = true;
        if (target.activeInHierarchy && !shown) {
            //reset trail
            trail.Clear();
            f = trail.widthMultiplier = 1;
        }

        trail.enabled = f > 0.01f;
        trail.widthMultiplier = f;
        f = Mathf.MoveTowards(f, UpdateTarget() ? 1f : 0f, Time.deltaTime * 2f);
        shown = target.activeInHierarchy;
    }

    protected virtual GameObject GetTarget() {
        if (secondaryTarget != null) {
            if (target.activeInHierarchy) return target;
            return secondaryTarget;
        }
        return target;
    }

    protected virtual bool UpdateTarget() {
        return true;
    }
}
