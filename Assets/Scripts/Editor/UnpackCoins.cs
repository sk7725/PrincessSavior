using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnpackCoins
{
    [MenuItem("GameObject/Unpack Coins", false, 101)]
    public static void UnpackSelectedCoins() {
        //if it is a prefab unpack it
        GameObject go = Selection.activeGameObject;
        if(PrefabUtility.GetPrefabInstanceStatus(go) != PrefabInstanceStatus.NotAPrefab) {
            PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        }

        //move children
        Transform parent = go.transform.parent;

        while(go.transform.childCount > 0) {
            Transform child = go.transform.GetChild(0);
            child.SetParent(parent);
            child.rotation = Quaternion.identity;
        }

        GameObject.DestroyImmediate(go);
    }

    [MenuItem("GameObject/Unpack Coins", true)]
    public static bool CoinCheck() {
        return Selection.activeGameObject != null && Selection.activeGameObject.transform.childCount > 2 && Selection.activeGameObject.transform.parent != null;
    }
}
