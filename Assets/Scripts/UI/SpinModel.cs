using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinModel : MonoBehaviour
{
    [SerializeField] private float speed = 150;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, speed * Time.unscaledDeltaTime, 0);
    }
}
