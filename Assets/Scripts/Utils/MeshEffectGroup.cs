using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mesh/Mesh Effect Group", 1)]
public class MeshEffectGroup : MonoBehaviour {
    public MeshEffect[] meshes;

#if UNITY_EDITOR
    public static int editorSelected = -1;

    private void Reset() {
        MeshCollider[] mc = GetComponents<MeshCollider>();
        MeshEffect[] prevmeshes = meshes;

        meshes = new MeshEffect[mc.Length]; //note that since the class is serialized, all the elements are filled
        for (int i = 0; i < mc.Length; i++) {
            //search prevmeshes for existing mesh effects
            if (prevmeshes != null) {
                foreach (MeshEffect e in prevmeshes) {
                    if (e.collider == mc[i]) {
                        meshes[i] = e;
                        break;
                    }
                }
            }

            if (meshes[i] == null) meshes[i] = new MeshEffect(mc[i]);
            //if (meshes[i] == null || meshes[i].collider == null) meshes[i].collider = mc[i];
        }
    }

    private void OnDrawGizmosSelected() {
        if (editorSelected != -1 && editorSelected < meshes.Length && meshes[editorSelected].collider != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireMesh(meshes[editorSelected].collider.sharedMesh, transform.position, transform.rotation, transform.lossyScale);
        }
    }
#endif

    [System.Serializable]
    public class MeshEffect {
        public MeshCollider collider;
        public GameObject hitFx;
        public AudioClip hitSound;

        public MeshEffect(MeshCollider mesh) {
            this.collider = mesh;
        }
    }
}
