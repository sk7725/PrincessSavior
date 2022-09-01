using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlexibleGridLayout), true)] [CanEditMultipleObjects]
public class GridFitterEditor : Editor {
    SerializedProperty rowMode, columnMode, customRowCount, customColumnCount;
    SerializedProperty m_CellSize, m_Padding, m_Spacing, m_StartCorner, m_StartAxis, m_ChildAlignment, m_Constraint, m_ConstraintCount;

    private void OnEnable() {
        rowMode = serializedObject.FindProperty("rowMode");
        columnMode = serializedObject.FindProperty("columnMode");
        customRowCount = serializedObject.FindProperty("customRowCount");
        customColumnCount = serializedObject.FindProperty("customColumnCount");

        m_CellSize = serializedObject.FindProperty("m_CellSize");
        m_Padding = serializedObject.FindProperty("m_Padding");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_StartCorner = serializedObject.FindProperty("m_StartCorner");
        m_StartAxis = serializedObject.FindProperty("m_StartAxis");
        m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        m_Constraint = serializedObject.FindProperty("m_Constraint");
        m_ConstraintCount = serializedObject.FindProperty("m_ConstraintCount");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Padding, true);
        EditorGUILayout.PropertyField(m_CellSize, true);
        EditorGUILayout.PropertyField(m_Spacing, true);
        EditorGUILayout.PropertyField(m_StartCorner, true);
        EditorGUILayout.PropertyField(m_StartAxis, true);
        EditorGUILayout.PropertyField(m_ChildAlignment, true);

        EditorGUILayout.PropertyField(m_Constraint, true);
        if (m_Constraint.enumValueIndex != 0) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_ConstraintCount);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10f);

        EditorGUILayout.PropertyField(rowMode);
        if (rowMode.enumValueIndex == 2) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(customRowCount);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(columnMode);
        if (columnMode.enumValueIndex == 2) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(customColumnCount);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}