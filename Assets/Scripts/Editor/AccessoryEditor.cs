using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Accessory), true)]
public class AccessoryEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        base.OnInspectorGUI();

        GUILayout.Space(10f);
        EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
        bool e = GUILayout.Button("Equip");
        if (e) {
            Accessory accessory = (Accessory)serializedObject.targetObject;
            accessory.EquipPlayer();
            GameControl.main.player.OnPartUpdate();
        }
        EditorGUI.EndDisabledGroup();
    }
}
