using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Blade), true)]
public class BladeEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        base.OnInspectorGUI();

        GUILayout.Space(10f);
        EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
        bool e = GUILayout.Button("Equip");
        if (e) {
            Blade blade = (Blade)serializedObject.targetObject;
            blade.EquipPlayer();
            GameControl.main.player.OnPartUpdate();
        }
        EditorGUI.EndDisabledGroup();
    }
}
