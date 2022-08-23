using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(MeshEffectGroup))]
public class MeshEffectEditor : Editor {
    const int PROPS = 3;

    SerializedProperty meshes, gameObject;
    ReorderableList list;
    //public static int selected = -1;
    int row = 0;
    List<MeshCollider> collist = new List<MeshCollider>();

    private void OnEnable() {
        meshes = serializedObject.FindProperty("meshes");
        gameObject = serializedObject.FindProperty("m_GameObject");
        list = new ReorderableList(serializedObject, meshes, true, true, true, true);
        list.drawHeaderCallback = DrawHeader;
        list.drawElementCallback = DrawListItems;
        list.elementHeightCallback = ElementHeight;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        GetColliderList();

        list.DoLayoutList();

        //debug
        EditorGUILayout.IntField("Selected", MeshEffectGroup.editorSelected);

        serializedObject.ApplyModifiedProperties();
    }

    /*
    public void OnSceneGUI() {
        
        if (selected != -1 && Camera.current != null) {
            MeshEffectGroup mg = target as MeshEffectGroup;
            if (mg.meshes.Length <= selected) return;
            Mesh mesh = mg.meshes[selected].collider.sharedMesh;
        }
    }*/

    private void DrawHeader(Rect rect) {
        MeshEffectGroup.editorSelected = -1;
        EditorGUI.LabelField(rect, "Mesh Effects");
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) {
        row = 0;
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty col = element.FindPropertyRelative("collider");
        if (isActive) MeshEffectGroup.editorSelected = index;
        EditorGUI.PropertyField(PRect(rect), col, new GUIContent(string.Format("Collider [#{0} {1}]", GetColliderIndex(col), GetColliderName(col))));
        Row();
        EditorGUI.PropertyField(PRect(rect), element.FindPropertyRelative("hitFx"));
        Row();
        EditorGUI.PropertyField(PRect(rect), element.FindPropertyRelative("hitSound"));
    }

    private float ElementHeight(int index) {
        return EditorGUIUtility.singleLineHeight * PROPS;
    }

    private void Row() {
        row++;
    }

    private Rect PRect(Rect rect) {
        return new Rect(rect.x, rect.y + row * EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight);
    }

    void GetColliderList() {
        GameObject go = gameObject.objectReferenceValue as GameObject;
        go.GetComponents(collist);
    }

    int GetColliderIndex(SerializedProperty element) {
        if (element.objectReferenceValue == null) return -1;
        MeshCollider col = element.objectReferenceValue as MeshCollider;
        return collist.IndexOf(col);
    }

    string GetColliderName(SerializedProperty element) {
        if (element.objectReferenceValue == null) return "Missing";
        return ((MeshCollider)element.objectReferenceValue).sharedMesh.name;
    }
}