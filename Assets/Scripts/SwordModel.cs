using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordModel : MonoBehaviour
{
    [System.Serializable]
    public class PartModel {
        public MeshFilter filter;
        public MeshRenderer renderer;
    }

    public PartModel blade, handle, accessory;
}
