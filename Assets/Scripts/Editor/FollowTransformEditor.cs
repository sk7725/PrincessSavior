using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowTransform), true)]
public class FollowTransformEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        base.OnInspectorGUI();

        GUILayout.Space(10f);
        bool e = GUILayout.Button("Apply");
        if (e) {
            FollowTransform f = (FollowTransform)serializedObject.targetObject;
            f.Update();
        }
    }
}
