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
            GameControl.main.player.blade.OnUnequip();
            GameControl.main.player.blade = blade;
            GameControl.main.player.blade.OnEquip();
        }
        EditorGUI.EndDisabledGroup();
    }
}
