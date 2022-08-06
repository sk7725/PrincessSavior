using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollower : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private TrailRenderer trail;

    private bool shown = false;

    void Start()
    {
        shown = target.activeInHierarchy;
        trail.enabled = false;
    }

    void Update()
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
        if(target.activeInHierarchy) trail.enabled = true;
        if (target.activeInHierarchy && !shown) {
            //reset trail
            trail.Clear();
        }

        shown = target.activeInHierarchy;
    }
}
