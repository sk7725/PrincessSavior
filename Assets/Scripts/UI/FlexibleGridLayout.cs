using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Flexible Grid Layout", 11)]
public class FlexibleGridLayout : GridLayoutGroup {
    public enum GridFitterMode {
        none,
        automatic,
        custom
    }

    public GridFitterMode rowMode = GridFitterMode.none;
    public GridFitterMode columnMode = GridFitterMode.none;

    public int customRowCount = 1;
    public int customColumnCount = 1;

    private void UpdateCellSize() {
        if(rowMode != GridFitterMode.none) {
            int n = RowCount();
            if(n > 0) {
                float h = (rectTransform.rect.size.y - padding.vertical - spacing.y * (n - 1)) / n;
                Vector2 v = cellSize;
                v.y = h;
                cellSize = v;
            }
        }

        if (columnMode != GridFitterMode.none) {
            int n = ColumnCount();
            if (n > 0) {
                float w = (rectTransform.rect.size.x - padding.horizontal - spacing.x * (n - 1)) / n;
                Vector2 v = cellSize;
                v.x = w;
                cellSize = v;
            }
        }
    }

    public override void SetLayoutHorizontal() {
        UpdateCellSize();
        base.SetLayoutHorizontal();
    }

    public override void SetLayoutVertical() {
        UpdateCellSize();
        base.SetLayoutVertical();
    }

    private int RowCount() {
        if(rowMode == GridFitterMode.custom) return customRowCount;
        if (constraint == Constraint.FixedRowCount) return constraintCount;
        if (constraint == Constraint.FixedColumnCount) return Mathf.CeilToInt(transform.childCount / (float)constraintCount);
        return -1;
    }

    private int ColumnCount() {
        if (columnMode == GridFitterMode.custom) return customColumnCount;
        if (constraint == Constraint.FixedColumnCount) return constraintCount;
        if (constraint == Constraint.FixedRowCount) return Mathf.CeilToInt(transform.childCount / (float)constraintCount);
        return -1;
    }
}
