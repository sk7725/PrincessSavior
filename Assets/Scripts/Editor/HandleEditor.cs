using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Handle), true)]
public class HandleEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        base.OnInspectorGUI();

        GUILayout.Space(10f);
        EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
        bool e = GUILayout.Button("Equip");
        if (e) {
            Handle handle = (Handle)serializedObject.targetObject;
            handle.EquipPlayer();
            GameControl.main.player.OnPartUpdate();
        }
        EditorGUI.EndDisabledGroup();
    }
}
