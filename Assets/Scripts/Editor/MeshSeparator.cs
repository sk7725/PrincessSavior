using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshSeparator
{
    [MenuItem("GameObject/Separate Submeshes", false, 100)]
    private static void SeparateSelectedMesh() {
        MeshFilter mf = Selection.activeGameObject.GetComponent<MeshFilter>();
        Mesh mesh = mf.sharedMesh;

        string path = PrepareFolder(Selection.activeGameObject.name);

        for (int i = 0; i < mesh.subMeshCount; i++) {
            Mesh sub = mesh.GenerateSubMesh(i);
            AssetDatabase.CreateAsset(sub, string.Format("{0}/{1}_s{2}.mesh", path, Selection.activeGameObject.name, i));
        }
        AssetDatabase.SaveAssets();
    }

    private static string PrepareFolder(string name) {
        if (!AssetDatabase.IsValidFolder("Assets/Generated")) {
            AssetDatabase.CreateFolder("Assets", "Generated");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Generated/"+name)) {
            AssetDatabase.CreateFolder("Assets/Generated", name);
        }
        return "Assets/Generated/" + name;
    }


    [MenuItem("GameObject/Separate Submeshes", true, 100)]
    private static bool MeshCheck() {
        if (Selection.activeGameObject != null) {
            MeshFilter mf;
            return Selection.activeGameObject.TryGetComponent(out mf) && mf.sharedMesh != null && mf.sharedMesh.subMeshCount > 1;
        }
        return false;
    }
}
