using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "Level Collection", order = 200)]
public class LevelCollection : ScriptableObject {
    public Level[] levels;
}
