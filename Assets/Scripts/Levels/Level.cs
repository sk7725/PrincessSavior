using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 200)]
public class Level : ScriptableObject {
    public int id = -1;

    public string introScene;
    public string gameScene;
    public string endScene;
    public string trueEndScene;

    public int maxGems = 5;

    public GameObject previewPrefab;

    public bool alwaysUnlocked = false;
    public bool hidden = false;

    public bool IsHidden() {
        return hidden; //will be useful later for seasonal levels (probably)
    }
}
